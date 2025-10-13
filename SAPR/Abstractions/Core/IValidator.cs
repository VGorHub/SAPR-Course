namespace WeightPlatePlugin.Abstractions.Core
{
    /// <summary>Общий контракт валидатора.</summary>
    public interface IValidator<T>
    {
        IValidationResult Validate(T value);
    }
}
