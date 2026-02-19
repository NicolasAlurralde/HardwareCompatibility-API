using System;
using System.Collections.Generic;
using System.Text;
using PCBuilder.Domain.Enums;

namespace PCBuilder.Domain.Entities
{
    public class PcCase : Component
    {
        public FormFactor MaxMotherboardSize { get; set; } // Ej: Si soporta ATX, por retrocompatibilidad soporta MicroATX
        public int MaxGpuLengthMm { get; set; } // Límite para la VideoCard
        public int MaxCpuCoolerHeightMm { get; set; } // Límite para disipadores por aire

        // Soporte para Refrigeración Líquida
        public bool SupportsLiquidCooling { get; set; }
        public int MaxRadiatorSizeMm { get; set; } // Ej: 240, 360 (para validar contra el Cooler)

        // Soporte de Fuente de Poder (PSU)
        public PsuFormFactor SupportedPsuFormFactor { get; set; } // Ej: ATX o SFX
        public int MaxPsuLengthMm { get; set; } // Límite de largo para la fuente
    }
}
