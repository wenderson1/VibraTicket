using Application.Commons;
using Application.Query.Affiliate.GetAffiliateByDocument;
using Application.Query.Affiliate.GetAffiliateById;
using Application.UseCases.Affiliate.CreateAffiliate;
using Application.UseCases.Affiliate.DeleteAffiliate;
using Application.UseCases.Affiliate.UpdateAffiliate;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AffiliateController : ControllerBase
{
    private readonly ICreateAffiliateUseCase _createAffiliateUseCase;
    private readonly IUpdateAffiliateUseCase _updateAffiliateUseCase;
    private readonly IDeleteAffiliateUseCase _deleteAffiliateUseCase;
    private readonly IGetAffiliateByIdQuery _getAffiliateByIdQuery;
    private readonly IGetAffiliateByDocumentQuery _getAffiliateByDocumentQuery;

    public AffiliateController(
        ICreateAffiliateUseCase createAffiliateUseCase,
        IUpdateAffiliateUseCase updateAffiliateUseCase,
        IDeleteAffiliateUseCase deleteAffiliateUseCase,
        IGetAffiliateByIdQuery getAffiliateByIdQuery,
        IGetAffiliateByDocumentQuery getAffiliateByDocumentQuery)
    {
        _createAffiliateUseCase = createAffiliateUseCase;
        _updateAffiliateUseCase = updateAffiliateUseCase;
        _deleteAffiliateUseCase = deleteAffiliateUseCase;
        _getAffiliateByIdQuery = getAffiliateByIdQuery;
        _getAffiliateByDocumentQuery = getAffiliateByDocumentQuery;
    }

    /// <summary>
    /// Cria um novo afiliado
    /// </summary>
    /// <param name="input">Dados do afiliado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>ID do afiliado criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateAffiliateInput input, CancellationToken cancellationToken)
    {
        var result = await _createAffiliateUseCase.ExecuteAsync(input, cancellationToken);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result);
        
        return result.Error.Type switch
        {
            Error.ErrorType.Validation => BadRequest(result),
            _ => StatusCode(500, result)
        };
    }

    /// <summary>
    /// Atualiza um afiliado existente
    /// </summary>
    /// <param name="id">ID do afiliado</param>
    /// <param name="input">Dados para atualização</param>
    /// <returns>Resultado da operação</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAffiliateInput input)
    {
        var result = await _updateAffiliateUseCase.Execute(id, input);
        if (result.IsSuccess)
            return Ok(result);

        return result.Error.Type switch
        {
            Error.ErrorType.Validation => BadRequest(result),
            Error.ErrorType.NotFound => NotFound(result),
            _ => StatusCode(500, result)
        };
    }

    /// <summary>
    /// Remove logicamente um afiliado
    /// </summary>
    /// <param name="id">ID do afiliado</param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteAffiliateUseCase.Execute(id);
        if (result.IsSuccess)
            return Ok(result);

        return result.Error.Type switch
        {
            Error.ErrorType.NotFound => NotFound(result),
            _ => StatusCode(500, result)
        };
    }

    /// <summary>
    /// Obtém um afiliado pelo ID
    /// </summary>
    /// <param name="id">ID do afiliado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do afiliado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<GetAffiliateByIdOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<GetAffiliateByIdOutput>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<GetAffiliateByIdOutput>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _getAffiliateByIdQuery.ExecuteAsync(id, cancellationToken);
        if (result.IsSuccess)
            return Ok(result);

        return result.Error.Type switch
        {
            Error.ErrorType.NotFound => NotFound(result),
            _ => StatusCode(500, result)
        };
    }

    /// <summary>
    /// Obtém um afiliado pelo número do documento
    /// </summary>
    /// <param name="document">Número do documento do afiliado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do afiliado</returns>
    [HttpGet("by-document/{document}")]
    [ProducesResponseType(typeof(Result<GetAffiliateByDocumentOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<GetAffiliateByDocumentOutput>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<GetAffiliateByDocumentOutput>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByDocument(string document, CancellationToken cancellationToken)
    {
        var result = await _getAffiliateByDocumentQuery.ExecuteAsync(document, cancellationToken);
        if (result.IsSuccess)
            return Ok(result);

        return result.Error.Type switch
        {
            Error.ErrorType.NotFound => NotFound(result),
            _ => StatusCode(500, result)
        };
    }
}