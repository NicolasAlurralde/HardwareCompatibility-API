using System;
using System.Collections.Generic;
using System.Text;

namespace PCBuilder.Domain.Entities
{
    public abstract class Component
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // El consumo en Watts es clave para validar si la Fuente de Poder soporta la PC
        public int ConsumptionWatts { get; set; }
    }
}
