using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Project.Models;

namespace Project.Controllers
{
    public partial class MachineController
    {
        [HttpGet("list")]
        public IActionResult List(string sortOrder)
        {
            if (!_cache.TryGetValue(MachinesCacheKey, out List<MachineModel>? machines))
            {
                machines = _context.MachineModels?.OrderBy(m => m.Name).ToList() ?? new List<MachineModel>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(MachinesCacheKey, machines, cacheEntryOptions);
            }

            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["RoomSortParm"] = sortOrder == "Room" ? "room_desc" : "Room";

            if (machines != null)
            {
                machines = sortOrder switch
                {
                    "name_desc" => machines.OrderByDescending(m => m.Name).ToList(),
                    "Room" => machines.OrderBy(m => m.Room).ToList(),
                    "room_desc" => machines.OrderByDescending(m => m.Room).ToList(),
                    _ => machines.OrderBy(m => m.Name).ToList(),
                };
            }

            return View(machines);
        }

        [HttpGet("refreshList")]
        public IActionResult RefreshList()
        {
            InvalidateListCache(); // Invalidate cache for machines
            return RedirectToAction("List");
        }
    }
}
