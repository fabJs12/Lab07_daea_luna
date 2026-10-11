# Laboratorio 07 - Personas (Sección A)

Aplicación WPF con SQL Server, ADO.NET, arquitectura por capas y LINQ.
Se conserva la solución `Lab07.slnx` y el proyecto WPF original en .NET 10
(`net10.0-windows`). No se creó el documento Word ni se realizaron operaciones de Git.

## Proyectos y referencias

- Entidades: `Persona`, `EstadoPersona`, `PersonaEstado` y `ResumenEstado`.
- Datos → Entidades: `Conexion`, `PersonaDatos` y `EstadoPersonaDatos`.
- Negocio → Datos y Entidades: `PersonaNegocio`.
- Lab07 → Negocio y Entidades: presentación mediante eventos WPF.

Datos es la única capa con acceso a SQL Server. Usa `Microsoft.Data.SqlClient`
7.1.1 como único paquete NuGet directo, con sus dependencias automáticas.
La presentación no utiliza SQL ni referencia directamente a Datos.

## Ejecutar

En Windows, con el SDK .NET 10 y SQL Server disponible:

```powershell
dotnet build Lab07.slnx
dotnet run --project Lab07/Lab07.csproj
```

En Visual Studio, abrir `Lab07.slnx` y seleccionar **Lab07** como proyecto de inicio.
La cadena de conexión está centralizada en `Datos/Conexion.cs`, en
`CadenaConexion`. Cambiar allí `localhost\SQLEXPRESS` si se usa otra instancia.
La conexión utiliza autenticación de Windows y la base `Lab07DB`.

## Contrato de la base de datos

No se encontró un archivo `.sql` en la solución. No se creó ni modificó ninguno.
Se comprobaron las tablas, columnas, parámetros y definiciones directamente en
la base existente `Lab07DB` de `localhost\SQLEXPRESS`.

`Persona` conserva exactamente cinco campos: `idPersona`, `nombres`,
`apellidos`, `dni` y `estado`. El catálogo `EstadoPersona` estaba vacío:
se agregaron los valores requeridos **1 = Activo** y **0 = Eliminado**.

`PersonaDatos` consume estos procedimientos existentes:

- `usp_Persona_Listar`: personas activas.
- `usp_Persona_ListarTodos`: personas activas y eliminadas.
- `usp_Persona_Insertar`: `@nombres`, `@apellidos`, `@dni`.
- `usp_Persona_Actualizar`: `@idPersona`, `@nombres`, `@apellidos`, `@dni`.
- `usp_Persona_Eliminar`: `@idPersona`; ejecuta `UPDATE Persona SET estado = 0`.

`EstadoPersonaDatos` consume `usp_EstadoPersona_Listar`.
El procedimiento existente `usp_Persona_Buscar` no se utiliza: la búsqueda
requerida se realiza con LINQ en Negocio.

## Demostraciones LINQ en PersonaNegocio

| Método | Operación | Botón |
| --- | --- | --- |
| `Listar()` | `from ... select`, consulta simple | Listar |
| `Buscar(texto)` | `Where`, nombres/apellidos sin distinguir mayúsculas y DNI | Buscar |
| `OrdenarPorApellido()` | `OrderBy` y `ThenBy` | Ordenar A-Z |
| `ListarConEstado()` | `join`, incluye personas eliminadas | Ver estados |
| `AgruparPorEstado()` | `GroupBy`, `Select`, `Count`, `OrderBy` | Agrupar |

## Verificación realizada

La solución completa compiló con **0 errores y 0 advertencias**.
Pasaron **34 comprobaciones** reales: validaciones, inserción, actualización,
eliminación lógica, consulta simple, búsquedas, ordenamiento, JOIN, agrupación,
carga inicial WPF y selección segura de los tres tipos de resultados.

Las personas de prueba se insertaron dentro de una transacción y se revirtieron.
No quedan personas de prueba en la base; los dos estados sí permanecen.
Las pruebas de inserción pueden dejar saltos normales en el IDENTITY tras el rollback.
El programa de comprobación estuvo fuera de la solución entregada.

## Pruebas manuales y capturas para el Word

1. Abrir la aplicación y comprobar el listado inicial de activos.
2. Pulsar Nuevo y Guardar con datos válidos; capturar el registro en el listado.
3. Probar campos vacíos y DNI con letras o menos de ocho números; capturar el mensaje.
4. Seleccionar una persona, editar y Actualizar; comprobar el cambio.
5. Buscar por nombre, apellido y DNI, incluyendo mayúsculas y búsqueda vacía.
6. Crear personas con el mismo apellido y nombres diferentes; comprobar Ordenar A-Z.
7. Cancelar una eliminación y luego confirmarla; comprobar que desaparece de Listar.
8. Pulsar Ver estados; capturar activos y eliminados, demostrando la eliminación lógica.
9. Pulsar Agrupar; capturar los conteos por estado.
10. Seleccionar filas en Ver estados y Agrupar; comprobar que no hay conversiones inválidas.

También conviene capturar los cuatro proyectos y sus referencias en Visual Studio,
las cinco consultas de `PersonaNegocio`, ADO.NET en `PersonaDatos` y la compilación correcta.
