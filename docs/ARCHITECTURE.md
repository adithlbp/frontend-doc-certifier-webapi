# Arquitectura del Sistema Frontend Document Certifier

## Visión General

El servicio **Frontend Document Certifier** es una API .NET 8 que evalúa documentos de especificación frontend usando modelos LLM a través de Genius API.

## Flujo de Datos

```
┌─────────────────┐
│   Usuario       │
│  (Frontend)     │
└────────┬────────┘
         │ POST /v1/evaluations
         │ (file_general.md, file_servicio.md)
         ▼
┌─────────────────────────────────────┐
│  EvaluationsController              │
│  - Receives Markdown files          │
│  - Validates input                  │
└────────┬────────────────────────────┘
         │
         ▼
┌─────────────────────────────────────┐
│  CreateEvaluationCommandHandler     │
│  1. Extract Markdown content        │
│  2. Create Revisions                │
│  3. Save Evaluation (RUNNING)        │
│  4. Call Genius API                 │
│  5. Process JSON response           │
│  6. Update Evaluation (DONE)        │
└────────┬────────────────────────────┘
         │
         ├─────────────────┐
         ▼                 ▼
┌──────────────┐   ┌──────────────┐
│ RevisionRepo │   │ GeniusApi    │
│              │   │ Client       │
└──────────────┘   └──────┬───────┘
                          │
                          │ HTTP POST with structured prompt
                          │ { response_format: "json_object" }
                          ▼
                   ┌──────────────┐
                   │ Genius API   │
                   │ (LLM Models) │
                   └──────┬───────┘
                          │ JSON Response
                          │ (structured)
                          ▼
                   ┌──────────────┐
                   │ ParseGenius  │
                   │ Response     │
                   └──────────────┘
```

## Capas de Arquitectura (Clean Architecture)

### 1. Core Layer

#### Domain
- **Entities**: `Revision`, `Evaluation`, `Criterion`, `EvaluationCriterion`
- **Interfaces**: `IEvaluationRepository`, `IRevisionRepository`, `IGeniusApiClient`

#### Application
- **DTOs**: Mapeo de entidades a DTOs para API responses
- **Use Cases**: 
  - `CreateEvaluationCommandHandler` - Proceso de evaluación
  - `GetEvaluationQueryHandler` - Obtener evaluación por ID
  - `GetRevisionsQueryHandler` - Listar revisiones con paginación

### 2. Infrastructure Layer

#### Persistence
- **SpecDbContext**: DbContext con configuraciones Fluent API para PostgreSQL
- **Repositories**: Implementaciones de repositorios con EF Core

#### Clients
- **GeniusApiClient**: 
  - Construye prompt estructurado para LLM
  - Envía request con `response_format: "json_object"`
  - Parsea respuesta JSON estructurada

### 3. Presentation Layer
- **Controllers**: `EvaluationsController`, `RevisionsController`
- **Middleware**: Exception handling, CORS configurable

## Integración con Genius API

### Formato del Prompt

El prompt enviado a Genius API incluye:
1. Contenido de los documentos Markdown
2. Instrucciones claras de evaluación
3. Formato de salida JSON obligatorio

### Respuesta Estructurada

Genius API retorna JSON con:
```json
{
  "overallStatus": { "status": "PASS|FAIL|PARTIAL" },
  "scores": {
    "byStatus": { "pass": 0, "fail": 0, "na": 0, "total": 0 },
    "bySeverity": { "CRITICAL": {...}, "MEDIUM": {...}, "LOW": {...} },
    "critical": { "passed": 0, "failed": 0, "ids": {...} }
  },
  "gate": {
    "rule": "no_critical_fail",
    "passed": true,
    "reason": "...",
    "details": { "criticalFailedCount": 0, "failedIds": [] }
  },
  "criteria": [...]
}
```

## Regla de Gate

**FAIL si existe ≥1 criterio CRITICAL en FAIL**

La regla se aplica automáticamente en la respuesta de Genius API y se persiste en la evaluación.

## Base de Datos

### Tablas Principales

1. **revisions**: Registro de versiones de documentos
2. **evaluations**: Evaluaciones completas con resultados
3. **criteria**: Catálogo de criterios de evaluación
4. **evaluation_criteria**: Resultados por criterio en cada evaluación

### Conexión a Cloud SQL

- Usa socket Unix: `Host=/cloudsql/{INSTANCE_CONNECTION_NAME}`
- Credenciales desde Secret Manager
- PostgreSQL 8.0.11 compatible

## Configuración de CORS

Configurable por variable de entorno `CORS_ALLOWED_ORIGINS`:
- `*` = Permitir todos los orígenes (desarrollo)
- `url1,url2,url3` = Lista de orígenes permitidos (producción)

