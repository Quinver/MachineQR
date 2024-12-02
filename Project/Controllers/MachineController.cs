using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using QRCoder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;

namespace Project.Controllers
{
    [Route("machine")]
    public partial class MachineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string MachinesCacheKey = "MachinesCacheKey";

        public MachineController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        private void InvalidateBioCache(string room, int id)
        {
            string cacheKey = $"Machine_{room}_{id}";
            _cache.Remove(cacheKey);
        }
        private void InvalidateListCache()
        {
            _cache.Remove(MachinesCacheKey);
        }
    }
}