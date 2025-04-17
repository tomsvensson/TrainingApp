using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingApp.Models;

namespace TrainingApp.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ILogger<SessionsController> _logger;
        private readonly Database db;
        public SessionsController(ILogger<SessionsController> logger)
        {
            _logger = logger;
            db = new Database();
        }
        // GET: SessionsContrller
        public IActionResult Index()
        {
            var sessions = db.GetSessions();
            return View(sessions);
        }

        // GET: SessionsContrller/Details/5
        public IActionResult Details(int id)
        {
            var session = db.GetSessions().FirstOrDefault(s => s.Id == id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        // GET: SessionsContrller/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SessionsContrller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Session session)
        {
            if(session.Exercise.Length > 50)
            {
                ModelState.AddModelError("Excercise", "The excercise text cannot be longer than 50 characters.");
            }
            if (session.Sets < 1 || session.Sets > 100)
            {
                ModelState.AddModelError("Sets", "Sets must be a number between 1 and 100.");
            }
            if (session.Reps < 1 || session.Reps > 100)
            {
                ModelState.AddModelError("Reps", "Reps must be a number between 1 and 100.");
            }
            if (session.Weight < 1 || session.Weight > 500)
            {
                ModelState.AddModelError("Weight", "Weight must be a number between 1 and 500.");
            }
            if (ModelState.IsValid)
            {
                db.SaveSession(session.Exercise, session.Sets, session.Reps, session.Weight);
                ModelState.Clear();
                ViewBag.Message = "Session created successfully. You can create another one.";
            }
            return View();
        }

        // GET: SessionsContrller/Edit/5
        public IActionResult Edit(int id)
        {
            var session = db.GetSessions().FirstOrDefault(s => s.Id == id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        // POST: SessionsContrller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Session session)
        {
            if (session.Exercise.Length > 50)
            {
                ModelState.AddModelError("Excercise", "The exercise text cannot be longer than 50 characters.");
            }
            if (session.Sets < 1 || session.Sets > 100)
            {
                ModelState.AddModelError("Sets", "Sets must be a number between 1 and 100.");
            }
            if (session.Reps < 1 || session.Reps > 100)
            {
                ModelState.AddModelError("Reps", "Reps must be a number between 1 and 100.");
            }
            if (session.Weight < 1 || session.Weight > 500)
            {
                ModelState.AddModelError("Weight", "Weight must be a number between 1 and 500.");
            }

            if (ModelState.IsValid)
            {
                db.UpdateSession(session);
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        // GET: SessionsContrller/Delete/5
        public IActionResult Delete(int id)
        {
            var session = db.GetSessions().FirstOrDefault(s => s.Id == id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        // POST: SessionsContrller/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Session session)
        {
            try
            {
                db.DeleteSession(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
