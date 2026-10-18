using System.Globalization;
using System.Text.RegularExpressions;
using SistemaInventario.Entidades.Excepciones;

namespace SistemaInventario.Presentacion.Utilidades;

/// <summary>Estilos y utilidades comunes de la interfaz.</summary>
public static class Ui
{
    public static readonly Color Primario = Color.FromArgb(37, 99, 235);
    public static readonly Color Peligro = Color.FromArgb(220, 38, 38);
    public static readonly Color Neutro = Color.FromArgb(107, 114, 128);
    public static readonly Color Oscuro = Color.FromArgb(17, 24, 39);
    public static readonly Font Fuente = new("Segoe UI", 9.5f);

    /// <summary>Crea un botón plano con el color indicado.</summary>
    public static Button Boton(string texto, Color color, int ancho = 100) => new()
    {
        Text = texto,
        Width = ancho,
        Height = 32,
        FlatStyle = FlatStyle.Flat,
        BackColor = color,
        ForeColor = Color.White,
        Cursor = Cursors.Hand,
        Margin = new Padding(0, 0, 8, 0),
        FlatAppearance = { BorderSize = 0 }
    };

    /// <summary>Aplica el estilo estándar a un DataGridView de solo lectura.</summary>
    public static void EstilizarGrid(DataGridView g)
    {
        g.ReadOnly = true;
        g.AllowUserToAddRows = false;
        g.AllowUserToDeleteRows = false;
        g.AllowUserToResizeRows = false;
        g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        g.RowHeadersVisible = false;
        g.BackgroundColor = Color.White;
        g.BorderStyle = BorderStyle.None;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 34;
        g.ColumnHeadersDefaultCellStyle.BackColor = Oscuro;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
        g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
        g.DefaultCellStyle.SelectionForeColor = Color.Black;
        g.RowTemplate.Height = 28;
    }

    /// <summary>Asigna datos al grid, conserva la fila seleccionada y da formato a fechas y montos.</summary>
    public static void CargarGrid(DataGridView g, object datos)
    {
        int? seleccionado = g.Columns.Contains("Id") ? (g.CurrentRow?.Cells["Id"].Value as int?) : null;
        g.DataSource = datos;

        foreach (DataGridViewColumn col in g.Columns)
        {
            col.HeaderText = Regex.Replace(col.Name, "(?<!^)([A-Z])", " $1").Replace("Categoria", "Categoría");
            var tipo = Nullable.GetUnderlyingType(col.ValueType) ?? col.ValueType;
            if (tipo == typeof(DateTime)) col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            else if (tipo == typeof(decimal))
            {
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (col.Name == "Id") col.FillWeight = 25;
        }

        if (seleccionado is int id && g.Columns.Contains("Id"))
        {
            foreach (DataGridViewRow fila in g.Rows)
            {
                if (fila.Cells["Id"].Value is int v && v == id)
                {
                    fila.Selected = true;
                    g.CurrentCell = fila.Cells[0];
                    break;
                }
            }
        }
    }

    /// <summary>Ejecuta una acción mostrando cualquier error al usuario. Devuelve true si no hubo errores.</summary>
    public static bool Ejecutar(Action accion)
    {
        try
        {
            accion();
            return true;
        }
        catch (ExcepcionAplicacion ex)
        {
            Error(ex.Message);
        }
        catch (Exception ex)
        {
            Error("Ocurrió un error inesperado: " + ex.Message);
        }
        return false;
    }

    public static void Error(string mensaje) =>
        MessageBox.Show(mensaje, "Sistema de Inventario", MessageBoxButtons.OK, MessageBoxIcon.Error);

    public static void Aviso(string mensaje) =>
        MessageBox.Show(mensaje, "Sistema de Inventario", MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static bool Confirmar(string mensaje) =>
        MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

    /// <summary>Convierte texto a decimal; vacío o inválido devuelve null (útil para filtros).</summary>
    public static decimal? LeerDecimalOpcional(string? texto) =>
        decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out var valor) ? valor : null;
}
