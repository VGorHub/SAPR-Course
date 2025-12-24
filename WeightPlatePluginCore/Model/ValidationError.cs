namespace WeightPlatePluginCore.Model
{
    /// <summary>
    /// Описание одной ошибки валидации конкретного параметра.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Создаёт новый объект ошибки валидации параметра.
        /// </summary>
        /// //TODO: RSDN
        /// <param name="parameter">Идентификатор параметра, к которому относится ошибка.</param>
        /// <param name="message">Текст сообщения об ошибке.</param>
        public ValidationError(ParameterId parameter, string message)
        {
            Parameter = parameter;
            Message = message;
        }

        /// <summary>
        /// Параметр, для которого произошла ошибка.
        /// </summary>
        public ParameterId Parameter { get; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Возвращает текст сообщения об ошибке.
        /// Оставлено для совместимости с существующим кодом.
        /// </summary>
        public string GetMessage() => Message;

        /// <summary>
        /// Возвращает идентификатор параметра, для которого произошла ошибка.
        /// Оставлено для совместимости с существующим кодом.
        /// </summary>
        public ParameterId GetParameter() => Parameter;
    }
}
