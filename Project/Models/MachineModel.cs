using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class MachineModel
    {
        public int Id  { get; set; }
        public string Name { get; set; } = "Change Name?";
        public string Room { get; set; } = "Change Room?";
        public string? Description { get; set; }
    }
}