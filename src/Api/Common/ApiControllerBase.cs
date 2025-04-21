using Microsoft.AspNetCore.Mvc;
using Application.Commons;

namespace Api.Common 
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        // Podemos injetar um logger genérico aqui se quisermos logar
        // eventos comuns a todas as controllers ou no próprio mapeamento.
        // Usar ILogger<ApiControllerBase> é uma opção.
        private readonly ILogger<ApiControllerBase> _baseLogger;

        protected ApiControllerBase(ILogger<ApiControllerBase> baseLogger)
        {
            _baseLogger = baseLogger;
        }

        // Movemos o método para cá e o tornamos protected
        protected IActionResult MapErrorToActionResult(Error error)
        {
             _baseLogger.LogWarning("Mapeando erro da aplicação para resultado HTTP. Código: {ErrorCode}, Descrição: {ErrorDescription}, Tipo: {ErrorType}",
                                  error.Code, error.Description, error.Type);

            switch (error.Type)
            {
                case Error.ErrorType.Validation:
                    // Retorna 400 com o objeto Error serializado no corpo
                    return BadRequest(error);
                case Error.ErrorType.NotFound:
                    // Retorna 404 com o objeto Error serializado no corpo
                    return NotFound(error);
                case Error.ErrorType.Conflict:
                    // Retorna 409 com o objeto Error serializado no corpo
                    return Conflict(error);
                case Error.ErrorType.Failure:
                case Error.ErrorType.None: // Segurança
                default:
                    // Logamos especificamente os erros que caem no 500
                    _baseLogger.LogError("Erro mapeado para 500 Internal Server Error. Código: {ErrorCode}", error.Code);
                    // Retorna 500 com o objeto Error serializado no corpo
                    return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
    }
}