# Guía de Testing - Com.Coppel.Web.Api

## Estructura de Proyectos de Test

La solución incluye tres tipos de proyectos de test siguiendo las mejores prácticas de Microsoft:

```text
WebApiTemplate.sln
├── Com.Coppel.Web.Api/                    # Proyecto principal
├── Com.Coppel.Web.Api.UnitTests/          # Tests unitarios
├── Com.Coppel.Web.Api.IntegrationTests/   # Tests de integración
└── Com.Coppel.Web.Api.EndToEndTests/      # Tests end-to-end
```

## Tipos de Tests

### 1. Tests Unitarios (`Com.Coppel.Web.Api.UnitTests`)

- **Propósito**: Probar lógica de negocio aislada
- **Alcance**: Entidades de dominio, casos de uso, validaciones
- **Velocidad**: Muy rápidos (< 1ms por test)
- **Dependencias**: Mocks y stubs

**Ejemplos incluidos:**

- `ProductoTests.cs`: Tests de la entidad Producto
- `CreateProductoCommandHandlerTests.cs`: Tests del caso de uso

### 2. Tests de Integración (`Com.Coppel.Web.Api.IntegrationTests`)

- **Propósito**: Probar interacción entre componentes
- **Alcance**: Repositorios, base de datos, servicios
- **Velocidad**: Moderados (10-100ms por test)
- **Dependencias**: Base de datos en memoria

**Ejemplos incluidos:**

- `ProductoRepositoryIntegrationTests.cs`: Tests del repositorio con BD

### 3. Tests End-to-End (`Com.Coppel.Web.Api.EndToEndTests`)

- **Propósito**: Probar serialización, DTOs y estructura de respuestas
- **Alcance**: Modelos de datos, serialización JSON, códigos HTTP
- **Velocidad**: Rápidos (< 10ms por test)
- **Dependencias**: Sin dependencias externas (tests de estructura)

**Ejemplos incluidos:**

- `ProductosControllerE2ETests.cs`: Tests de DTOs y serialización

## Comandos de Ejecución

### Ejecutar todos los tests

```bash
dotnet test
```

### Ejecutar por tipo de test

```bash
# Solo tests unitarios
dotnet test Com.Coppel.Web.Api.UnitTests

# Solo tests de integración
dotnet test Com.Coppel.Web.Api.IntegrationTests

# Solo tests end-to-end
dotnet test Com.Coppel.Web.Api.EndToEndTests
```

### Ejecutar con filtros

```bash
# Excluir tests E2E (más rápido para desarrollo)
dotnet test --filter "FullyQualifiedName!~EndToEnd"

# Solo tests de una clase específica
dotnet test --filter "FullyQualifiedName~ProductoTests"
```

### Generar reporte de cobertura

```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults
```

### Modo watch para desarrollo

```bash
dotnet watch test --project Com.Coppel.Web.Api.UnitTests
```

## Configuración de CI/CD

### Azure DevOps Pipeline

```yaml
- task: DotNetCoreCLI@2
  displayName: 'Run Unit Tests'
  inputs:
    command: 'test'
    projects: '**/*UnitTests.csproj'
    arguments: '--configuration $(BuildConfiguration) --collect:"XPlat Code Coverage"'

- task: DotNetCoreCLI@2
  displayName: 'Run Integration Tests'
  inputs:
    command: 'test'
    projects: '**/*IntegrationTests.csproj'

- task: DotNetCoreCLI@2
  displayName: 'Run E2E Tests'
  inputs:
    command: 'test'
    projects: '**/*EndToEndTests.csproj'
```

### GitHub Actions

```yaml
- name: Run Unit Tests
  run: dotnet test Com.Coppel.Web.Api.UnitTests --no-build --verbosity normal

- name: Run Integration Tests
  run: dotnet test Com.Coppel.Web.Api.IntegrationTests --no-build --verbosity normal

- name: Run E2E Tests
  run: dotnet test Com.Coppel.Web.Api.EndToEndTests --no-build --verbosity normal
```

## Convenciones de Naming

### Métodos de Test

```csharp
// Patrón: MethodName_StateUnderTest_ExpectedBehavior
[Fact]
public void Create_WithValidData_ShouldCreateProducto()

// Patrón alternativo: Given_When_Then
[Fact]
public void Given_ValidProduct_When_Creating_Then_ReturnsSuccess()
```

### Clases de Test

```csharp
// Para tests unitarios
public class ProductoTests
public class CreateProductoCommandHandlerTests

// Para tests de integración
public class ProductoRepositoryIntegrationTests

// Para tests E2E
public class ProductosControllerE2ETests
```

## Frameworks y Librerías Utilizadas

- **xUnit**: Framework de testing principal
- **FluentAssertions**: Assertions más legibles
- **Moq**: Mocking framework para tests unitarios (solo en UnitTests)
- **Microsoft.EntityFrameworkCore.InMemory**: Base de datos en memoria para tests de integración
- **Microsoft.AspNetCore.Mvc.Testing**: Testing de APIs (en Integration y E2E tests)
- **Microsoft.Playwright**: Automatización de navegadores (en E2E tests)
- **Newtonsoft.Json**: Serialización JSON para tests E2E
- **coverlet.collector**: Recolección de cobertura de código

## Mejores Prácticas

1. **AAA Pattern**: Arrange, Act, Assert
2. **Tests independientes**: Cada test debe poder ejecutarse solo
3. **Nombres descriptivos**: El nombre debe explicar qué se está probando
4. **Un concepto por test**: Cada test debe verificar una sola cosa
5. **Tests rápidos**: Los unitarios deben ser muy rápidos
6. **Datos de prueba**: Usar builders o factories para crear datos

## Agregar Nuevos Tests

### Para agregar un test unitario

1. Crear archivo en `Com.Coppel.Web.Api.UnitTests/`
2. Seguir la estructura de carpetas del proyecto principal
3. Usar mocks para dependencias externas

### Para agregar un test de integración

1. Crear archivo en `Com.Coppel.Web.Api.IntegrationTests/`
2. Usar `DbContextOptionsBuilder` con InMemory database
3. Crear contexto único por test con `Guid.NewGuid().ToString()`

### Para agregar un test E2E

1. Crear archivo en `Com.Coppel.Web.Api.EndToEndTests/`
2. Implementar `IDisposable` para limpieza de recursos
3. Probar DTOs, serialización y estructura de respuestas
