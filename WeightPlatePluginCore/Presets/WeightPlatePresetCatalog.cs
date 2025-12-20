using System;
using System.Collections.Generic;
using WeightPlatePluginCore.Model;

namespace WeightPlatePluginCore.Presets
{
    /// <summary>
    /// Каталог пресетов блинов.
    /// </summary>
    public static class WeightPlatePresetCatalog
    {
        /// <summary>
        /// Набор предопределённых пресетов блинов.
        /// Содержит стандартные варианты параметров для типовых блинов,
        /// а также пользовательский пресет <see cref="WeightPlatePresetId.Custom"/>.
        /// </summary>
        /// <remarks>
        /// Каждый предопределённый пресет создаётся с полностью валидным
        /// набором параметров <see cref="Parameters"/>.
        ///
        /// Пресет <see cref="WeightPlatePresetId.Custom"/> не содержит параметров
        /// и используется как маркер ручного ввода пользователем.
        /// </remarks>
        private static readonly WeightPlatePreset[] Presets =
        {
            Create(
                WeightPlatePresetId.StandardTraining,
                "Стандартный (Ø450, отв. Ø28)",
                outerDiameterD: 450,
                thicknessT: 45,
                holeDiameterd: 28,
                chamferRadiusR: 5,
                recessRadiusL: 120,
                recessDepthG: 15),

            Create(
                WeightPlatePresetId.Olympic50,
                "Олимпийский (Ø450, отв. Ø50)",
                outerDiameterD: 450,
                thicknessT: 45,
                holeDiameterd: 50,
                chamferRadiusR: 3,
                recessRadiusL: 150,
                recessDepthG: 20),

            Create(
                WeightPlatePresetId.Compact300,
                "Компактный (Ø300)",
                outerDiameterD: 300,
                thicknessT: 30,
                holeDiameterd: 28,
                chamferRadiusR: 3,
                recessRadiusL: 90,
                recessDepthG: 10),

            Create(
                WeightPlatePresetId.Dumbbell200,
                "Гантельный/малый (Ø200)",
                outerDiameterD: 200,
                thicknessT: 20,
                holeDiameterd: 26,
                chamferRadiusR: 2,
                recessRadiusL: 60,
                recessDepthG: 8),

            new WeightPlatePreset(
                WeightPlatePresetId.Custom,
                "Пользовательский",
                parameters: null)
        };

        /// <summary>
        /// Идентификатор пресета по умолчанию при старте.
        /// </summary>
        public static WeightPlatePresetId DefaultPresetId
        {
            get { return WeightPlatePresetId.StandardTraining; }
        }

        /// <summary>
        /// Возвращает все пресеты.
        /// </summary>
        public static IReadOnlyList<WeightPlatePreset> GetAll()
        {
            return Presets;
        }

        /// <summary>
        /// Возвращает пресет по id.
        /// </summary>
        public static WeightPlatePreset GetById(WeightPlatePresetId id)
        {
            for (int i = 0; i < Presets.Length; i++)
            {
                if (Presets[i].Id == id)
                {
                    return Presets[i];
                }
            }

            throw new ArgumentOutOfRangeException(
                nameof(id), 
                id, 
                "Неизвестный пресет.");
        }

        /// <summary>
        /// Создает экземпляр WeightPlatePreset с валидным Parameters.
        /// </summary>
        private static WeightPlatePreset Create(
            WeightPlatePresetId id,
            string name,
            double outerDiameterD,
            double thicknessT,
            double holeDiameterd,
            double chamferRadiusR,
            double recessRadiusL,
            double recessDepthG)
        {
            var p = new Parameters();
            p.SetOuterDiameterD(outerDiameterD);
            p.SetThicknessT(thicknessT);
            p.SetHoleDiameterd(holeDiameterd);
            p.SetChamferRadiusR(chamferRadiusR);
            p.SetRecessRadiusL(recessRadiusL);
            p.SetRecessDepthG(recessDepthG);

            // Пресеты обязаны быть валидными.
            p.ValidateAll();

            return new WeightPlatePreset(id, name, p);
        }
    }
}
