namespace WeightPlatePlugin.Abstractions.Core
{
    /// <summary>Коллекция параметров блина (D, T, d, R, L, G).</summary>
    public interface IWeightPlateParameters
    {
        /// <summary>Чтение/запись значения по типу параметра.</summary>
        double this[ParamType type] { get; set; }

        /// <summary>Получить объект параметра (для доступа к мин/макс и валидации).</summary>
        IParameter GetParameter(ParamType type);

        /// <summary>Установить значение с проверкой.</summary>
        void SetValue(ParamType type, double value);

        /// <summary>Глобальная проверка взаимных ограничений (например, T ≤ D/10, d &lt; D, d &lt; L &lt; D, G &lt; T).</summary>
        IValidationResult ValidateAll();
    }
}
