using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Project.Models;

namespace Project.Controllers
{
    public partial class MachineController
    {
        // Get view to Create a new machine
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }
        // Post from view to create a new machine
        [HttpPost("create")]
        public IActionResult Create(MachineModel machine)
        {
            if (ModelState.IsValid)
            {
                if (_context.MachineModels != null)
                {
                    _context.MachineModels.Add(machine);
                }
                else
                {
                    return View("NotFound");
                }
                _context.SaveChanges();
                _cache.Remove(MachinesCacheKey); // Invalidate cache
                return RedirectToAction("List");
            }

            return View(machine);
        }
    }
}
