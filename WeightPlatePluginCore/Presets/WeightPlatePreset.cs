using System;
using WeightPlatePluginCore.Model;

namespace WeightPlatePluginCore.Presets
{
    /// <summary>
    /// Пресет блина: отображаемое имя + набор параметров.
    /// Для Custom параметры отсутствуют.
    /// </summary>
    public sealed class WeightPlatePreset
    {
        /// <summary>
        /// Инициализирует новый экземпляр пресета.
        /// </summary>
        /// <param name="id">Идентификатор пресета.</param>
        /// <param name="displayName">Отображаемое имя пресета.</param>
        /// <param name="parameters">
        /// Набор параметров блина.
        /// Для пресета <see cref="WeightPlatePresetId.Custom"/> может быть null.
        /// </param>
        public WeightPlatePreset(WeightPlatePresetId id, string displayName, Parameters parameters)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("Имя пресета не задано.", nameof(displayName));
            }

            Id = id;
            DisplayName = displayName;
            Parameters = parameters;
        }

        /// <summary>
        /// Идентификатор пресета.
        /// </summary>
        public WeightPlatePresetId Id { get; }

        /// <summary>
        /// Отображаемое имя пресета.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Параметры блина, связанные с пресетом.
        /// Для пользовательского пресета может быть null.
        /// </summary>
        public Parameters Parameters { get; }


        /// <summary>
        /// Признак пользовательского пресета.
        /// </summary>
        public bool IsCustom
        {
            get { return Id == WeightPlatePresetId.Custom; }
        }

        /// <summary>
        /// Возвращает строковое представление пресета
        /// для отображения в элементах пользовательского интерфейса.
        /// </summary>
        /// <returns>Отображаемое имя пресета.</returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
