using System;
using System.Collections.Generic;
using System.Text;
using PCBuilder.Domain.Enums;

namespace PCBuilder.Domain.Entities
{
    public class Motherboard : Component
    {
        public SocketType Socket { get; set; }
        public RamType SupportedRam { get; set; }
        public int RamSlots { get; set; }

        public int MaxRamCapacityGb { get; set; } // Límite de memoria (Ej: 32, 64, 128)

        // Almacenamiento
        public int M2Slots { get; set; }
        public int SataSlots { get; set; }

        // Expansión y Video (Tus agregados)
        public int PcieX16Slots { get; set; } // Cuántas placas de video físicas entran
        public PcieGeneration PcieGeneration { get; set; } // Versión del bus (Ej: Gen3, Gen4)

        // Factor de forma y Estética (Tus agregados)
        public FormFactor FormFactor { get; set; } // Para comparar con el PcCase
        public bool HasArgbHeaders { get; set; } // Para comparar con el Cooler o el PcCase
    }
}
