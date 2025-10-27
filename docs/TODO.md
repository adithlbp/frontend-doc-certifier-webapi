# TODO - Pendientes de Implementación

## ✅ Completado

1. ✅ Estructura base del proyecto .NET 8 con Clean Architecture
2. ✅ Entidades de dominio (Revision, Evaluation, Criterion, EvaluationCriterion)
3. ✅ DTOs basados en YAML OpenAPI
4. ✅ Repositorios (Evaluation, Revision, Criterion)
5. ✅ DbContext con configuraciones EF Core para PostgreSQL
6. ✅ Cliente Genius API con prompt estructurado
7. ✅ Handlers CQRS (CreateEvaluation, GetEvaluation, GetRevisions)
8. ✅ Controllers (Evaluations, Revisions)
9. ✅ Program.cs con DI, CORS configurable, Health Checks
10. ✅ Actualización de PDF a Markdown
11. ✅ Formato JSON obligatorio en Genius API
12. ✅ Documentación (README, ARCHITECTURE)

## ⚠️ Pendiente

### 1. Migraciones EF Core
```bash
# Instalar herramienta (ya instalado)
export PATH="$PATH:/Users/adith/.dotnet/tools"

# Crear migración inicial
cd Com.Coppel.Web.Api
dotnet ef migrations add InitialCreate --context SpecDbContext --output-dir Migrations

# Aplicar migración
dotnet ef database update --context SpecDbContext
```

### 2. Validación de Archivos Markdown
- ✅ Ya se valida `file_general` y `file_servicio` son requeridos
- ⚠️ **Falta**: Validar extensión .md
- ⚠️ **Falta**: Validar tamaño máximo de archivos
- ⚠️ **Falta**: Validar que el contenido sea Markdown válido

### 3. Persistencia de Criterios Individuales
- ✅ Los resultados completos se guardan en `RawResult` (jsonb)
- ⚠️ **Falta**: Persistir `EvaluationCriterion` en tabla separate
- ⚠️ **Falta**: Implementar repositorio para criterios de evaluación

### 4. Extracción de Metadatos del Markdown
El DBML especifica que las revisiones deben tener:
- `spec_version`
- `spec_date`
- `spec_author`

**Actual**: No se extrae automáticamente  
**Falta**: Parser de Markdown para extraer estos metadatos del YAML frontmatter:

```markdown
---
version: "1.0"
date: "2025-01-15"
author: "Equipo Frontend"
---
```

### 5. Mapeo Completo DTOs
- ✅ `EvaluationDto` básico implementado
- ⚠️ **Falta**: Completar mapeo de criteria individuales en MapToDto
- ⚠️ **Falta**: Incluir scores por severidad completos

### 6. Tests
- ⚠️ **Falta**: Unit tests para handlers
- ⚠️ **Falta**: Integration tests para repositorios
- ⚠️ **Falta**: E2E tests para endpoints

### 7. Configuración de Producción
- ✅ appsettings.json con variables de entorno
- ⚠️ **Falta**: Archivo de configuración para Cloud Run
- ⚠️ **Falta**: Scripts de despliegue

### 8. Error Handling
- ⚠️ **Falta**: Validar timeout de Genius API
- ⚠️ **Falta**: Retry logic para llamadas a Genius API
- ⚠️ **Falta**: Validar rate limits

## 🔧 Tareas Específicas Recomendadas

### Alta Prioridad

1. **Crear migraciones EF Core**
   - Comando: `dotnet ef migrations add InitialCreate`
   - Aplicar: `dotnet ef database update`

2. **Validar archivos Markdown**
   - Agregar validación de extensión .md
   - Validar tamaño máximo (ej: 10MB)
   - Validar estructura de Markdown básica

3. **Implementar Parser de Frontmatter**
   - Extraer metadatos del Markdown
   - Rellenar `spec_version`, `spec_date`, `spec_author`

### Media Prioridad

4. **Persistir Criterios Individuales**
   - Crear método en repositorio para agregar criterios
   - Iterar sobre `result.Criteria` y guardar

5. **Completar Mapeo de DTOs**
   - Incluir todos los criterios en el DTO de respuesta
   - Mapear scores por severidad completos

### Baja Prioridad

6. **Tests**
   - Unit tests para command handlers
   - Integration tests para repositorios
   - E2E tests básicos

7. **Documentación Adicional**
   - API.md con ejemplos de uso
   - DB.md con esquema detallado
   - ADRs (Architecture Decision Records)

## 📝 Notas de Implementación

### Sobre el Procesamiento de Markdown

El servicio lee archivos Markdown como texto plano y los envía directamente a Genius API. No se requiere:
- Parser de Markdown
- Conversión a HTML
- Renderizado

Solo lectura de archivos `.md` y envío del contenido como string.

### Sobre Genius API

El prompt incluye instrucciones claras y un ejemplo de formato JSON, forzando `response_format: "json_object"`. Esto asegura que la respuesta sea parseable.

### Sobre la Regla de Gate

La regla "FAIL si ≥1 criterio CRITICAL en FAIL" se implementa en dos niveles:
1. En el prompt de Genius API (instrucciones)
2. En la lógica del handler (validación después de recibir respuesta)

