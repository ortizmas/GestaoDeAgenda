using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoDeAgenda.Models;
using Microsoft.AspNetCore.Http;

namespace GestaoDeAgenda.Controllers
{
    public class EventController : Controller
    {
        private readonly ProjectContext _context;

        public EventController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.OrderBy(e => e.Date).ToListAsync());
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Type,Name,Description,Date,Local,CreatedAt,UpdatedAt")] Event @event)
        {
            var eventType = @event.Type;
            var eventDate = @event.Date;

            if (eventType == 1)
            {
                var events = _context.Events.Where(e => e.Type.Equals(eventType) && e.Date.Equals(eventDate)).ToList();
                if (events.Count() > 0)
                {
                    ViewBag.error = "Já existe um evento na data selecionada";
                    return View(@event);
                }
                
            }
            

            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                _context.Add(@event);

                @event.UserEvent = new List<UserEvent>
                {
                    new UserEvent
                    {
                        UserId = (int)userId
                    }
                };
                // _context.SaveChanges();
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Type,Name,Description,Date,Local,CreatedAt,UpdatedAt")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            return View(@event);
        }

        // GET: Event/Edit/5
        public IActionResult Participate(int id)
        {
            var UserEvents = new UserEvent();
            var userId = HttpContext.Session.GetInt32("UserId");
            var check = _context.UserEvents.Where(ue => ue.EventId.Equals(id) && ue.UserId.Equals(userId)).ToList();

            if (check.Count() == 0)
            {
                
                UserEvents.UserId = (int)userId;
                UserEvents.EventId = (int)id;
                _context.Add(UserEvents);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }
            else
            {

                ViewBag.error = "Você jâ esta participando do evento";
                return RedirectToAction("Index");

            }
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
