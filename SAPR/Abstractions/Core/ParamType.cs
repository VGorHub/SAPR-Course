namespace WeightPlatePlugin.Abstractions.Core
{
    /// <summary>Типы параметров блина для штанги.</summary>
    public enum ParamType
    {
        /// <summary>Наружный диаметр D.</summary>
        OuterDiameter,
        /// <summary>Толщина T.</summary>
        Thickness,
        /// <summary>Диаметр отверстия d.</summary>
        HoleDiameter,
        /// <summary>Радиус скругления R.</summary>
        FilletRadius,
        /// <summary>Диаметр внутреннего углубления L.</summary>
        RecessDiameter,
        /// <summary>Глубина внутреннего углубления G.</summary>
        RecessDepth
    }
}

