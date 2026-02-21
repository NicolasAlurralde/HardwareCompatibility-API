using FluentValidation;
using PCBuilder.Domain.Entities;
using PCBuilder.Domain.Enums; // <-- Importante para que reconozca los Enums correctamente

namespace PCBuilder.API.Validators
{
    public class PCBuildValidator : AbstractValidator<PCBuild>
    {
        public PCBuildValidator()
        {
            RuleFor(x => x.BuildName)
                .NotEmpty().WithMessage("El ensamble debe tener un nombre para identificarlo.")
                .MaximumLength(100);

            // 1. EL CEREBRO: Procesador <-> Motherboard (Regla del Socket)
            RuleFor(x => x)
                .Must(x => x.Processor.Socket == x.Motherboard.Socket)
                .When(x => x.Processor != null && x.Motherboard != null)
                .WithMessage(x => $"Incompatibilidad Fatal: El procesador {x.Processor.Model} es socket {x.Processor.Socket}, pero la placa madre {x.Motherboard.Model} es {x.Motherboard.Socket}.");

            // 2. LA MEMORIA: Motherboard <-> RAM (Regla DDR)
            RuleFor(x => x)
                .Must(x => x.Motherboard.SupportedRam == x.Ram.Type)
                .When(x => x.Motherboard != null && x.Ram != null)
                .WithMessage(x => $"Incompatibilidad: La placa madre soporta {x.Motherboard.SupportedRam}, pero intentas conectarle memoria {x.Ram.Type}.");

            // 3. LA ENERGÍA: Fuente de Poder <-> Consumo del Sistema
            RuleFor(x => x)
                 .Must(x =>
                 {
                     // Sumamos las dos GPUs (si existen, sumamos sus watts, sino 0)
                     var gpuWatts = (x.VideoCard?.ConsumptionWatts ?? 0) + (x.SecondaryVideoCard?.ConsumptionWatts ?? 0);
                     var totalWattsNeeded = x.Processor!.ConsumptionWatts + gpuWatts + 100; // 100W de margen (Mother, RAM, etc)
                     return x.PowerSupply!.Wattage >= totalWattsNeeded;
                 })
                 .When(x => x.Processor != null && x.PowerSupply != null)
                 .WithMessage("La fuente de poder no tiene suficiente capacidad para este ensamble.");

            // 4. EL ESPACIO FÍSICO: Gabinete <-> Tarjeta de Video (Largo)
            RuleFor(x => x)
                // Pasa la validación si el gabinete no tiene el dato (0) O si el dato es mayor al de la placa
                .Must(x => x.PcCase.MaxGpuLengthMm == 0 || x.VideoCard!.LengthMm <= x.PcCase.MaxGpuLengthMm)
                .When(x => x.VideoCard != null && x.PcCase != null)
                .WithMessage(x => $"Espacio Insuficiente: La tarjeta de video mide {x.VideoCard!.LengthMm}mm, pero el gabinete solo soporta hasta {x.PcCase.MaxGpuLengthMm}mm.");

            // 4.1 AVISO POR FALTA DE DATOS (GPU)
            RuleFor(x => x)
                .Must(x => x.PcCase.MaxGpuLengthMm > 0)
                .When(x => x.VideoCard != null && x.PcCase != null)
                .WithSeverity(Severity.Warning)
                .WithMessage(x => $"Aviso de medidas: No tenemos el dato de cuánto espacio para placa de video tiene el gabinete {x.PcCase.Model}. Verificá en la web del fabricante si tu placa de {x.VideoCard!.LengthMm}mm entra correctamente."); ;

            // 5. TAMAÑO DE LA PLACA: Motherboard <-> Gabinete (SOLUCIÓN 1: Comparación Numérica Lineal)
            RuleFor(x => x)
                // LÓGICA: El soporte del Gabinete (Ej: ATX = 30) debe ser MAYOR O IGUAL al tamaño de la Placa (Ej: MicroATX = 20)
                .Must(x => (int)x.PcCase.MaxMotherboardSize >= (int)x.Motherboard.FormFactor)
                .When(x => x.Motherboard != null && x.PcCase != null)
                .WithMessage(x => $"Incompatibilidad de Tamaño: El gabinete soporta placas hasta {x.PcCase.MaxMotherboardSize}, pero intentas instalar una {x.Motherboard.FormFactor} que es más grande.");

            // 6. FORMATO DE LA FUENTE: PowerSupply <-> Gabinete (SOLUCIÓN 2: Matriz de Compatibilidad explícita)
            RuleFor(x => x)
                .Must(x => IsPsuCompatible(x.PowerSupply.FormFactor, x.PcCase.SupportedPsuFormFactor)) // Usamos el método ayudante de abajo
                .When(x => x.PowerSupply != null && x.PcCase != null)
                .WithMessage(x => $"Incompatibilidad de Fuente: El gabinete requiere formato {x.PcCase.SupportedPsuFormFactor}, pero seleccionaste una fuente {x.PowerSupply.FormFactor}.");

            // 7. LARGO DE LA FUENTE: PowerSupply <-> Gabinete (Espacio físico)
            RuleFor(x => x)
                .Must(x => x.PowerSupply.LengthMm <= x.PcCase.MaxPsuLengthMm)
                .When(x => x.PowerSupply != null && x.PcCase != null)
                .WithMessage(x => $"Falta de Espacio: La fuente mide {x.PowerSupply.LengthMm}mm, pero el gabinete solo tiene espacio para fuentes de hasta {x.PcCase.MaxPsuLengthMm}mm.");

            // 8. CUELLO DE BOTELLA: Motherboard <-> VideoCard (SOLUCIÓN 1: Comparación Numérica Lineal)
            RuleFor(x => x)
                .Must(x => (int)x.Motherboard.PcieGeneration >= (int)x.VideoCard!.PcieGeneration)
                .When(x => x.Motherboard != null && x.VideoCard != null)
                .WithSeverity(Severity.Warning)
                .WithMessage(x => $"Aviso de Rendimiento: La placa de video es {x.VideoCard!.PcieGeneration}, pero la placa madre es {x.Motherboard.PcieGeneration}. Funcionará, pero podrías perder algo de rendimiento.");
            // 9. REFRIGERACIÓN: Verificación de Cooler de Stock vs Custom Cooler
            RuleFor(x => x)
                // Pasa la validación SI el procesador trae cooler O SI el usuario compró un cooler aparte
                .Must(x => x.Processor!.IncludesStockCooler || x.Cooler != null)
                .When(x => x.Processor != null)
                .WithSeverity(Severity.Warning) // Podrías cambiarlo a Error si querés ser estricto
                .WithMessage(x => $"Atención Térmica: El procesador {x.Processor!.Model} no incluye disipador de fábrica. Te recomendamos fuertemente agregar un Cooler al ensamble.");

            // 10. SALIDA DE VIDEO (CRÍTICO): Sin gráficos integrados + Sin GPU dedicada
            RuleFor(x => x)
                // Pasa la validación SI el procesador tiene gráficos integrados O SI el usuario compró una placa de video
                .Must(x => x.Processor!.HasIntegratedGraphics || x.VideoCard != null)
                .When(x => x.Processor != null)
                .WithMessage(x => $"Falla de Video: El procesador {x.Processor!.Model} no tiene gráficos integrados. Debes agregar una Tarjeta de Video obligatoriamente para que la PC dé imagen.");

            // 11. AVISO DE REDUNDANCIA: Gráficos integrados + Tarjeta Dedicada
            RuleFor(x => x)
                // Esta regla solo se evalúa si hay GPU dedicada. Pasa la validación SI el procesador NO tiene gráficos integrados.
                .Must(x => !x.Processor!.HasIntegratedGraphics)
                .When(x => x.Processor != null && x.VideoCard != null)
                .WithSeverity(Severity.Warning)
                .WithMessage(x => $"Consejo de Ensamble: Estás combinando un procesador con gráficos integrados ({x.Processor!.Model}) y una placa de video dedicada. Podrías ahorrar dinero o ganar rendimiento eligiendo la versión sin gráficos integrados de este procesador.");
            // 12. LÍMITES DE ALMACENAMIENTO: Puertos M.2 disponibles (Suma principal + secundario)
            RuleFor(x => x)
                .Must(x =>
                {
                    int m2Count = 0;
                    if (x.Storage != null && x.Storage.InterfaceType == StorageInterfaceType.M2) m2Count += x.StorageQuantity;
                    if (x.SecondaryStorage != null && x.SecondaryStorage.InterfaceType == StorageInterfaceType.M2) m2Count += x.SecondaryStorageQuantity;

                    return x.Motherboard.M2Slots >= m2Count;
                })
                .When(x => x.Motherboard != null)
                .WithMessage(x =>
                {
                    // Volvemos a calcular para armar el mensaje exacto
                    int m2Count = 0;
                    if (x.Storage != null && x.Storage.InterfaceType == StorageInterfaceType.M2) m2Count += x.StorageQuantity;
                    if (x.SecondaryStorage != null && x.SecondaryStorage.InterfaceType == StorageInterfaceType.M2) m2Count += x.SecondaryStorageQuantity;

                    return $"Falta de puertos M.2: Intentas instalar {m2Count} disco(s) M.2, pero la placa madre {x.Motherboard.Model} solo tiene {x.Motherboard.M2Slots} ranura(s) M.2 disponibles.";
                });

            // 13. LÍMITES DE ALMACENAMIENTO: Puertos SATA disponibles (Suma principal + secundario)
            RuleFor(x => x)
                .Must(x =>
                {
                    int sataCount = 0;
                    if (x.Storage != null && x.Storage.InterfaceType == StorageInterfaceType.SATA) sataCount += x.StorageQuantity;
                    if (x.SecondaryStorage != null && x.SecondaryStorage.InterfaceType == StorageInterfaceType.SATA) sataCount += x.SecondaryStorageQuantity;

                    return x.Motherboard.SataSlots >= sataCount;
                })
                .When(x => x.Motherboard != null)
                .WithMessage(x =>
                {
                    int sataCount = 0;
                    if (x.Storage != null && x.Storage.InterfaceType == StorageInterfaceType.SATA) sataCount += x.StorageQuantity;
                    if (x.SecondaryStorage != null && x.SecondaryStorage.InterfaceType == StorageInterfaceType.SATA) sataCount += x.SecondaryStorageQuantity;

                    return $"Falta de puertos SATA: Intentas instalar {sataCount} disco(s) SATA, pero la placa madre {x.Motherboard.Model} solo tiene {x.Motherboard.SataSlots} puerto(s) SATA disponibles.";
                });



            // 14. LÍMITES DE MEMORIA: Cantidad de Slots físicos
            RuleFor(x => x)
                .Must(x => x.Motherboard.RamSlots >= x.RamQuantity)
                .When(x => x.Motherboard != null && x.Ram != null)
                .WithMessage(x => $"Falta de ranuras RAM: Intentas instalar {x.RamQuantity} módulo(s) de memoria, pero la placa madre {x.Motherboard.Model} solo tiene {x.Motherboard.RamSlots} ranura(s) disponibles.");

            // 15. LÍMITES DE MEMORIA: Capacidad Máxima en GB
            RuleFor(x => x)
                .Must(x => (x.Ram.CapacityGb * x.RamQuantity) <= x.Motherboard.MaxRamCapacityGb)
                .When(x => x.Motherboard != null && x.Ram != null)
                .WithMessage(x => $"Exceso de Memoria: El ensamble suma un total de {x.Ram.CapacityGb * x.RamQuantity}GB de RAM, pero la placa madre {x.Motherboard.Model} solo soporta hasta {x.Motherboard.MaxRamCapacityGb}GB.");

            // ==========================================
            // REFRIGERACIÓN FÍSICA Y ESPACIAL
            // ==========================================

            // 16. EL ESPACIO DEL COOLER: Gabinete <-> Disipador por Aire (Altura)
            RuleFor(x => x)
                .Must(x => x.PcCase.MaxCpuCoolerHeightMm == 0 || x.Cooler!.HeightMm <= x.PcCase.MaxCpuCoolerHeightMm)
                // Solo validamos la altura si el cooler es por aire
                .When(x => x.Cooler != null && x.PcCase != null && x.Cooler.Type == CoolerType.Air)
                .WithMessage(x => $"El disipador choca con la tapa: Mide {x.Cooler!.HeightMm}mm de alto, pero el gabinete solo admite hasta {x.PcCase.MaxCpuCoolerHeightMm}mm.");

            // 16.1 AVISO POR FALTA DE DATOS (COOLER POR AIRE)
            RuleFor(x => x)
                .Must(x => x.PcCase.MaxCpuCoolerHeightMm > 0)
                .When(x => x.Cooler != null && x.PcCase != null && x.Cooler.Type == CoolerType.Air)
                .WithSeverity(Severity.Warning)
                .WithMessage(x => $"Aviso de medidas: No tenemos el dato de altura máxima para coolers del gabinete {x.PcCase.Model}. Verificá si tu disipador de {x.Cooler!.HeightMm}mm permite cerrar la tapa.");

            // 17. REFRIGERACIÓN LÍQUIDA: Soporte general del Gabinete
            RuleFor(x => x)
                .Must(x => x.PcCase.SupportsLiquidCooling)
                .When(x => x.Cooler != null && x.PcCase != null && x.Cooler.Type == CoolerType.Liquid)
                .WithMessage(x => $"Incompatibilidad: El gabinete {x.PcCase.Model} no está diseñado para soportar sistemas de refrigeración líquida.");

            // 18. REFRIGERACIÓN LÍQUIDA: Tamaño del Radiador vs Gabinete
            RuleFor(x => x)
                // Pasa si no hay dato (0) o si el radiador es menor/igual al máximo soportado
                .Must(x => x.PcCase.MaxRadiatorSizeMm == 0 || x.Cooler!.RadiatorSizeMm <= x.PcCase.MaxRadiatorSizeMm)
                .When(x => x.Cooler != null && x.PcCase != null && x.Cooler.Type == CoolerType.Liquid && x.PcCase.SupportsLiquidCooling)
                .WithMessage(x => $"Falta de espacio para el radiador: El radiador es de {x.Cooler!.RadiatorSizeMm}mm, pero el gabinete solo soporta radiadores de hasta {x.PcCase.MaxRadiatorSizeMm}mm.");

            // 18.1 AVISO POR FALTA DE DATOS (RADIADOR)
            RuleFor(x => x)
                .Must(x => x.PcCase.MaxRadiatorSizeMm > 0)
                .When(x => x.Cooler != null && x.PcCase != null && x.Cooler.Type == CoolerType.Liquid && x.PcCase.SupportsLiquidCooling)
                .WithSeverity(Severity.Warning)
                .WithMessage(x => $"Aviso de medidas: No tenemos el dato del tamaño máximo de radiador para el gabinete {x.PcCase.Model}. Verificá si tu radiador de {x.Cooler!.RadiatorSizeMm}mm entra en la parte superior o frontal.");
            // 19. LÍMITE MULTI-GPU: Cantidad de puertos PCIe x16
            RuleFor(x => x)
                .Must(x => x.Motherboard.PcieX16Slots >= 2)
                .When(x => x.Motherboard != null && x.VideoCard != null && x.SecondaryVideoCard != null)
                .WithMessage(x => $"Falta de ranuras PCIe: Estás intentando conectar 2 Tarjetas de Video, pero la placa madre {x.Motherboard.Model} solo tiene {x.Motherboard.PcieX16Slots} ranura(s) PCIe x16.");
        }



        // =========================================================================
        // MÉTODOS AYUDANTES (Business Logic)
        // =========================================================================

        private bool IsPsuCompatible(PsuFormFactor psu, PsuFormFactor pcCaseSupportedPsu)
        {
            // Si el formato es exactamente el mismo, es compatible directo
            if (psu == pcCaseSupportedPsu) return true;

            // Matriz de excepciones del mundo real:
            return pcCaseSupportedPsu switch
            {
                // Un gabinete ATX suele aceptar fuentes chicas (SFX/SFXL) usando un adaptador de chapa
                PsuFormFactor.ATX => psu == PsuFormFactor.SFX || psu == PsuFormFactor.SFXL,
                // Un gabinete SFX jamás aceptaría una fuente ATX gigante
                PsuFormFactor.SFX => false,
                // Cualquier otro caso raro, lo denegamos por seguridad física
                _ => false
            };
        }
    }
}