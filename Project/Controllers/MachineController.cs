using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using QRCoder;

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

            // Pass the machine to the view (Bio.cshtml)
            return View(machine);
        }

        public IActionResult GenerateQRCode(string url)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            return File(ms.ToArray(), "image/png");
                        }
                }
                
            }
        }

        public IActionResult GenerateQr(int machineId)
        {
            string url = Url.Action("Bio", "Machine", new { id = machineId }, Request.Scheme);
            return GenerateQRCode(url);
        }

    }
}