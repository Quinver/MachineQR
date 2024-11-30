using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public partial class MachineController : Controller
    {
        // Delete a pdf from a machine
        [HttpPost("deletepdf")]
        public async Task<IActionResult> DeletePdf(int pdfId)
        {
            if (_context.MachinePdfs == null)
            {
                return View("NotFound");
            }

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

            if (pdf.MachineModel == null)
            {
                return NotFound("Machine model not found");
            }

            InvalidateBioCache(pdf.MachineModel.Room, pdf.MachineModel.Id);

            return RedirectToAction("Bio", new { room = pdf.MachineModel.Room, id = pdf.MachineModel.Id });
        }

        // Delete an image from a machine
        [HttpPost("deleteimage")]
        public async Task<IActionResult> DeleteImage(int machineId)
        {
            if (_context.MachineModels == null)
            {
                return View("NotFound");
            }

            var machine = await _context.MachineModels.FindAsync(machineId);
            if (machine == null)
                return NotFound("Machine not found");

            if (string.IsNullOrEmpty(machine.ImageUrl))
            {
                return NotFound("Image URL is null or empty");
            }

            if (System.IO.File.Exists(machine.ImageUrl))
            {
                System.IO.File.Delete(machine.ImageUrl);
            }

            machine.ImageUrl = null;
            await _context.SaveChangesAsync();

            InvalidateBioCache(machine.Room, machine.Id);

            return RedirectToAction("Bio", new { room = machine.Room, id = machine.Id });
        }

        // Delete a machine
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var machine = _context.MachineModels?.Find(id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MachineModels == null)
            {
                return NotFound();
            }
            var machine = await _context.MachineModels
                .Include(m => m.MachinePdfs) // Include related PDFs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (machine == null)
            {
                return NotFound();
            }

            // Delete the PDFs from the file system
            foreach (var pdf in machine.MachinePdfs)
            {
                if (System.IO.File.Exists(pdf.Path))
                {
                    System.IO.File.Delete(pdf.Path);
                }
            }

            // Remove the PDFs from the database
            _context.MachinePdfs?.RemoveRange(machine.MachinePdfs);

            // Delete the image from the file system
            if (!string.IsNullOrEmpty(machine.ImageUrl))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", machine.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Remove the machine from the database
            _context.MachineModels.Remove(machine);
            await _context.SaveChangesAsync();

            InvalidateListCache();
            return RedirectToAction("List");
        }
    }
}
