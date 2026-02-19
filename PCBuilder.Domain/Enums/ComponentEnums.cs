using System;
using System.Collections.Generic;
using System.Text;

namespace PCBuilder.Domain.Enums
{
    public enum SocketType
    {
        AM4,
        AM5,
        LGA1700,
        LGA1200
        // Podemos agregar más luego
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

    public enum EfficiencyRating
    {
        Generic,       // Fuentes sin certificación
        PlusWhite,
        PlusBronze,
        PlusSilver,
        PlusGold,
        PlusPlatinum,
        PlusTitanium
    }
    public enum PcieGeneration
    {
        Gen3,
        Gen4,
        Gen5
    }
    public enum FormFactor
    {
        EATX,
        ATX,
        MicroATX,
        MiniITX
    }

    public enum CoolerType
    {
        Air,
        Liquid
    }

    public enum PsuFormFactor
    {
        ATX,
        SFX,
        SFXL,
        TFX // Uno muy finito usado en PCs de oficina
    }

    public enum PsuModularity
    {
        NonModular,
        SemiModular,
        FullyModular
    }
}
