namespace WeightPlatePlugin.Model
{
    /// <summary>
    /// Описание одной ошибки валидации конкретного параметра
    /// </summary>
    public class ValidationError
    {
        public ValidationError(ParameterId parameter, string message)
        {
            Parameter = parameter;
            Message = message;
        }

        public ParameterId Parameter { get; }

        public string Message { get; }

        public string GetMessage() => Message;

        public ParameterId GetParameter() => Parameter;
    }
}
