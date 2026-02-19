using System;
using System.Collections.Generic;
using System.Text;
using PCBuilder.Domain.Enums;

namespace PCBuilder.Domain.Entities
{
    public class Cooler : Component
    {
        public CoolerType Type { get; set; }

        // Dimensiones dependiendo del tipo
        public int HeightMm { get; set; } // Relevante si es Air Cooler
        public int RadiatorSizeMm { get; set; } // Relevante si es Liquid Cooler

        // Estética y Conexión
        public bool HasArgb { get; set; } // Para validar si la Motherboard tiene el conector

        // Un cooler incluye adaptadores para múltiples plataformas
        public List<SocketType> SupportedSockets { get; set; } = new List<SocketType>();
    }
}
