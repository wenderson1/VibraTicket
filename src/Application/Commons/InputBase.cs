using FluentValidation;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Application.Commons
{
    public abstract record InputBase<TInput> where TInput : InputBase<TInput>
    {
        // Propriedade para armazenar o resultado da validação, se desejado
        [JsonIgnore] // Se você não quiser que isso seja serializado se o input for retornado
        public ValidationResult? ValidationResult { get; protected set; }

        /// <summary>
        /// Valida a instância atual do input usando o validador fornecido.
        /// </summary>
        /// <param name="validator">O validador específico para o tipo TInput.</param>
        /// <returns>O resultado da validação do FluentValidation.</returns>
        public virtual ValidationResult Validate(IValidator<TInput> validator)
        {
            // O cast para (TInput)this é crucial aqui.
            // Como TInput é restrito a ser BaseInput<TInput> (ou uma classe derivada),
            // 'this' (que é a instância da classe derivada) pode ser seguramente
            // tratado como TInput para o validador.
            ValidationResult result = validator.Validate((TInput)this);
            // this.ValidationResult = result; // Opcional: armazenar o resultado
            return result;
        }

        /// <summary>
        /// Indica se o input é válido após a execução do método Validate.
        /// Este método é uma conveniência e depende que Validate() tenha sido chamado
        /// com um validador que armazene o resultado ou que o resultado seja passado.
        /// Para uma abordagem mais robusta, sempre use o ValidationResult retornado por Validate().
        /// </summary>
        public bool IsValid => ValidationResult?.IsValid ?? false;

        /// <summary>
        /// Obtém os erros de validação como um dicionário.
        /// Chave: Nome da Propriedade, Valor: Array de mensagens de erro.
        /// </summary>
        public IReadOnlyDictionary<string, string[]> GetErrors()
        {
            if (ValidationResult == null || ValidationResult.IsValid)
            {
                return new Dictionary<string, string[]>();
            }
            return ValidationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }
    }
}