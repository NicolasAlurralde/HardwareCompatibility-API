using System;
using System.Collections.Generic;
using System.Text;
using PCBuilder.Domain.Enums;

namespace PCBuilder.Domain.Entities
{
    public class PowerSupply : Component
    {
        public int Wattage { get; set; }
        public EfficiencyRating Certification { get; set; }

        // Físico y Cables
        public PsuFormFactor FormFactor { get; set; }
        public int LengthMm { get; set; }

        // ¡Tu nueva idea!
        public PsuModularity Modularity { get; set; }
    }
}
