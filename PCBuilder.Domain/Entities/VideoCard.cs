using System;
using System.Collections.Generic;
using System.Text;
using PCBuilder.Domain.Enums;

namespace PCBuilder.Domain.Entities
{
    public class VideoCard : Component
    {
        public int VramGb { get; set; }
        public int LengthMm { get; set; }

        // El requerimiento de conexión (Tu agregado)
        public PcieGeneration PcieGeneration { get; set; }
    }
}