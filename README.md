# Sistema de Inventario — ISW-124 Proyecto 1

Aplicación de escritorio (C# / Windows Forms / SQL Server) para gestionar **Categorías, Suplidores y Productos**
con arquitectura por capas, Repository genérico, Singleton, inyección por constructor, LINQ y soft-delete.

## Requisitos
- Windows 10/11, [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) y Visual Studio 2022 (opcional)
- SQL Server (Express / LocalDB / Developer)

## Instalación
1. **Base de datos:** abrir SSMS y ejecutar en orden `database/schema.sql` y `database/datos.sql`.
2. **Cadena de conexión:** editar `src/SistemaInventario.Presentacion/appsettings.json` según su instancia
   (ej. `Server=.\SQLEXPRESS;Database=SistemaInventario;Trusted_Connection=True;TrustServerCertificate=True;`).
3. **Ejecutar:** abrir `src/SistemaInventario.sln`, establecer *Presentacion* como proyecto de inicio y pulsar F5.
4. **Ejecutable Release:**
   ```powershell
   dotnet publish src/SistemaInventario.Presentacion -c Release -o build
   ```

## Estructura
```
/SistemaInventario
├── README.md
├── /src
│   ├── SistemaInventario.Entidades      (modelos, excepciones, DTO de reportes)
│   ├── SistemaInventario.AccesoDatos    (IRepositorio<T>, repositorios, ConexionBD Singleton, Logger)
│   ├── SistemaInventario.LogicaNegocio  (servicios, validadores, LINQ, ContenedorServicios)
│   └── SistemaInventario.Presentacion   (Windows Forms: Controles/ listados, Formularios/ diálogos)
├── /database   (schema.sql, datos.sql)
├── /docs       (diagrama-clases.md, diagrama-er.md — Mermaid, se ven en GitHub)
└── /build      (ejecutable Release)
```
Dependencias: `Presentacion → LogicaNegocio → AccesoDatos → Entidades`. Presentacion solo referencia
Entidades y LogicaNegocio; el objeto `ContenedorServicios.Crear(cadena)` arma los repositorios y los inyecta.

## Pantallas
Menú lateral con **Categorías**, **Suplidores**, **Productos** (filtros por categoría, suplidor y rango de precios),
**Papelera** (restaurar categorías, suplidores y productos) y **Reportes** (las consultas LINQ). Todos los listados
tienen búsqueda en tiempo real; los formularios de alta/edición muestran los errores con `ErrorProvider`.

## Cómo usar la capa de negocio desde la UI
```csharp
var servicios = ContenedorServicios.Crear(cadenaConexion);   // ya lo hace Program.cs
servicios.Categorias.Listar(txtBuscar.Text);                  // filtrado en tiempo real
servicios.Productos.Listar(new ProductoFiltro { CategoriaId = 2, PrecioMinimo = 500 });

try { servicios.Productos.Crear(producto); }
catch (ValidacionException ex) { errorProvider.MostrarErrores(ex.Errores, mapaControles); } // clave = nombre de propiedad
catch (ExcepcionAplicacion ex) { MessageBox.Show(ex.Message); }   // Negocio / AccesoDatos
```
Claves de error por entidad: `Nombre`, `Descripcion`, `Fotos` · `NombreEmpresa`, `NombreContacto`, `Telefono`, `Correo`,
`SitioWeb` · `Nombre`, `PrecioUnitario`, `CategoriaId`, `SuplidorId`.
Papelera: `ListarEliminadas()/ListarEliminados()` y `Restaurar(id)` en cada servicio.

## Requisitos técnicos → dónde están
| Requisito | Ubicación |
|---|---|
| Repository `IRepositorio<T>` | `AccesoDatos/Interfaces/IRepositorio.cs`, `Repositorios/RepositorioBase.cs` |
| Singleton conexión | `AccesoDatos/ConexionBD.cs` |
| Inyección por constructor | Servicios y repositorios; composición en `LogicaNegocio/ContenedorServicios.cs` |
| Genéricos propios adicionales | `Validador<T>`, `RepositorioBase<T>`, `Buscador.Filtrar<T>` |
| 6 consultas LINQ | `LogicaNegocio/Servicios/ReporteServicio.cs` (+ filtros en `ProductoServicio.Listar`) |
| Validaciones | `LogicaNegocio/Validadores` (email, teléfono, URL, precio, foto, nombres) |
| try/catch `SqlException` + Logger | `RepositorioBase.Ejecutar` → `errores.log` |
| Soft-delete + reglas | `Activo BIT`; `Eliminar` bloquea si hay productos activos; `Restaurar` exige padres activos |

## Capturas de pantalla
Categorias:
![alt text](image.png)
Suplidores:
![alt text](image-1.png)
Productos:
![alt text](image-2.png)
Papelera:
![alt text](image-3.png)
Reportes:
![alt text](image-4.png)

## Video demostrativo
_Enlace al video (5–10 min)._
