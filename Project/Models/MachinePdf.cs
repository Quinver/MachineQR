namespace Project.Models
{
    public class MachinePdf
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }

        public int MachineModelId { get; set; }
        public MachineModel MachineModel { get; set; }
    }
}