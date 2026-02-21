using System;
using System.Collections.Generic;
using System.Text;

namespace PCBuilder.Domain.Enums
{
    // ==========================================
    // GRUPO 1: CATEGÓRICOS E IRREGULARES (Solución 2)
    // Se usan tal cual. Su compatibilidad se valida con 
    // lógicas específicas o matrices (ej. un Switch)
    // ==========================================

    public enum SocketType
    {
        AM4,
        AM5,
        LGA1700,
        LGA1200,
        AM3,
        LGA1150
    }

    public enum RamType
    {
        DDR3,
        DDR4,
        DDR5
    }

    public enum StorageInterfaceType
    {
        SATA,
        M2
    }

    public enum CoolerType
    {
        Air,
        Liquid
    }

    public enum PsuModularity
    {
        NonModular,
        SemiModular,
        FullyModular
    }

    // La compatibilidad física de las fuentes no es lineal (requiere adaptadores, etc.)
    public enum PsuFormFactor
    {
        ATX,
        SFX,
        SFXL,
        TFX
    }


    // ==========================================
    // GRUPO 2: LINEALES Y JERÁRQUICOS (Solución 1)
    // Tienen un "peso" numérico explícito para poder 
    // hacer validaciones matemáticas (>=, <=) súper rápidas.
    // ==========================================

    // Generaciones del bus: A mayor número, más velocidad.
    public enum PcieGeneration
    {
        Gen3 = 3,
        Gen4 = 4,
        Gen5 = 5
    }

    // Formato físico: A mayor número, placa madre más grande.
    // Ej: Un gabinete ATX (30) soporta MicroATX (20) porque 30 >= 20.
    public enum FormFactor
    {
        MiniITX = 10,
        MicroATX = 20,
        ATX = 30,
        EATX = 40
    }

    // Certificaciones: A mayor número, mejor eficiencia energética.
    // (Útil si a futuro querés obligar a que una PC muy potente use mínimo PlusGold)
    public enum EfficiencyRating
    {
        Generic = 0,
        PlusWhite = 10,
        PlusBronze = 20,
        PlusSilver = 30,
        PlusGold = 40,
        PlusPlatinum = 50,
        PlusTitanium = 60
    }
}
