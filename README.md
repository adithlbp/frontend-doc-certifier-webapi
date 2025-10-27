# Frontend Document Certifier API

Backend API para certificación de documentos de especificaciones frontend usando GENIUS API y Cloud SQL.

## 🏗️ Arquitectura

Este proyecto implementa **Clean Architecture** con .NET 8:

```
Com.Coppel.Web.Api/
├── Core/
│   ├── Domain/              # Entidades, Interfaces de repositorios
│   └── Application/          # DTOs, Handlers CQRS
├── Infrastructure/           # DbContext, Repositorios, Clientes HTTP
└── Presentation/            # Controllers, Middleware
```

## 🚀 Tecnologías

- **.NET 8** - Framework principal
- **Entity Framework Core 8** - ORM para PostgreSQL
- **Npgsql** - PostgreSQL provider
- **Swagger/OpenAPI** - Documentación de API
- **Cloud SQL** - Base de datos en PostgreSQL
- **GENIUS API** - Integración con modelos LLM

## 📋 Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL (local o Cloud SQL)
- Visual Studio 2022 o VS Code

## ⚡ Inicio Rápido

### 1. Configurar variables de entorno

Editar `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "SpecDb": "Host=localhost;Database=specdb;Username=specuser;Password=changeit;Port=5432"
  },
  "GENIUS_BASE_URL": "http://localhost:8000",
  "GENIUS_AUTH": "test-token"
}
```

### 2. Ejecutar migraciones

```bash
cd Com.Coppel.Web.Api
dotnet ef migrations add InitialCreate --context SpecDbContext
dotnet ef database update --context SpecDbContext
```

### 3. Ejecutar la aplicación

```bash
dotnet run --project Com.Coppel.Web.Api
```

### 4. Acceder a Swagger

```
http://localhost:5000/swagger
```

## 📚 API Endpoints

### Evaluaciones

- `POST /v1/evaluations` - Crear evaluación (subiendo PDFs)
- `GET /v1/evaluations/{id}` - Obtener evaluación por ID

### Revisiones

- `GET /v1/revisions` - Listar revisiones (paginado)
- `GET /v1/revisions/{revisionId}` - Obtener detalle de revisión

### Health Checks

- `GET /health` - Estado de salud de la aplicación

## 🌐 Despliegue en Cloud Run

### Sin Docker (Cloud Buildpacks)

```bash
gcloud run deploy frontend-doc-certifier-backend \
  --source . \
  --region us-central1 \
  --allow-unauthenticated \
  --add-cloudsql-instances "PROJECT:us-central1:frontspeccert-sql" \
  --update-env-vars \
    "INSTANCE_CONNECTION_NAME=PROJECT:us-central1:frontspeccert-sql" \
    "DB_NAME=specdb" \
    "DB_USER=specuser" \
    "GENIUS_BASE_URL=https://api-genius.coppel.com" \
    "CORS_ALLOWED_ORIGINS=*" \
  --set-secrets \
    "DB_PASSWORD=projects/PROJECT/secrets/db-password:latest" \
    "GENIUS_AUTH=projects/PROJECT/secrets/genius-auth:latest"
```

## 📝 Variables de Entorno

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `INSTANCE_CONNECTION_NAME` | Conexión a Cloud SQL | `PROJECT:REGION:INSTANCE` |
| `DB_NAME` | Nombre de la base de datos | `specdb` |
| `DB_USER` | Usuario de la base de datos | `specuser` |
| `DB_PASSWORD` | Password (desde Secret Manager) | - |
| `GENIUS_BASE_URL` | URL base de GENIUS API | `https://api-genius.coppel.com` |
| `GENIUS_AUTH` | Token de autenticación (desde Secret Manager) | - |
| `CORS_ALLOWED_ORIGINS` | Orígenes permitidos | `*` o lista separada por comas |

## 🧪 Testing

```bash
# Todos los tests
dotnet test

# Solo tests unitarios
dotnet test Com.Coppel.Web.Api.UnitTests

# Solo tests de integración
dotnet test Com.Coppel.Web.Api.IntegrationTests
```

## 📁 Estructura del Proyecto

```
Com.Coppel.Web.Api/
├── Core/
│   ├── Domain/
│   │   ├── Entities/        # Revision, Evaluation, Criterion, etc.
│   │   └── Interfaces/      # IEvaluationRepository, IGeniusApiClient
│   └── Application/
│       ├── DTOs/            # EvaluationDto, RevisionDto, etc.
│       └── UseCases/        # Commands y Queries
├── Infrastructure/
│   ├── Clients/             # GeniusApiClient
│   └── Persistence/         # DbContext, Repositorios
└── Presentation/
    └── Controllers/          # EvaluationsController
```

## 🔒 Seguridad

- **HTTPS** obligatorio en producción
- **CORS** configurable por entorno
- **Health Checks** para monitoreo
- **Secret Manager** para credenciales

## 📞 Soporte

Para soporte técnico o preguntas:
- **Email**: <desarrollo@coppel.com>
- **Documentación**: [Wiki interno](https://wiki.coppel.com)

---

### Desarrollado con ❤️ por el equipo de Coppel
