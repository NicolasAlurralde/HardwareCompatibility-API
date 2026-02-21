namespace PCBuilder.API.DTOs
{
    // Este es el "molde" limpio que Swagger le mostrará al frontend
    public class PCBuildRequestDto
    {
        public string BuildName { get; set; } = string.Empty;

        // --- PIEZAS OBLIGATORIAS (Sin el ?) ---
        public int ProcessorId { get; set; }
        public int MotherboardId { get; set; }

        // --- PIEZAS OPCIONALES (Con el ?) ---
        public int? RamId { get; set; }
        public int RamQuantity { get; set; } = 0;

        public int? PowerSupplyId { get; set; }
        public int? PcCaseId { get; set; }

        public int? StorageId { get; set; }
        public int StorageQuantity { get; set; } = 0;

        public int? SecondaryStorageId { get; set; }
        public int SecondaryStorageQuantity { get; set; } = 0;

        public int? VideoCardId { get; set; }
        public int? SecondaryVideoCardId { get; set; }

        public int? CoolerId { get; set; }
    }
}
