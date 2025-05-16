using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Configurations
{
    public class SimpleInternalServerErrorExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<SimpleInternalServerErrorExceptionHandler> _logger;

        public SimpleInternalServerErrorExceptionHandler(ILogger<SimpleInternalServerErrorExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Loga a mensagem e o stack trace da exceção
            _logger.LogError(
                exception,
                "Ocorreu uma exceção não tratada. Mensagem: {ErrorMessage}. StackTrace: {StackTrace}. TraceIdentifier: {TraceIdentifier}",
                exception.Message,
                exception.StackTrace,
                httpContext.TraceIdentifier);

            // Define o status code para 500 Internal Server Error
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Define o tipo de conteúdo da resposta
            httpContext.Response.ContentType = "application/json";

            // Cria a mensagem de erro genérica
            var errorResponse = new
            {
                message = $"Ocorreu um erro interno. Informe esse ID ao suporte: {httpContext.TraceIdentifier}"
                // Você pode adicionar um ID de erro para rastreamento, se desejar
                // errorId = httpContext.TraceIdentifier
            };

            // Escreve a resposta JSON
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse), cancellationToken);

            // Retorna true para indicar que a exceção foi tratada
            // e nenhum outro handler de exceção deve ser invocado.
            return true;
        }
    }
}