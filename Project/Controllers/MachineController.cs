using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using QRCoder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace Project.Controllers
{
    [Route("machine")]
    public class MachineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string MachinesCacheKey = "MachinesCacheKey";

        public MachineController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet("list")]
        public IActionResult List(string sortOrder)
        {
            if (!_cache.TryGetValue(MachinesCacheKey, out List<MachineModel>? machines))
            {
                machines = _context.MachineModels.OrderBy(m => m.Name).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(MachinesCacheKey, machines, cacheEntryOptions);
            }

            // Default sorting: ascending by name
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["RoomSortParm"] = sortOrder == "Room" ? "room_desc" : "Room";

            if (machines != null)
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        machines = machines.OrderByDescending(m => m.Name).ToList();
                        break;
                    case "Room":
                        machines = machines.OrderBy(m => m.Room).ToList();
                        break;
                    case "room_desc":
                        machines = machines.OrderByDescending(m => m.Room).ToList();
                        break;
                    default:
                        machines = machines.OrderBy(m => m.Name).ToList();
                        break;
                }
            }
            return View(machines);
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
                _cache.Remove(MachinesCacheKey); // Invalidate cache
                return RedirectToAction("List");
            }

            return View(machine);
        }

        [HttpGet("refresh")]
        public IActionResult Refresh()
        {
            _cache.Remove(MachinesCacheKey); // Invalidate cache
            return RedirectToAction("List");
        }

        // For every machine in the database there needs to be a view "Bio"
        [HttpGet("{room}/{id}")]
        public async Task<IActionResult> Bio(string room, int id)
        {
            var machine = await _context.MachineModels
                .Include(m => m.MachinePdfs) // Ensure MachinePdfs are included
                .FirstOrDefaultAsync(m => m.Id == id && m.Room == room);

            if (machine == null)
            {
                return View("NotFound");
            }

            var qrCodeUrl = Url.Action("Bio", "Machine", new { room, id }, Request.Scheme);
            ViewBag.QrCodeUrl = qrCodeUrl;

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

        [HttpPost]
        public async Task<IActionResult> UploadPdf(int machineId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var machine = await _context.MachineModels
                .Include(m => m.MachinePdfs)
                .FirstOrDefaultAsync(m => m.Id == machineId);

            if (machine == null)
                return NotFound("Machine not found");

            var uploadsFolder = Path.Combine("wwwroot", "pdfs");
            Directory.CreateDirectory(uploadsFolder); // Ensure directory exists
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var pdf = new MachinePdf
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Path = filePath,
                MachineModelId = machineId // Ensure MachineModelId is set correctly
            };

            _context.MachinePdfs.Add(pdf);
            await _context.SaveChangesAsync();

            return RedirectToAction("Bio", new { room = machine.Room, id = machine.Id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPdf(int id)
        {
            var pdf = await _context.MachinePdfs.FindAsync(id);
            if (pdf == null)
                return NotFound();

            var filePath = Path.GetFullPath(pdf.Path);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on server");

            return PhysicalFile(filePath, pdf.ContentType, pdf.FileName);
        }

        public async Task<IActionResult> SyncPdfs(int machineId)
        {
            var machine = await _context.MachineModels
                .Include(m => m.MachinePdfs)
                .FirstOrDefaultAsync(m => m.Id == machineId);

            if (machine == null)
                return NotFound("Machine not found");

            var uploadsFolder = Path.Combine("wwwroot", "pdfs");
            var existingFiles = Directory.GetFiles(uploadsFolder).Select(Path.GetFileName).ToList();

            var pdfsToRemove = machine.MachinePdfs.Where(pdf => !existingFiles.Contains(pdf.FileName)).ToList();

            if (pdfsToRemove.Any())
            {
                _context.MachinePdfs.RemoveRange(pdfsToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Bio", new { room = machine.Room, id = machine.Id });
        }

        // Delete a pdf from a machine
        [HttpPost("deletepdf")]
        public async Task<IActionResult> DeletePdf(int pdfId)
        {
            var pdf = await _context.MachinePdfs
                .Include(p => p.MachineModel) // Ensure MachineModel is included
                .FirstOrDefaultAsync(p => p.Id == pdfId);

            if (pdf == null)
                return NotFound("Pdf not found");

            // Delete the file from the file system
            if (System.IO.File.Exists(pdf.Path))
            {
                System.IO.File.Delete(pdf.Path);
            }

            _context.MachinePdfs.Remove(pdf);
            await _context.SaveChangesAsync();

            return RedirectToAction("Bio", new { room = pdf.MachineModel.Room, id = pdf.MachineModel.Id });
        }
    }

    public enum Size
    {
        small,
        medium,
        large
    }
}