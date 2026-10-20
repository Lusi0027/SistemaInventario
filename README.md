# Sistema de gestión de Categorías, Suplidores y Productos

**ISW-124 Programación Avanzada — Proyecto 1 (individual)**
Aplicación de escritorio en **C# / Windows Forms / SQL Server** con arquitectura por capas.

## Funcionalidades
- Login (usuario de prueba: `admin` / `admin123`) y menú principal.
- CRUD con borrado lógico de **Categorías** (hasta 5 fotos), **Suplidores** y **Productos**.
- Búsqueda en tiempo real en todos los listados; productos filtrables por categoría, suplidor y rango de precios.
- **Papelera** para ver y restaurar registros eliminados.
- No se puede eliminar una categoría/suplidor con productos activos.
- **Reportes** con 5 consultas LINQ (conteo por categoría, top 5 suplidores, promedios, etc.).
- Validaciones en la capa de negocio mostradas con `ErrorProvider`.
- Errores de base de datos registrados en `errores.log`.

## Arquitectura (4 proyectos)
| Proyecto | Contenido |
|---|---|
| `SistemaInventario.Entidades` | Modelos de dominio |
| `SistemaInventario.AccesoDatos` | `IRepositorio<T>`, `RepositorioBase<T>`, repositorios, `ConexionBD` (Singleton), `Logger` |
| `SistemaInventario.LogicaNegocio` | Servicios, validaciones, consultas LINQ y `FabricaServicios` (inyección de dependencias) |
| `SistemaInventario.Presentacion` | Formularios Windows Forms (solo referencia Entidades y Lógica de Negocio) |

Patrones: **Repository** (interfaz genérica), **Singleton** (conexión), **inyección de dependencias por constructor**.
Genéricos propios: `IRepositorio<T>`, `RepositorioBase<T>`, `Ejecutar<TResultado>()` y `Utilidades.Buscar<T>()`.

## Instalación
**Requisitos:** Windows, Visual Studio 2022 (carga de trabajo *Desarrollo de escritorio .NET*), .NET 8 SDK y SQL Server (Express o LocalDB).

1. Abrir SQL Server Management Studio y ejecutar en orden `database/schema.sql` y `database/datos.sql`.
2. Abrir `src/SistemaInventario.sln` en Visual Studio.
3. Si su SQL Server no es `localhost`, edite `src/SistemaInventario.Presentacion/conexion.txt`  
   (ej. SQL Express: `Server=.\SQLEXPRESS;Database=SistemaInventario;Trusted_Connection=True;TrustServerCertificate=True;`).
4. Establecer **SistemaInventario.Presentacion** como proyecto de inicio y presionar **F5**.
5. Iniciar sesión con `admin` / `admin123`.

### Generar el ejecutable (carpeta `/build`)
```
dotnet publish src/SistemaInventario.Presentacion -c Release -o build
```
(Recuerde copiar/editar `conexion.txt` junto al `.exe` si cambia el servidor.)

## Diagramas
- [Diagrama de clases](docs/diagrama-clases.md)
- [Diagrama entidad-relación](docs/diagrama-er.md)

## Capturas de pantalla
_(Agregue aquí las capturas: login, menú, categorías, suplidores, productos, papelera, reportes)_

## Video demostrativo
_(Enlace al video de 5–10 minutos)_
