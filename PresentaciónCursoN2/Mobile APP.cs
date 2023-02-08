using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;


namespace PresentaciónCursoN2
{
    public partial class Mobile : Form
    {
        private List<Articulos> listaArticulos;
        public Mobile()
        {
            InitializeComponent();
        }

        private void Mobile_Load(object sender, EventArgs e)
        {
            cargar();
            cbCampo.Items.Add("Nombre");
            cbCampo.Items.Add("Precio");
            cbCampo.Items.Add("Marca");
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbUrlimagen.Load(imagen);
            }
            catch (Exception)
            {

                pbUrlimagen.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/681px-Placeholder_view_vector.svg.png");
            }           
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaArtículo alta = new AltaArtículo();
            alta.ShowDialog();
            cargar();
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulos = negocio.listar();
                dgvArticulos.DataSource = listaArticulos;    
                ocultar();
                cargarImagen(listaArticulos[0].UrlImagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultar()
        {
            dgvArticulos.Columns["UrlImagen"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

            AltaArtículo modificar = new AltaArtículo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulos seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("Confirmar eliminación", "Eliminando...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if(respuesta == DialogResult.OK)
                {
                    seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtBusquedaRapida_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> listaFiltrada;
            string buscar = txtBusquedaRapida.Text;

            if (buscar != "")
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToLower().Contains(buscar.ToLower()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultar();
        }

        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbCampo.SelectedItem.ToString();

            if(opcion == "Nombre" || opcion == "Marca")
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Empieza con");
                cbCriterio.Items.Add("Contiene");
                cbCriterio.Items.Add("Termina con");
                
            }
            else
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Menor a");
                cbCriterio.Items.Add("Igual a");
                cbCriterio.Items.Add("Mayor a");
            }

            
        }

        private bool validarBusqueda()
        {
            if(cbCampo.SelectedIndex == -1 || cbCriterio.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el campo y el criterio de búsqueda", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            if (cbCampo.SelectedItem.ToString() == "Precio")
            {
                if (!soloNumeros(txtBusquedaAvanzada.Text))
                {
                    MessageBox.Show("Ingrese solo nros para buscar por precio");
                    return true;
                }else if (txtBusquedaAvanzada.Text == "")
                {
                    DialogResult mensaje = MessageBox.Show("Ingrese un precio para realizar la búsqueda");
                    return true;
                }
            }
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnBusquedaAvanzada_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarBusqueda())
                {
                    return;
                }
                string campo = cbCampo.SelectedItem.ToString();
                string criterio = cbCriterio.SelectedItem.ToString();
                string filtro = txtBusquedaAvanzada.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
            
        }
    }
}
