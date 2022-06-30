using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirportApplication.Data;
using AirportApplication.Models;
using AirportApplication.ViewModels;

namespace AirportApplication.Controllers
{
    public class PilotsController : Controller
    {
        private readonly AirportApplicationContext _context;

        public PilotsController(AirportApplicationContext context)
        {
            _context = context;
        }

        // GET: Pilots
        public async Task<IActionResult> Index(string FullName, string Rank, string Company)
        {
            IQueryable<Pilot> pilotsQuery = _context.Pilot.AsQueryable().Include(x => x.Company);
            IQueryable<string> ranksQuery = _context.Pilot.OrderBy(x => x.Rank).Select(x => x.Rank).Distinct();
            IQueryable<string> companyQuery = _context.Pilot.OrderBy(x => x.Company.Title).Select(x => x.Company.Title).Distinct();

            if (!string.IsNullOrEmpty(FullName))
            {
                if (FullName.Contains(" "))
                {
                    string[] names = FullName.Split(" ");
                    pilotsQuery = pilotsQuery.Where(x => x.FirstName.Contains(names[0]) || x.LastName.Contains(names[1]) ||
                    x.FirstName.Contains(names[1]) || x.LastName.Contains(names[0]));
                }
                else
                {
                    pilotsQuery = pilotsQuery.Where(x => x.FirstName.Contains(FullName) || x.LastName.Contains(FullName));
                }
            }
            if (!string.IsNullOrEmpty(Rank))
            {
                pilotsQuery = pilotsQuery.Where(x => x.Rank.Contains(Rank));
            }
            if (!string.IsNullOrEmpty(Company))
            {
                pilotsQuery = pilotsQuery.Where(x => x.Company.Title.Contains(Company));
            }

            var PilotFilterVM = new PilotFilter
            {
                Pilots = await pilotsQuery.Include(x => x.Company).ToListAsync(),
                Ranks = new SelectList(await ranksQuery.ToListAsync()),
                Companies = new SelectList(await companyQuery.ToListAsync())
            };

            return View(PilotFilterVM);
        }

        // GET: Pilots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pilot == null)
            {
                return NotFound();
            }

            var pilot = await _context.Pilot
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pilot == null)
            {
                return NotFound();
            }

            return View(pilot);
        }

        // GET: Pilots/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Country");
            return View();
        }

        // POST: Pilots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Rank,DateOfBirth,Nationality,ProfilePicture,CompanyId")] Pilot pilot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pilot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Country", pilot.CompanyId);
            return View(pilot);
        }

        // GET: Pilots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pilot == null)
            {
                return NotFound();
            }

            var pilot = await _context.Pilot.FindAsync(id);
            if (pilot == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Country", pilot.CompanyId);
            return View(pilot);
        }

        // POST: Pilots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Rank,DateOfBirth,Nationality,ProfilePicture,CompanyId")] Pilot pilot)
        {
            if (id != pilot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pilot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PilotExists(pilot.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Country", pilot.CompanyId);
            return View(pilot);
        }

        // GET: Pilots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pilot == null)
            {
                return NotFound();
            }

            var pilot = await _context.Pilot
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pilot == null)
            {
                return NotFound();
            }

            return View(pilot);
        }

        // POST: Pilots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pilot == null)
            {
                return Problem("Entity set 'AirportApplicationContext.Pilot'  is null.");
            }
            var pilot = await _context.Pilot.FindAsync(id);
            if (pilot != null)
            {
                _context.Pilot.Remove(pilot);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PilotExists(int id)
        {
          return (_context.Pilot?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
