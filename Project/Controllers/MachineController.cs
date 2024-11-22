using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    [Route("machine/room/{id}")]
    public class MachineController : Controller
    {
        private readonly ILogger<MachineController> _logger;
        private readonly ApplicationDbContext _context;

        public MachineController(ILogger<MachineController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(MachineModel model)
        {
            if (ModelState.IsValid)
            {
                _context.MachineModels.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        // For every machine in the database there needs to be a view "Bio"
        [HttpGet]
        public IActionResult Bio(int id)
        {
            // Get the machine with the specified ID
            var machine = _context.MachineModels.FirstOrDefault(m => m.Id == id);

            // If the machine does not exist, return a 404 Not Found response
            if (machine == null)
            {
                return View("NotFound");
            }

            // Pass the machine to the view (Bio.cshtml)
            return View(machine);
        }
    }
}