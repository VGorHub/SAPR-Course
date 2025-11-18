using System;
using System.Collections.Generic;

namespace WeightPlatePlugin.Model
{
    /// <summary>
    /// Исключение валидации параметров, соответствующее классу ParameterValidationException на UML.
    /// Хранит список ошибок, каждая из которых ссылается на конкретный параметр.
    /// </summary>
    public class ParameterValidationException : Exception
    {
        public ParameterValidationException(List<ValidationError> errors)
            : base(errors != null && errors.Count > 0 ? errors[0].Message : string.Empty)
        {
            Errors = errors ?? new List<ValidationError>();
        }

        public ParameterValidationException(ValidationError error)
            : this(new List<ValidationError> { error })
        {
        }

        public IReadOnlyList<ValidationError> Errors { get; }

        public IReadOnlyList<ValidationError> GetErrors()
        {
            return Errors;
        }
    }
}

