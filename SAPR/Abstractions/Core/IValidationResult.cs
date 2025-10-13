using System.Collections.Generic;

namespace WeightPlatePlugin.Abstractions.Core
{
    /// <summary>Результат валидации параметров/модели.</summary>
    public interface IValidationResult
    {
        bool IsValid { get; }
        string Message { get; }
        IReadOnlyDictionary<ParamType, string> FieldErrors { get; }
    }
}
