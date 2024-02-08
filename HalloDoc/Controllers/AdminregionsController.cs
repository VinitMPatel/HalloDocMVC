using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HalloDoc.Models;

namespace HalloDoc.Controllers
{
    public class AdminregionsController : Controller
    {
        private readonly HalloDocMvcContext _context;

        public AdminregionsController(HalloDocMvcContext context)
        {
            _context = context;
        }

        // GET: Adminregions
        public IActionResult region()
        {

            IEnumerable<Adminregion> objlist = _context.Adminregions;
            return View(objlist);
              //return _context.Adminregions != null ? 
              //            View(await _context.Adminregions.ToListAsync()) :
              //            Problem("Entity set 'HalloDocMvcContext.Adminregions'  is null.");
        }

        // GET: Adminregions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Adminregions == null)
            {
                return NotFound();
            }

            var adminregion = await _context.Adminregions
                .FirstOrDefaultAsync(m => m.Adminregionid == id);
            if (adminregion == null)
            {
                return NotFound();
            }

            return View(adminregion);
        }

        // GET: Adminregions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Adminregions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Adminregionid,Adminid,Regionid")] Adminregion adminregion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminregion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminregion);
        }

        // GET: Adminregions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Adminregions == null)
            {
                return NotFound();
            }

            var adminregion = await _context.Adminregions.FindAsync(id);
            if (adminregion == null)
            {
                return NotFound();
            }
            return View(adminregion);
        }

        // POST: Adminregions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Adminregionid,Adminid,Regionid")] Adminregion adminregion)
        {
            if (id != adminregion.Adminregionid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminregion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminregionExists(adminregion.Adminregionid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adminregion);
        }

        // GET: Adminregions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Adminregions == null)
            {
                return NotFound();
            }

            var adminregion = await _context.Adminregions
                .FirstOrDefaultAsync(m => m.Adminregionid == id);
            if (adminregion == null)
            {
                return NotFound();
            }

            return View(adminregion);
        }

        // POST: Adminregions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Adminregions == null)
            {
                return Problem("Entity set 'HalloDocMvcContext.Adminregions'  is null.");
            }
            var adminregion = await _context.Adminregions.FindAsync(id);
            if (adminregion != null)
            {
                _context.Adminregions.Remove(adminregion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminregionExists(int id)
        {
          return (_context.Adminregions?.Any(e => e.Adminregionid == id)).GetValueOrDefault();
        }
    }
}
