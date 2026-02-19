using System;
using System.Collections.Generic;
using System.Text;
using PCBuilder.Domain.Enums;
namespace PCBuilder.Domain.Entities
{
    public class Storage : Component
    {
        public StorageInterfaceType InterfaceType { get; set; } // SATA o M2
        public int CapacityGb { get; set; } // Ej: 1000 (1TB)
        public int ReadSpeedMb { get; set; } // Opcional, pero genial para mostrar en el frontend
    }
}
