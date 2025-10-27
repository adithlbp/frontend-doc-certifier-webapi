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
            
            // Construir prompt estructurado con formato de salida JSON
            var prompt = BuildEvaluationPrompt(request.DocumentGeneralMarkdown, request.DocumentServicioMarkdown);
            
            var geniusRequest = new
            {
                prompt = prompt,
                model = request.Model ?? "gemini-1.5-pro",
                temperature = 0.5,
                max_tokens = 8000,
                response_format = new { type = "json_object" } // Forzar salida JSON estructurada
            };
            
            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/v1/chat/completions", 
                geniusRequest, 
                cancellationToken);
            
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
            
            if (jsonResponse.ValueKind == JsonValueKind.Null)
            {
                throw new InvalidOperationException("Genius API returned null response");
            }
            
            var result = ParseGeniusResponse(jsonResponse);
            
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

    private static string BuildEvaluationPrompt(string generalMarkdown, string servicioMarkdown)
    {
        return $@"Evalúa los siguientes documentos de especificación frontend contra los criterios de arquitectura establecidos.

DOCUMENTO GENERAL:
{generalMarkdown}

DOCUMENTO DE SERVICIO:
{servicioMarkdown}

INSTRUCCIONES:
1. Evalúa cada criterio (CRITICAL, MEDIUM, LOW) según la información en los documentos
2. Para cada criterio aplicable, determina si cumple (PASS), no cumple (FAIL), o no aplica (N/A)
3. Proporciona evidencia específica del documento
4. Calcula los scores por severidad y por status

FORMATO DE RESPUESTA (JSON obligatorio):
{{
  ""overallStatus"": {{
    ""status"": ""PASS|FAIL|PARTIAL""
  }},
  ""scores"": {{
    ""byStatus"": {{
      ""pass"": 0,
      ""fail"": 0,
      ""na"": 0,
      ""total"": 0
    }},
    ""bySeverity"": {{
      ""CRITICAL"": {{ ""pass"": 0, ""fail"": 0, ""na"": 0, ""total"": 0 }},
      ""MEDIUM"": {{ ""pass"": 0, ""fail"": 0, ""na"": 0, ""total"": 0 }},
      ""LOW"": {{ ""pass"": 0, ""fail"": 0, ""na"": 0, ""total"": 0 }}
    }},
    ""critical"": {{
      ""passed"": 0,
      ""failed"": 0,
      ""na"": 0,
      ""ids"": {{
        ""passed"": [],
        ""failed"": [],
        ""na"": []
      }}
    }}
  }},
  ""gate"": {{
    ""rule"": ""no_critical_fail"",
    ""passed"": true,
    ""reason"": """",
    ""details"": {{
      ""criticalFailedCount"": 0,
      ""failedIds"": []
    }}
  }},
  ""criteria"": [
    {{
      ""id"": ""AF-1"",
      ""category"": ""Categoría"",
      ""title"": ""Título del criterio"",
      ""applicable"": true,
      ""status"": ""PASS|FAIL|N/A"",
      ""severity"": ""CRITICAL|MEDIUM|LOW"",
      ""evidence"": ""Evidencia del documento"",
      ""comments"": ""Comentarios adicionales""
    }}
  ]
}}";
    }

    private static GeniusEvaluationResponse ParseGeniusResponse(JsonElement jsonResponse)
    {
        try
        {
            var result = new GeniusEvaluationResponse { Success = true };
            
            // Extraer el contenido de la respuesta
            if (jsonResponse.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var choice = choices[0];
                if (choice.TryGetProperty("message", out var message) && 
                    message.TryGetProperty("content", out var content))
                {
                    var contentString = content.GetString() ?? "{}";
                    var evaluationResult = JsonSerializer.Deserialize<EvaluationResultJson>(
                        contentString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    result.Result = evaluationResult;
                }
            }
            
            // Extraer token usage
            if (jsonResponse.TryGetProperty("usage", out var usage))
            {
                result.TokenUsage = new TokenUsage
                {
                    PromptTokens = usage.TryGetProperty("prompt_tokens", out var pt) ? pt.GetInt32() : 0,
                    CompletionTokens = usage.TryGetProperty("completion_tokens", out var ct) ? ct.GetInt32() : 0,
                    TotalTokens = usage.TryGetProperty("total_tokens", out var tt) ? tt.GetInt32() : 0
                };
            }
            
            return result;
        }
        catch (Exception ex)
        {
            return new GeniusEvaluationResponse
            {
                Success = false,
                Error = $"Error al parsear respuesta: {ex.Message}"
            };
        }
    }
}

