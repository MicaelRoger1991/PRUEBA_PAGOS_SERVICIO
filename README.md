# PRUEBA PAGOS SERVICIO
Solución interna para registrar pagos de servicios básicos (agua, electricidad, telecomunicaciones) de clientes.

## 1. Requisitos locales
- .NET 8
- Docker (PostgreSQL local)

## 2. Base de datos (PostgreSQL en Docker)
Contenedor: `postgres-container`

### 2.1 Credenciales sugeridas
- Superusuario: `postgres`
- Password: `TuPassword`
- Usuario app: `userdb_esapp`
- Password app: `userdb_esapp`
- Base de datos: `db_esapp`

### 2.2 Crear usuario y base
Ejecuta estos comandos una sola vez:

```bash
docker exec -it postgres-container psql -U postgres -c "CREATE USER userdb_esapp WITH PASSWORD 'userdb_esapp';"
docker exec -it postgres-container psql -U postgres -c "CREATE DATABASE db_esapp OWNER userdb_esapp;"
docker exec -it postgres-container psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE db_esapp TO userdb_esapp;"
```

## 3. Configuración de conexión (appsettings.json)
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

### 3.1 Cómo cifrar el password
Usa `SecurityEncrypt` (mismo algoritmo que `DataAccess`). Puedes usar este snippet en una consola temporal:

```csharp
using EsApp.CROSS.Encrypt;

var encrypt = new SecurityEncrypt("QWERTY0123456789");
Console.WriteLine(encrypt.encrypt("userdb_esapp"));
```

Pega el resultado en `Password` dentro de `appsettings.json`.

## 4. EF - Plan de migración (Code First)
Pasos generales:
1) Agregar paquetes EF Core para PostgreSQL.
2) Crear `DbContext` y entidades mínimas (auth/sessions).
3) Crear migraciones iniciales y aplicar a la base (`dotnet ef database update`).
4) Reemplazar `DataAccess` y repositorios ADO por repositorios EF.

## 5. EF - Paso a paso
1) Asegura Postgres arriba y la BD creada (ver sección de Base de datos).
2) Restaura paquetes:
```bash
dotnet restore
```
1) Instala la herramienta de migraciones 
```bash
dotnet tool install --global dotnet-ef
```
1) Crea la migración inicial:
```bash
dotnet ef migrations add InitialCreate \
  -p Infrastructure/EsApp.Persistence/EsApp.Persistence.csproj \
  -s Presentation/EsApp.Api/EsApp.Api.csproj \
  -o Infrastructure/EsApp.Persistence/Migrations
```
Si ya existe `InitialCreate`, crea una migración nueva con otro nombre, por ejemplo:
```bash
dotnet ef migrations add AddPaymentsServices \
  -p Infrastructure/EsApp.Persistence/EsApp.Persistence.csproj \
  -s Presentation/EsApp.Api/EsApp.Api.csproj \
  -o Infrastructure/EsApp.Persistence/Migrations
```
1) Aplica la migración a la base:
```bash
dotnet ef database update \
  -p Infrastructure/EsApp.Persistence/EsApp.Persistence.csproj \
  -s Presentation/EsApp.Api/EsApp.Api.csproj
```

Notas:
- La conexión se construye desde `DataBaseSettings` usando `ISecurityEncrypt`.
- Si la red está restringida, habilita acceso a NuGet antes de `dotnet restore`.

## 6. Usuarios de prueba
Se insertan 3 usuarios al aplicar migraciones (tabla `usersMaster`):
- `caja` / pass: `123abc` / rol: `CAJA`
- `plataforma` / pass: `123abc` / rol: `PLATAFORMA`
- `gerente` / pass: `123abc` / rol: `GERENTE`

Estos usuarios sirven para probar el login/JWT desde el inicio.

Se insertan 2 clientes de prueba (tabla `customers`):
- `Carlos Lopez` / documento: `12345678`
- `Maria Fernandez` / documento: `87654321`

## 7. Cómo consumir el API
Flujo simple para probar el API (todos los endpoints requieren JWT):

1) Auth - Login (obligatorio para obtener token)
```http
POST /Auth/Login
Content-Type: application/json

{
  "user": "caja",
  "password": "123abc"
}
```

2) Auth - RefreshToken (cuando el token expira)
```http
PUT /Auth/RefreshToken
Content-Type: application/json

{
  "token": "<token_actual>",
  "refreshToken": "<refresh_token>"
}
```

3) Parametrics - GetCurrency
Sirve para obtener los tipos de moneda disponibles y su ID (datos paramétricos).
```http
GET /Parametrics/GetCurrency
```

4) ServiceProvider - GetServiceProvider
Lista los servicios que se pueden pagar y su tipo de moneda.
```http
GET /ServiceProvider/GetServiceProvider
```

5) Customers - GetCustomerByDocumentNumber
Busca cliente por número de documento. Requiere JWT y rol **CAJA**, **PLATAFORMA** o **GERENTE**.
```http
GET /Customers/GetCustomerByDocumentNumber?documentNumber=12345678
```

6) Header para endpoints protegidos
```http
Authorization: Bearer <token>
```

## 8. Arquitectura
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

## 9. Avance del examen (lo ya logrado)
- Migración a EF Core (Code First) con PostgreSQL.
- Esquema completo y relaciones: usuarios, sesiones, monedas, clientes, proveedores y pagos.
- Usuarios, clientes y catálogos para pruebas rápidas.
- Endpoints listos (monedas, servicios, clientes por documento).

## 10. Extras no solicitados (pero ya incluidos)
- Login con JWT + refresh token.
- Control de rol en endpoints (ej. clientes solo CAJA/PLATAFORMA/GERENTE).
- Test unitario básico para encriptación.


## 11. Tests
Proyecto de pruebas: `Presentation/EsApp.Api.Test`

### Prueba de encriptación
Incluye un test de ida y vuelta (encrypt/decrypt) para validar `SecurityEncrypt`.

Ejecutar tests:
```bash
dotnet test Presentation/EsApp.Api.Test/EsApp.Api.Test.csproj
```
