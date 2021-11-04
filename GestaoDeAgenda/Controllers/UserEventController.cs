using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoDeAgenda.Models;

namespace GestaoDeAgenda.Controllers
{
    public class UserEventController : Controller
    {
        private readonly ProjectContext _context;

        public UserEventController(ProjectContext context)
        {
            _context = context;
        }

        // GET: UserEvent
        public async Task<IActionResult> Index()
        {
            var projectContext = _context.UserEvents.Include(u => u.Event).Include(u => u.User);
            return View(await projectContext.ToListAsync());
        }

        // GET: UserEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents
                .Include(u => u.Event)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userEvent == null)
            {
                return NotFound();
            }

            return View(userEvent);
        }

        // GET: UserEvent/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: UserEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,EventId")] UserEvent userEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", userEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userEvent.UserId);
            return View(userEvent);
        }

        // GET: UserEvent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents.FindAsync(id);
            if (userEvent == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", userEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userEvent.UserId);
            return View(userEvent);
        }

        // POST: UserEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,EventId")] UserEvent userEvent)
        {
            if (id != userEvent.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserEventExists(userEvent.UserId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", userEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userEvent.UserId);
            return View(userEvent);
        }

        // GET: UserEvent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents
                .Include(u => u.Event)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userEvent == null)
            {
                return NotFound();
            }

            return View(userEvent);
        }

        // POST: UserEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userEvent = await _context.UserEvents.FindAsync(id);
            _context.UserEvents.Remove(userEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserEventExists(int id)
        {
            return _context.UserEvents.Any(e => e.UserId == id);
        }
    }
}
