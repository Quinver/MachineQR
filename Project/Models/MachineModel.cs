using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class MachineModel
    {
        public int Id  { get; set; }

        [Display(Name = "Naam")]
        public string Name { get; set; } = "Change Name?";

        [Display(Name = "Lokaal")]
        public string Room { get; set; } = "Change Room?";
        
        [Display(Name = "Beschrijving")]
        public string? Description { get; set; }

        public List<MachinePdf> MachinePdfs { get; set; } = new List<MachinePdf>();
    }
}