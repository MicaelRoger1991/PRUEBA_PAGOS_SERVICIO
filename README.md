# PRUEBA_PAGOS_SERVICIO
Solución interna para registrar pagos de servicios básicos (agua, electricidad, telecomunicaciones) de clientes
Solución interna para registrar pagos de servicios básicos (agua, electricidad, telecomunicaciones) de clientes.

## Requisitos locales
- .NET 8
- Docker (PostgreSQL local)

## Base de datos (PostgreSQL en Docker)
Contenedor: `postgres-container`

### Credenciales sugeridas
- Superusuario: `postgres`
- Password: `TuPasswordSegura1234@*+`
- Usuario app: `userdb_esapp`
- Password app: `userdb_esapp`
- Base de datos: `db_esapp`

### Crear usuario y base
Ejecuta estos comandos una sola vez:

```bash
docker exec -it postgres-container psql -U postgres -c "CREATE USER userdb_esapp WITH PASSWORD 'userdb_esapp';"
docker exec -it postgres-container psql -U postgres -c "CREATE DATABASE db_esapp OWNER userdb_esapp;"
docker exec -it postgres-container psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE db_esapp TO userdb_esapp;"
```

## Configuración de conexión (appsettings.json)
La app usa `DataBaseSettings` con password cifrado y `secretKey` para desencriptar.

Ejemplo de configuración:
```json
"DataBaseSettings": {
  "ConnectionStrings": [
    {
      "Name": "EsAppDb",
      "Server": "localhost",
      "DataBase": "db_esapp",
      "User": "userdb_esapp",
      "Password": "<PASSWORD_CIFRADO>",
      "Timeout": 300
    }
  ]
},
"secretKey": "QWERTY0123456789"
```

### Cómo cifrar el password
Usa `SecurityEncrypt` (mismo algoritmo que `DataAccess`). Puedes usar este snippet en una consola temporal:

```csharp
using EsApp.CROSS.Encrypt;

var encrypt = new SecurityEncrypt("QWERTY0123456789");
Console.WriteLine(encrypt.encrypt("userdb_esapp"));
```

Pega el resultado en `Password` dentro de `appsettings.json`.

## EF Core (Code First) - Plan de migración
Este proyecto migrará desde ADO.NET a EF Core con enfoque Code First.

Pasos generales:
1) Agregar paquetes EF Core para PostgreSQL.
2) Crear `DbContext` y entidades mínimas (auth/sessions).
3) Crear migraciones iniciales y aplicar a la base (`dotnet ef database update`).
4) Reemplazar `DataAccess` y repositorios ADO por repositorios EF.

## EF Core (Code First) - Paso a paso
1) Asegura Postgres arriba y la BD creada (ver sección de Base de datos).
2) Restaura paquetes:
```bash
dotnet restore
```
3) Instala la herramienta de migraciones (si no la tienes):
```bash
dotnet tool install --global dotnet-ef
```
4) Crea la migración inicial:
```bash
dotnet ef migrations add InitialCreate \
  -p Infrastructure/EsApp.Persistence/EsApp.Persistence.csproj \
  -s Presentation/EsApp.Api/EsApp.Api.csproj \
  -o Infrastructure/EsApp.Persistence/Migrations
```
5) Aplica la migración a la base:
```bash
dotnet ef database update \
  -p Infrastructure/EsApp.Persistence/EsApp.Persistence.csproj \
  -s Presentation/EsApp.Api/EsApp.Api.csproj
```

Notas:
- La conexión se construye desde `DataBaseSettings` usando `ISecurityEncrypt`.
- Si la red está restringida, habilita acceso a NuGet antes de `dotnet restore`.

## Usuarios de prueba (seed)
Se insertan 3 usuarios al aplicar migraciones (tabla `usersMaster`):
- `caja` / pass: `123abc` / rol: `CAJA`
- `plataforma` / pass: `123abc` / rol: `PLATAFORMA`
- `gerente` / pass: `123abc` / rol: `GERENTE`

Estos usuarios sirven para probar el login/JWT desde el inicio.

## Cómo consumir el API (Auth)
Flujo simple para probar autenticación:

1) Login (obligatorio para obtener token)
```http
POST /Auth/Login
Content-Type: application/json

{
  "user": "caja",
  "password": "123abc"
}
```

2) Refresh token (cuando el token expira)
```http
PUT /Auth/RefreshToken
Content-Type: application/json

{
  "token": "<token_actual>",
  "refreshToken": "<refresh_token>"
}
```

3) Consumir endpoints protegidos
```http
Authorization: Bearer <token>
```

## Arquitectura
Se usa una arquitectura modular con enfoque hexagonal:
- `Presentation`: endpoints, middleware y configuración de la API.
- `Application`: casos de uso y validaciones.
- `Domain`: entidades y contratos (interfaces).
- `Infrastructure`: persistencia, autenticación y servicios externos.
- `Core`: utilidades transversales (logger, encrypt, shared).

Buenas prácticas aplicadas:
- Separación de responsabilidades por capa.
- Repositorios como abstracción de persistencia.
- DTOs/Request/Response y validaciones en Application.
- Configuración centralizada y reutilizable.

## Solución de problemas
### Error al instalar dotnet-ef
Si ves un error tipo `DotnetToolSettings.xml was not found`, limpia caches y reinstala:

```bash
dotnet nuget locals all --clear
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 9.0.0
```

### Error de compilación al generar migraciones
Si `dotnet ef migrations add` falla con "Build failed":
1) Compila para ver el error real:
```bash
dotnet build Presentation/EsApp.Api/EsApp.Api.csproj -v minimal
```
2) Corrige los errores y vuelve a ejecutar la migración.

## Tests
Proyecto de pruebas: `Presentation/EsApp.Api.Test`

### Prueba de encriptación
Incluye un test de ida y vuelta (encrypt/decrypt) para validar `SecurityEncrypt`.

Ejecutar tests:
```bash
dotnet test Presentation/EsApp.Api.Test/EsApp.Api.Test.csproj
```
