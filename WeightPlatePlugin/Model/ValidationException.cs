using System;
using System.Collections.Generic;

namespace WeightPlatePlugin.Model
{
    /// <summary>
    /// Исключение валидации параметров
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(List<ValidationError> errors)
            : base(errors != null && errors.Count > 0 ? errors[0].Message : string.Empty)
        {
            Errors = errors ?? new List<ValidationError>();
        }

        public ValidationException(ValidationError error)
            : this(error != null
                ? new List<ValidationError> { error }
                : new List<ValidationError>())
        {
        }

        public IReadOnlyList<ValidationError> Errors { get; }

        public IReadOnlyList<ValidationError> GetErrors() => Errors;

        public bool IsValid => Errors.Count == 0;
    }
}
