using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Estore2API.Models;
using Estore2API.Models.DTO;

namespace Estore2API.Controllers
{
    public class OrderDetailsController : ODataController
    {
        private readonly eStoreContext _context;

        public OrderDetailsController(eStoreContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
          if (_context.OrderDetails == null)
          {
              return NotFound();
          }
            return await _context.OrderDetails.ToListAsync();
        }

        [EnableQuery]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail([FromRoute] int key)
        {
          if (_context.OrderDetails == null)
          {
              return NotFound();
          }
            var orderDetail = await _context.OrderDetails.FindAsync(key);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }
        
        public async Task<IActionResult> PutOrderDetail([FromRoute] int id, [FromBody] OrderDetailDTO orderDetailDto)
        {
            OrderDetail od = orderDetailDto.GetOrderDetail();
            if (id != od.OrderDetailId)
            {
                return BadRequest();
            }

            _context.Entry(od).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        public async Task<ActionResult<OrderDetail>> PostOrderDetail([FromBody] OrderDetailDTO orderDetailDto)
        {
          if (_context.OrderDetails == null)
          {
              return Problem("Entity set 'eStoreContext.OrderDetails'  is null.");
          }
          OrderDetail od = orderDetailDto.GetOrderDetail();
            _context.OrderDetails.Add(od);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDetail", new { id = od.OrderDetailId }, od);
        }

        [EnableQuery]
        public async Task<IActionResult> DeleteOrderDetail([FromRoute] int key)
        {
            if (_context.OrderDetails == null)
            {
                return NotFound();
            }
            var orderDetail = await _context.OrderDetails.FindAsync(key);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return (_context.OrderDetails?.Any(e => e.OrderDetailId == id)).GetValueOrDefault();
        }
    }
}
