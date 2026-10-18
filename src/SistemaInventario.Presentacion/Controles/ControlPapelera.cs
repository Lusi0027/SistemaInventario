using SistemaInventario.LogicaNegocio;

namespace SistemaInventario.Presentacion;

/// <summary>Pantalla "Papelera": registros eliminados de categorías, suplidores y productos.</summary>
public class ControlPapelera : UserControl
{
    private readonly List<PanelPapelera> _paneles = new();
    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill, Padding = new Point(16, 6) };

    public ControlPapelera(ContenedorServicios servicios)
    {
        Dock = DockStyle.Fill;
        BackColor = Color.White;
        Font = Utilidades.Ui.Fuente;

        AgregarPestana("Categorías", new PanelPapelera(
            t => servicios.Categorias.ListarEliminadas(t).Select(c => new
            {
                c.Id, c.Nombre, Descripción = c.Descripcion, Modificación = c.FechaModificacion
            }).ToList(),
            servicios.Categorias.Restaurar));

        AgregarPestana("Suplidores", new PanelPapelera(
            t => servicios.Suplidores.ListarEliminados(t).Select(s => new
            {
                s.Id, Empresa = s.NombreEmpresa, Contacto = s.NombreContacto, Correo = s.Correo, Modificación = s.FechaModificacion
            }).ToList(),
            servicios.Suplidores.Restaurar));

        AgregarPestana("Productos", new PanelPapelera(
            t => servicios.Productos.ListarEliminados(t).Select(p => new
            {
                p.Id, p.Nombre, Precio = p.PrecioUnitario, Categoría = p.CategoriaNombre,
                Suplidor = p.SuplidorNombre, Modificación = p.FechaModificacion
            }).ToList(),
            servicios.Productos.Restaurar));

        var titulo = new Label
        {
            Text = "Papelera",
            Dock = DockStyle.Top,
            Height = 52,
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            Padding = new Padding(16, 12, 0, 0)
        };
        Controls.Add(_tabs);
        Controls.Add(titulo);

        // Al cambiar de pestaña se refresca (p. ej. tras restaurar una categoría padre).
        _tabs.SelectedIndexChanged += (_, _) =>
        {
            if (_tabs.SelectedIndex >= 0) _paneles[_tabs.SelectedIndex].Recargar();
        };
    }

    private void AgregarPestana(string titulo, PanelPapelera panel)
    {
        var tab = new TabPage(titulo);
        tab.Controls.Add(panel);
        _tabs.TabPages.Add(tab);
        _paneles.Add(panel);
    }
}
