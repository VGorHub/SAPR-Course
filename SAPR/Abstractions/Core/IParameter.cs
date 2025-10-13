namespace WeightPlatePlugin.Abstractions.Core
{
    /// <summary>Отдельный числовой параметр с диапазоном допустимых значений.</summary>
    public interface IParameter
    {
        double MinValue { get; set; }
        double MaxValue { get; set; }
        double Value { get; set; }

        /// <summary>Проверка Value ∈ [MinValue; MaxValue] и прочих ограничений.</summary>
        IValidationResult Validate();
    }
}
