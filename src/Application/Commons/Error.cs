using FluentValidation.Results;

namespace Application.Commons
{
    public class Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
        public static readonly Error NullValue = new("Error.NullValue", "Um valor nulo foi fornecido inesperadamente.", ErrorType.Failure);
        public static readonly Error InternalError = new("Error.Internal", "Um erro inesperado ocorreu.", ErrorType.Failure);

        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }
        public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; init; }

        private Error(string code, string description, ErrorType type, IReadOnlyDictionary<string, string[]>? validationErrors = null)
        {
            Code = code;
            Description = description;
            Type = type;
            ValidationErrors = validationErrors;
        }

        public enum ErrorType
        {
            None = 0,
            Validation = 1, // Mapeia para 400 Bad Request
            NotFound = 2,   // Mapeia para 404 Not Found
            Conflict = 3,   // Mapeia para 409 Conflict
            Failure = 4     // Mapeia para 500 Internal Server Error (ou outros erros genéricos)
        }

        public static Error Validation(string description, string code = "Validation.Error") =>
            new(code, description, ErrorType.Validation);

        public static Error NotFound(string description, string code = "NotFound.Error") =>
            new(code, description, ErrorType.NotFound);

        public static Error Conflict(string description, string code = "Conflict.Error") =>
            new(code, description, ErrorType.Conflict);

        public static Error Failure(string description, string code = "Failure.Error") =>
            new(code, description, ErrorType.Failure);
        public static Error Custom(ErrorType type, string code, string description) =>
            new(code, description, type);

        public static Error FromValidationResult(ValidationResult validationResult)
        {
            if (validationResult.IsValid)
            {
                return Failure("Erro inesperado ao processar resultado de validação válido.");
            }
            var errorsDictionary = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => string.IsNullOrEmpty(g.Key) ? "GeneralErrors" : g.Key, // Chave para erros não associados a uma propriedade
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            string mainDescription = validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Um ou mais erros de validação ocorreram.";
            string mainErrorCode = "Input.Validation";

            return new Error(mainErrorCode, mainDescription, ErrorType.Validation, errorsDictionary);
        }
    }
}