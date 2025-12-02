namespace WeightPlatePlugin.Model
{
    /// <summary>
    /// Список параметров геометрии диска.
    /// </summary>
    public enum ParameterId
    {
        //TODO: XML +

        /// <summary>
        /// Радиус фаски/скругления кромок (R).
        /// </summary>
        ChamferRadiusR,

        /// <summary>
        /// Диаметр центрального отверстия (d).
        /// </summary>
        HoleDiameterd,

        /// <summary>
        /// Наружный диаметр диска (D).
        /// </summary>
        OuterDiameterD,

        /// <summary>
        /// Глубина внутреннего углубления (G).
        /// </summary>
        RecessDepthG,

        /// <summary>
        /// Радиус внутреннего углубления (L).
        /// </summary>
        RecessRadiusL,

        /// <summary>
        /// Толщина диска (T).
        /// </summary>
        ThicknessT
    }
}
