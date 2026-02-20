using System;

namespace PCBuilder.Domain.Entities
{
    public class PCBuild
    {
        public int Id { get; set; }
        public string BuildName { get; set; } = string.Empty; // Ej: "PC Gamer 1080p" o "Workstation"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Relaciones Obligatorias (Lo mínimo para que una PC encienda) ---

        public int ProcessorId { get; set; }
        public Processor Processor { get; set; } = null!;

        public int MotherboardId { get; set; }
        public Motherboard Motherboard { get; set; } = null!;

        public int RamId { get; set; }
        public Ram Ram { get; set; } = null!;
        public int RamQuantity { get; set; } = 1; // Para saber si compró 1 o 2 módulos iguales

        public int PowerSupplyId { get; set; }
        public PowerSupply PowerSupply { get; set; } = null!;

        public int PcCaseId { get; set; }
        public PcCase PcCase { get; set; } = null!;

        public int StorageId { get; set; }
        public Storage Storage { get; set; } = null!;

        // --- Relaciones Opcionales ---

        // Es opcional porque el procesador puede tener gráficos integrados (Ej: Ryzen 5 5600G)
        public int? VideoCardId { get; set; }
        public VideoCard? VideoCard { get; set; }

        // Es opcional porque el procesador puede traer el cooler de fábrica (Stock Cooler)
        public int? CoolerId { get; set; }
        public Cooler? Cooler { get; set; }


        // --- Lógica de Negocio Integrada ---

        // Propiedad calculada: No se guarda en la base de datos, se calcula al vuelo sumando las partes
        public decimal TotalPrice =>
            (Processor?.Price ?? 0) +
            (Motherboard?.Price ?? 0) +
            ((Ram?.Price ?? 0) * RamQuantity) +
            (PowerSupply?.Price ?? 0) +
            (PcCase?.Price ?? 0) +
            (Storage?.Price ?? 0) +
            (VideoCard?.Price ?? 0) +
            (Cooler?.Price ?? 0);
    }
}
