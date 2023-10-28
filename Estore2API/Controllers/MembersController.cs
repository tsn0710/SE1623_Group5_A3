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

namespace Estore2API.Controllers
{
    public class MembersController : ODataController
    {
        private readonly eStoreContext _context;

        public MembersController(eStoreContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
          if (_context.Members == null)
          {
              return NotFound();
          }
            return await _context.Members.ToListAsync();
        }

        [EnableQuery]
        public async Task<ActionResult<Member>> GetMember([FromRoute] int key)
        {
          if (_context.Members == null)
          {
              return NotFound();
          }
            var member = await _context.Members.FindAsync(key);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        public async Task<IActionResult> PutMember([FromRoute] int key, [FromBody] Member member)
        {
            if (key != member.MemberId)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
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

        public async Task<ActionResult<Member>> PostMember([FromBody] Member member)
        {
            if (_context.Members == null)
            {
                return Problem("Entity set 'eStoreContext.Members'  is null.");
            }
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        public async Task<IActionResult> DeleteMember([FromRoute] int key)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(key);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
