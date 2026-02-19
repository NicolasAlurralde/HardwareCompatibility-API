using System;
using System.Collections.Generic;
using System.Text;
using PCBuilder.Domain.Enums;
namespace PCBuilder.Domain.Entities
{
    public class Ram : Component
    {
        public RamType Type { get; set; } // Ej: DDR4
        public int SpeedMhz { get; set; } // Ej: 3200
        public int CapacityGb { get; set; } // Ej: 32
        public int ModulesCount { get; set; } // Ej: 2 (si es un kit de 2x16GB)
    }
}
