namespace Project.Models
{
    public class MachinePdf
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public required string Path { get; set; }

        public int MachineModelId { get; set; }
        public MachineModel? MachineModel { get; set; }
    }
}