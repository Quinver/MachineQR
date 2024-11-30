using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Project.Models;

namespace Project.Controllers
{
    public partial class MachineController
    {
        // For every machine in the database there needs to be a view "Bio"
        [HttpGet("{room}/{id}")]
        public async Task<IActionResult> Bio(string room, int id)
        {
            string cacheKey = $"Machine_{room}_{id}";
            if (!_cache.TryGetValue(cacheKey, out MachineModel? machine))
            {
                if (_context.MachineModels == null)
                {
                    return View("NotFound");
                }

                machine = await _context.MachineModels
                    .Include(m => m.MachinePdfs) // Ensure MachinePdfs are included
                    .FirstOrDefaultAsync(m => m.Id == id && m.Room == room);

                if (machine == null)
                {
                    return View("NotFound");
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Adjust the expiration time as needed

                _cache.Set(cacheKey, machine, cacheEntryOptions);
            }

            var qrCodeUrl = Url.Action("Bio", "Machine", new { room, id }, Request.Scheme);
            ViewBag.QrCodeUrl = qrCodeUrl;

            return View(machine);
        }
    }
}
