

# Sistema de Inventario

Sistema de escritorio desarrollado en **C# con Windows Forms y SQL Server** para administrar categorías, suplidores y productos.

El proyecto utiliza una arquitectura por capas para separar las interfaces, las reglas de negocio y el acceso a la base de datos. También incluye patrones de diseño, programación genérica, consultas LINQ y eliminación lógica de registros.

## Requisitos para ejecutar el proyecto

- Windows 10 u 11.
- .NET 8 SDK.
- Visual Studio 2022.
- SQL Server (Express, LocalDB o Developer).
- SQL Server Management Studio (SSMS).

## Instalación

1. Abrir SQL Server Management Studio y ejecutar los archivos `schema.sql` y `datos.sql`, ubicados en la carpeta `database`, en ese mismo orden.
2. Abrir el archivo `appsettings.json`, ubicado en `src/SistemaInventario.Presentacion`, y configurar la cadena de conexión según el servidor SQL Server que se esté utilizando.
3. Abrir la solución `SistemaInventario.sln` desde Visual Studio.
4. Seleccionar `SistemaInventario.Presentacion` como proyecto de inicio.
5. Presionar **F5** para ejecutar el programa.


## Organización del proyecto

El sistema está dividido en cuatro proyectos:

**SistemaInventario.Entidades:** contiene las clases que representan las categorías, suplidores y productos, además de las excepciones y los modelos utilizados en los reportes.

**SistemaInventario.AccesoDatos:** se encarga de las operaciones con SQL Server. Aquí se encuentran los repositorios, las interfaces, la conexión a la base de datos y el registro de errores.

**SistemaInventario.LogicaNegocio:** contiene los servicios, las validaciones y las reglas que deben cumplirse antes de guardar o modificar información.

**SistemaInventario.Presentacion:** contiene los formularios de Windows Forms que utiliza el usuario para interactuar con el sistema.

La capa de presentación utiliza los servicios de lógica de negocio, sin acceder directamente a los repositorios.

## Funciones del sistema

### Categorías

Permite registrar, consultar, editar y eliminar categorías. Cada una tiene un nombre, una descripción y puede incluir hasta cinco fotografías.

### Suplidores

Permite administrar la información de los proveedores, incluyendo nombre de la empresa, contacto, teléfono, correo electrónico y sitio web.

### Productos

Permite registrar productos con su nombre, precio, categoría y suplidor. También cuenta con filtros para buscar productos por nombre, categoría, suplidor y rango de precios.

### Papelera

Los registros eliminados no se borran definitivamente de la base de datos. Se marcan como inactivos y pueden recuperarse desde la Papelera.

El sistema también evita eliminar categorías o suplidores que tengan productos activos relacionados.

### Reportes

Se implementaron seis consultas LINQ para obtener información del inventario:

1. Cantidad de productos por categoría.
2. Los cinco suplidores con más productos.
3. Productos dentro de un rango de precios y una categoría.
4. Búsqueda de productos por nombre.
5. Precio promedio de los productos por categoría.
6. Producto más caro de cada categoría.

## Validaciones

Antes de guardar los datos, el sistema verifica que la información sea correcta.

- Los nombres y campos obligatorios no pueden estar vacíos.
- Los teléfonos deben tener un formato dominicano válido.
- Los correos electrónicos y sitios web deben tener un formato correcto.
- Los precios deben ser mayores que cero y tener como máximo dos decimales.
- Las fotografías deben ser JPG, JPEG o PNG y no superar los 2 MB.

Cuando algún dato es incorrecto, se muestra un mensaje en el formulario mediante `ErrorProvider`.

## Patrones de diseño y otros conceptos utilizados

**Repository:** permite organizar las operaciones de acceso a datos mediante la interfaz genérica `IRepositorio<T>`.

**Singleton:** se utiliza en `ConexionBD` para trabajar con una única instancia de configuración de conexión.

**Inyección de dependencias:** los repositorios se pasan a los servicios mediante sus constructores.

**Genéricos:** se utilizan en clases y métodos como `RepositorioBase<T>`, `Validador<T>` y `Buscador.Filtrar<T>` para reutilizar código.

**LINQ:** se utiliza para realizar búsquedas, agrupaciones, filtros y cálculos sobre los productos.

## Manejo de errores

Las operaciones de acceso a la base de datos cuentan con manejo de excepciones para identificar errores de SQL Server.

También se creó una clase `Logger` que guarda los errores en un archivo llamado `errores.log`, lo que permite consultar los problemas que puedan ocurrir durante la ejecución.

## Base de datos

La base de datos está desarrollada en SQL Server y contiene las tablas necesarias para almacenar categorías, suplidores, productos y fotografías.

Se utilizan claves primarias y foráneas para relacionar los registros y mantener la integridad de los datos.

Los archivos SQL se encuentran en la carpeta `database`:

- `schema.sql`: crea la estructura de la base de datos.
- `datos.sql`: inserta los registros de prueba.

## Archivos del proyecto

El repositorio incluye las siguientes carpetas y archivos:

- `src`: código fuente de los cuatro proyectos.
- `database`: scripts de creación y datos de prueba.
- `docs`: diagramas de clases y entidad-relación.
- `build`: ejecutable compilado en Release.
- `README.md`: información e instrucciones para ejecutar el sistema.

## Capturas de pantalla

<img width="653" height="380" alt="image" src="https://github.com/user-attachments/assets/38b7e03d-9de8-4821-aa95-bd7c46e19568" />

<img width="581" height="346" alt="image" src="https://github.com/user-attachments/assets/1585afda-1ee7-4922-a737-8d0ea4bd19b0" />

<img width="565" height="350" alt="image" src="https://github.com/user-attachments/assets/5c3e5c18-dfa8-4b75-b94c-d4b414382dcb" />


<img width="561" height="335" alt="image" src="https://github.com/user-attachments/assets/09ff17ae-7e44-4c17-8b62-3d101e963727" />


<img width="557" height="334" alt="image" src="https://github.com/user-attachments/assets/d9fdcaf7-bbcb-4702-87d1-04d9870a7ad9" />



## Video demostrativo
_Enlace al video (5–10 min)._
