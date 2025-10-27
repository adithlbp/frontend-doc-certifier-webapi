using System.Net.Http.Json;
using System.Text.Json;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;

namespace Com.Coppel.Web.Api.Infrastructure.Clients;

/// <summary>
/// Cliente HTTP para Genius API
/// </summary>
public class GeniusApiClient : IGeniusApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeniusApiClient> _logger;
    private readonly string _baseUrl;
    private readonly string? _authToken;

    public GeniusApiClient(
        HttpClient httpClient,
        ILogger<GeniusApiClient> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["GENIUS_BASE_URL"] ?? throw new InvalidOperationException("GENIUS_BASE_URL is required");
        _authToken = configuration["GENIUS_AUTH"];
        
        if (!string.IsNullOrEmpty(_authToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
        }
    }

    public async Task<GeniusEvaluationResponse> EvaluateDocumentsAsync(
        GeniusEvaluationRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Enviando evaluación a Genius API");
            
            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/evaluate", 
                request, 
                cancellationToken);
            
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<GeniusEvaluationResponse>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken);
            
            if (result == null)
            {
                throw new InvalidOperationException("Genius API returned null response");
            }
            
            _logger.LogInformation("Evaluación recibida exitosamente de Genius API");
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al comunicarse con Genius API");
            return new GeniusEvaluationResponse
            {
                Success = false,
                Error = $"Error de comunicación: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al evaluar documentos");
            return new GeniusEvaluationResponse
            {
                Success = false,
                Error = $"Error inesperado: {ex.Message}"
            };
        }
    }
}

