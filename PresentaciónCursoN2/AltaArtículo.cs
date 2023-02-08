using dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Collections;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace PresentaciónCursoN2
{
    public partial class AltaArtículo : Form
    {
        private Articulos articulo = null;
        
        public AltaArtículo()
        {
            InitializeComponent();
            Text = "Agregar Nuevo Artículo";
        }

        public AltaArtículo(Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Artículo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            ArticuloNegocio negocio = new ArticuloNegocio();    
            try
            {
                if (validarCarga())
                {
                    return;
                }

                if(articulo == null)
                {
                    articulo = new Articulos();
                }
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.UrlImagen = txtUrlimagen.Text;
                articulo.Marca = (Marcas)cbMarca.SelectedItem;
                articulo.Categoria = (Categorias)cbCategoria.SelectedItem;


                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {       
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }               
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AltaArtículo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();          
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();               

            try
            {
                cbMarca.DataSource = marcaNegocio.listar();
                cbMarca.ValueMember = "Id";
                cbMarca.DisplayMember = "Descripcion";
                cbCategoria.DataSource = categoriaNegocio.listar();
                cbCategoria.ValueMember = "Id";
                cbCategoria.DisplayMember = "Descripcion";

                cbMarca.SelectedIndex = -1;
                cbCategoria.SelectedIndex = -1;

                if (articulo != null)
                {
                    txtCodigo.Text= articulo.Codigo;
                    txtNombre.Text= articulo.Nombre;
                    txtDescripcion.Text= articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtUrlimagen.Text = articulo.UrlImagen;
                    cargarImagen(articulo.UrlImagen);
                    cbMarca.SelectedValue = articulo.Marca.Id;
                    cbCategoria.SelectedValue = articulo.Categoria.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlimagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlimagen.Text);
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

        private bool validarCarga()
        {
            if(string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text)
                || cbMarca.SelectedIndex == -1 || cbCategoria.SelectedIndex == -1)
            {
                MessageBox.Show("Complete los campos vacíos", "", MessageBoxButtons.OK);
                return true;
            }               
            if(!soloNumeros(txtPrecio.Text))
            {
                MessageBox.Show("Complete el campo de precio solo con números");
                return true;
            }else if(txtPrecio.Text == "")
            {
                MessageBox.Show("Complete los campos vacíos");
                return true;
            }
            else
            {
                return false;
            }
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
                
    }
}
