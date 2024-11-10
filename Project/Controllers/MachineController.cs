using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class MachineController : Controller
    {
        private readonly ILogger<MachineController> _logger;
        private readonly ApplicationDbContext _context;

        public MachineController(ILogger<MachineController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<TestEntity> machines;
            try
            {
                machines = _context.TestEntities.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching machines from the database.");
                machines = new List<TestEntity>();
            }
            return View(machines);
        }
    }
}