using System;
using System.Collections.Generic;
using System.Text;
using PCBuilder.Domain.Enums;
namespace PCBuilder.Domain.Entities
{
    public class Processor : Component
    {
        public SocketType Socket { get; set; }
        public RamType SupportedRam { get; set; }
        public bool IncludesStockCooler { get; set; } // Dato útil para saber si hay que obligar a comprar un cooler
    }
}
