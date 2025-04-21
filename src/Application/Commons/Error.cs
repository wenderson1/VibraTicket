namespace Application.Commons
{
    public class Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
        public static readonly Error NullValue = new("Error.NullValue", "Um valor nulo foi fornecido inesperadamente.", ErrorType.Failure);

        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public enum ErrorType
        {
            None = 0,
            Validation = 1, // Mapeia para 400 Bad Request
            NotFound = 2,   // Mapeia para 404 Not Found
            Conflict = 3,   // Mapeia para 409 Conflict
            Failure = 4     // Mapeia para 500 Internal Server Error (ou outros erros genéricos)
        }

        public static Error Custom(ErrorType type, string code, string description) => new(code, description, type);
        public static Error Validation(string code, string description) => new(code, description, ErrorType.Validation);
        public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
        public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
        public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);
    }
}