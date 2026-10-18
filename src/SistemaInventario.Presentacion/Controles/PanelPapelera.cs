using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Panel reutilizable de Papelera: lista registros eliminados y permite restaurarlos.</summary>
public class PanelPapelera : UserControl
{
    private readonly Func<string?, object> _cargar;
    private readonly Action<int> _restaurar;
    private readonly TextBox _txtBuscar = new() { PlaceholderText = "Buscar...", Width = 220, Margin = new Padding(0, 3, 12, 0) };
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill };
    private readonly System.Windows.Forms.Timer _retardo = new() { Interval = 250 };

    /// <param name="cargar">Devuelve los registros eliminados (con propiedad Id) filtrados por texto.</param>
    /// <param name="restaurar">Restaura el registro con ese Id.</param>
    public PanelPapelera(Func<string?, object> cargar, Action<int> restaurar)
    {
        _cargar = cargar;
        _restaurar = restaurar;
        Dock = DockStyle.Fill;
        Font = Ui.Fuente;
        Ui.EstilizarGrid(_grid);

        var btnRestaurar = Ui.Boton("Restaurar", Ui.Primario, 100);
        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(8, 8, 8, 0) };
        barra.Controls.AddRange(new Control[] { _txtBuscar, btnRestaurar });
        Controls.Add(_grid);
        Controls.Add(barra);

        _retardo.Tick += (_, _) => { _retardo.Stop(); Recargar(); };
        _txtBuscar.TextChanged += (_, _) => { _retardo.Stop(); _retardo.Start(); };
        btnRestaurar.Click += (_, _) => RestaurarSeleccionado();
        _grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) RestaurarSeleccionado(); };
        Load += (_, _) => Recargar();
    }

    /// <summary>Vuelve a consultar los registros eliminados.</summary>
    public void Recargar() => Ui.Ejecutar(() => Ui.CargarGrid(_grid, _cargar(_txtBuscar.Text)));

    private void RestaurarSeleccionado()
    {
        if (_grid.CurrentRow?.Cells["Id"].Value is not int id) { Ui.Aviso("Seleccione un registro."); return; }
        if (Ui.Ejecutar(() => _restaurar(id)))
        {
            Recargar();
            Ui.Aviso("Registro restaurado correctamente.");
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _retardo.Dispose();
        base.Dispose(disposing);
    }
}
