namespace SistemaInventario.Presentacion;

/// <summary>Colores, fuentes y controles con el estilo pastel de toda la aplicación.</summary>
public static class Estilo
{
    // Verdes pasteles (fondos)
    public static readonly Color Fondo = Color.FromArgb(232, 245, 233);
    public static readonly Color Encabezado = Color.FromArgb(178, 223, 191);
    public static readonly Color Seleccion = Color.FromArgb(200, 235, 210);
    public static readonly Color Texto = Color.FromArgb(31, 61, 43);

    // Colores fuertes para los botones (hacen contraste con el verde pastel)
    public static readonly Color Primario = Color.FromArgb(45, 106, 79);
    public static readonly Color Secundario = Color.FromArgb(74, 111, 165);
    public static readonly Color Acento = Color.FromArgb(244, 162, 97);
    public static readonly Color Peligro = Color.FromArgb(211, 93, 110);
    public static readonly Color Lila = Color.FromArgb(124, 106, 170);

    public static readonly Font Fuente = new("Segoe UI", 10F);
    public static readonly Font FuenteNegrita = new("Segoe UI", 10F, FontStyle.Bold);

    /// <summary>Crea un botón plano de color con texto que contrasta.</summary>
    public static Button Boton(string texto, Color color, int ancho = 110)
    {
        var b = new Button
        {
            Text = texto, Width = ancho, Height = 36, BackColor = color,
            ForeColor = color.GetBrightness() > 0.6f ? Texto : Color.White,
            FlatStyle = FlatStyle.Flat, Font = FuenteNegrita, Cursor = Cursors.Hand,
            Margin = new Padding(6, 0, 0, 0), UseVisualStyleBackColor = false
        };
        b.FlatAppearance.BorderSize = 0;
        return b;
    }

    /// <summary>Etiqueta alineada para barras de herramientas.</summary>
    public static Label Etiqueta(string texto) => new()
    {
        Text = texto, AutoSize = true, ForeColor = Texto, Font = FuenteNegrita, Margin = new Padding(12, 9, 3, 0)
    };

    /// <summary>Tabla de solo lectura con colores pasteles.</summary>
    public static DataGridView Grid()
    {
        var g = new DataGridView
        {
            Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
            AllowUserToDeleteRows = false, AllowUserToResizeRows = false, MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false, BorderStyle = BorderStyle.None, BackgroundColor = Fondo,
            EnableHeadersVisualStyles = false, Font = Fuente,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersHeight = 38
        };
        g.ColumnHeadersDefaultCellStyle.BackColor = Primario;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        g.ColumnHeadersDefaultCellStyle.Font = FuenteNegrita;
        g.DefaultCellStyle.BackColor = Color.White;
        g.DefaultCellStyle.ForeColor = Texto;
        g.DefaultCellStyle.SelectionBackColor = Seleccion;
        g.DefaultCellStyle.SelectionForeColor = Texto;
        g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(243, 250, 245);
        return g;
    }

    public static void Aviso(string mensaje) =>
        MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Error(string mensaje) =>
        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

    public static bool Confirmar(string mensaje) =>
        MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
}
