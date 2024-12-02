using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public partial class MachineController
    {
        [HttpPost("upload-pdf")]
        public async Task<IActionResult> UploadPdf(int machineId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files selected");

            if (_context.MachineModels == null)
            {
                return NotFound("Machine models not found");
            }

            var machine = await _context.MachineModels
                .Include(m => m.MachinePdfs)
                .FirstOrDefaultAsync(m => m.Id == machineId);

            if (machine == null)
                return NotFound("Machine not found");

            var uploadsFolder = Path.Combine("wwwroot", "pdfs");
            Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
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

                    if (_context.MachinePdfs != null)
                    {
                        _context.MachinePdfs.Add(pdf);
                    }
                    else
                    {
                        return View("NotFound");
                    }
                }
            }

            InvalidateBioCache(machine.Room, machine.Id);

            await _context.SaveChangesAsync();

            return RedirectToAction("Bio", new { room = machine.Room, id = machine.Id });
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(int machineId, IFormFile image)
        {
            if (image == null)
                return BadRequest("No image selected");

            if (_context.MachineModels == null)
            {
                return NotFound("Machine models not found");
            }

            var machine = await _context.MachineModels.FindAsync(machineId);
            if (machine == null)
                return NotFound("Machine not found");

            var uploadsFolder = Path.Combine("wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

            if (image.Length > 0)
            {
                // Delete the existing image if it exists
                if (!string.IsNullOrEmpty(machine.ImageUrl) && System.IO.File.Exists(Path.Combine("wwwroot", machine.ImageUrl)))
                {
                    System.IO.File.Delete(Path.Combine("wwwroot", machine.ImageUrl));
                }

                var relativePath = Path.Combine("images", image.FileName);
                var filePath = Path.Combine(uploadsFolder, image.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                machine.ImageUrl = relativePath;
                await _context.SaveChangesAsync();
            }

            InvalidateBioCache(machine.Room, machine.Id);

            return RedirectToAction("Bio", new { room = machine.Room, id = machine.Id });
        }

        [HttpGet("pdf/{id}")]
        public async Task<IActionResult> GetPdf(int id)
        {
            if (_context.MachinePdfs == null)
            {
                return View("NotFound");
            }

            var pdf = await _context.MachinePdfs.FindAsync(id);
            if (pdf == null)
                return NotFound();

            var filePath = Path.GetFullPath(pdf.Path);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on server");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            // Manually set headers to force inline viewing
            Response.Headers.Append("Content-Disposition", $"inline; filename=\"{pdf.FileName}\"");
            return File(fileBytes, pdf.ContentType);
        }

    }
}
