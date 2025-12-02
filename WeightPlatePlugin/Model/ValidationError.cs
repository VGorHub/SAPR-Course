namespace WeightPlatePlugin.Model
{
    /// <summary>
    /// Описание одной ошибки валидации конкретного параметра
    /// </summary>
    public class ValidationError
    {
        //TODO: XML
        public ValidationError(ParameterId parameter, string message)
        {
            Parameter = parameter;
            Message = message;
        }

        public ParameterId Parameter { get; }

        public string Message { get; }

        //TODO: refactor
        public string GetMessage() => Message;

        //TODO: refactor
        public ParameterId GetParameter() => Parameter;
    }
}
