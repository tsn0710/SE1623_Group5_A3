using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Estore2API.Models;
using Estore2API.Models.DTO;
using Newtonsoft.Json.Linq;

namespace Estore2API.Controllers
{
    public class OrdersController : ODataController
    {
        private readonly eStoreContext _context;

        public OrdersController(eStoreContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.Include("OrderDetails").ToListAsync();
        }

        [EnableQuery]
        public async Task<ActionResult<Order>> GetOrder([FromRoute] int key)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.Include("OrderDetails").Where(o=>o.OrderId==key).FirstAsync();
            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        public async Task<IActionResult> PutOrder([FromRoute] int key, [FromBody] OrderDTO orderDto)
        {
            var data = JObject.Parse(orderDto.RequiredDate);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Order order1 = System.Text.Json.JsonSerializer.Deserialize<Order>(data.ToString(), options);
            if (key != order1.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task<ActionResult<Order>> PostOrder([FromBody] OrderDTO orderDto)
        {
            //string rawContent = string.Empty;
            //using (var reader = new StreamReader(Request.Body,
            //              encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
            //{
            //    rawContent = await reader.ReadToEndAsync();
            //}
            //var data = JObject.Parse(rawContent);
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true,
            //};
            //Order order1 = System.Text.Json.JsonSerializer.Deserialize<Order>(data.ToString(), options);
            Order order1 = orderDto.GetOrder();
            if (_context.Orders == null)
          {
              return Problem("Entity set 'eStoreContext.Orders'  is null.");
          }

            _context.Orders.Add(order1);
            await _context.SaveChangesAsync();
            return Ok(order1.OrderId);
        }
        
        public async Task<IActionResult> DeleteOrder([FromODataUri] int key)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(key);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
