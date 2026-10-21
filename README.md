# Sistema de Inventario

**ISW-124 Programación Avanzada — Proyecto 1**

Programa desarrollado en C#, Windows Forms y SQL Server para manejar categorías, suplidores y productos.

## Funciones

- Agregar, editar, buscar y eliminar registros.
- Guardar hasta cinco fotos por categoría.
- Filtrar productos por categoría, suplidor y precio.
- Recuperar registros desde la papelera.
- Consultar cinco reportes con LINQ.
- Validar los datos ingresados y guardar los errores en errores.log.

Las categorías y los suplidores no se pueden eliminar si tienen productos activos.

## Estructura

El proyecto tiene cuatro capas:

- **Entidades:** clases que representan los datos.
- **Acceso a Datos:** conexión y operaciones con SQL Server.
- **Lógica de Negocio:** servicios, validaciones y reportes.
- **Presentación:** formularios del programa.

Se aplicaron Repository, Singleton, inyección de dependencias y genéricos para reutilizar código.

## Cómo ejecutarlo

Necesitas Visual Studio con desarrollo de escritorio .NET, el SDK de .NET 8 y SQL Server.

1. Ejecuta database/schema.sql.
2. En una copia local de database/datos.sql, asigna una contraseña a @ClaveInicial y ejecuta el script.
3. Abre src/SistemaInventario.sln.
4. Ajusta el servidor en conexion.txt.
5. Selecciona SistemaInventario.Presentacion como proyecto de inicio y presiona F5.
6. Entra con admin y la contraseña que configuraste.

## Capturas y video

<img width="207" height="263" alt="image" src="https://github.com/user-attachments/assets/d4912439-7963-4617-8736-b3a0ef97fa1b" />

<img width="450" height="322" alt="image" src="https://github.com/user-attachments/assets/54ed67c3-1037-4cda-a768-bf46c7baf3cd" />

<img width="731" height="427" alt="image" src="https://github.com/user-attachments/assets/cb4ae274-ec20-4cd7-ab7d-9ec12bdf2413" />

<img width="731" height="428" alt="image" src="https://github.com/user-attachments/assets/4e2795f0-3f9e-455d-8832-8419dda1817e" />

<img width="734" height="431" alt="image" src="https://github.com/user-attachments/assets/1046f64f-474e-49db-915d-202d4e1ea7f5" />

<img width="622" height="390" alt="image" src="https://github.com/user-attachments/assets/fa0786c1-3ca5-4549-8b34-25389e06aa18" />

<img width="583" height="366" alt="image" src="https://github.com/user-attachments/assets/4bb947a2-5c69-433f-8464-e21424a14460" />

**Link del video:** https://universidaduce-my.sharepoint.com/:v:/g/personal/lc2024-3658_uce_edu_do/IQBBPo7O1fvJS7U0uYkP6fVxAVj78DR_QWPcsyqaiAyLwPE?nav=eyJyZWZlcnJhbEluZm8iOnsicmVmZXJyYWxBcHAiOiJPbmVEcml2ZUZvckJ1c2luZXNzIiwicmVmZXJyYWxBcHBQbGF0Zm9ybSI6IldlYiIsInJlZmVycmFsTW9kZSI6InZpZXciLCJyZWZlcnJhbFZpZXciOiJNeUZpbGVzTGlua0NvcHkifX0&e=pjEtVu





