using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using QRCoder;
using System.Drawing;
using System.IO;

namespace Project.Controllers
{
    [Route("machine")]
    public class MachineController : Controller
    {
        private readonly ILogger<MachineController> _logger;
        private readonly ApplicationDbContext _context;

        public MachineController(ILogger<MachineController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("list")]
        public IActionResult List(string sortOrder)
        {
            var machines = _context.MachineModels.AsQueryable();

            // Default sorting: ascending by name
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["RoomSortParm"] = sortOrder == "Room" ? "room_desc" : "Room";

            switch (sortOrder)
            {
                case "name_desc":
                    machines = machines.OrderByDescending(m => m.Name);
                    break;
                case "Room":
                    machines = machines.OrderBy(m => m.Room);
                    break;
                case "room_desc":
                    machines = machines.OrderByDescending(m => m.Room);
                    break;
                default:
                    machines = machines.OrderBy(m => m.Name);
                    break;
            }

            return View(machines.ToList());
        }

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
                _context.MachineModels.Add(machine);
                _context.SaveChanges();
                return RedirectToAction("List");
            }

            return View(machine);
        }

        // For every machine in the database there needs to be a view "Bio"
        [HttpGet("{room}/{id}")]
        public IActionResult Bio(string room, int id)
        {
            // Get the machine with the specified ID and room
            var machine = _context.MachineModels.FirstOrDefault(m => m.Id == id && m.Room == room);

            // If the machine does not exist, return a 404 Not Found response
            if (machine == null)
            {
                return View("NotFound");
            }

            // Generate QR code for the machine bio page
            var qrCodeUrl = Url.Action("Bio", "Machine", new { room, id }, Request.Scheme);
            ViewBag.QrCodeUrl = qrCodeUrl;

            // Pass the machine to the view (Bio.cshtml)
            return View(machine);
        }

        [HttpGet("qrcode/{room}/{id}")]
        public IActionResult GenerateQrCode(string room, int id, Size size = Size.small)
        {
            int dotSize = 5;
            switch (size)
            {
                case Size.small:
                    dotSize = 5;
                    break;
                case Size.medium:
                    dotSize = 10;
                    break;
                case Size.large:
                    dotSize = 15;
                    break;
                default:
                    return BadRequest("Invalid size parameter.");
            }

            var qrCodeUrl = Url.Action("Bio", "Machine", new { room, id }, Request.Scheme);
            using (var qrGenerator = new QRCodeGenerator())
            {
                if (string.IsNullOrEmpty(qrCodeUrl))
                {
                    return BadRequest("QR code URL is null or empty.");
                }

                var qrCodeData = qrGenerator.CreateQrCode(qrCodeUrl, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                var qrCodeBytes = qrCode.GetGraphic(dotSize);

                return File(qrCodeBytes, "image/png");
            }
        }
    }

    public enum Size
    {
        small,
        medium,
        large
    }
}