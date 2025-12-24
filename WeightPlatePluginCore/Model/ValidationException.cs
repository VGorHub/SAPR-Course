using System;
using System.Collections.Generic;

namespace WeightPlatePluginCore.Model
{
    /// <summary>
    /// Исключение валидации параметров.
    /// Содержит список всех ошибок, найденных при проверке.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Создаёт исключение валидации на основе списка ошибок.
        /// </summary>
        /// //TODO: RSDN
        /// <param name="errors">Список ошибок валидации. Может быть пустым, но не равным null.</param>
        public ValidationException(List<ValidationError> errors)
            : base(errors != null && errors.Count > 0 ? errors[0].Message : string.Empty)
        {
            Errors = errors ?? new List<ValidationError>();
        }

        /// <summary>
        /// Создаёт исключение валидации для одной ошибки.
        /// </summary>
        /// <param name="error">Ошибка валидации (может быть null).</param>
        public ValidationException(ValidationError error)
            : this(error != null
                ? new List<ValidationError> { error }
                : new List<ValidationError>())
        {
        }

        /// <summary>
        /// Список всех ошибок валидации.
        /// </summary>
        public IReadOnlyList<ValidationError> Errors { get; }

        /// <summary>
        /// Возвращает список ошибок валидации.
        /// </summary>
        public IReadOnlyList<ValidationError> GetErrors() => Errors;

        /// <summary>
        /// true, если ошибок нет.
        /// </summary>
        public bool IsValid => Errors.Count == 0;
    }
}
