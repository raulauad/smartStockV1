using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartStockClienteWinFormsNet8.Dtos.Usuarios;
using SmartStockClienteWinFormsNet8.Servicios;
using SmartStockClienteWinFormsNet8.Helpers;

namespace SmartStockClienteWinFormsNet8
{
    public partial class FormInicio : Form
    {
        // Indicador en memoria de si ya existe un admin registrado.
        private bool _adminRegistrado = false;

        public FormInicio()
        {
            InitializeComponent();
            AplicarTemas();
            ActualizarMensajeAdmin();
        }

        private void AplicarTemas()
        {
            this.BackColor = Temas.FondoPrincipal;

            panelLateral.BackColor = Temas.PanelLateral;
            panelCentral.BackColor = Temas.FondoPrincipal;

            //  Botón Admin 
            btnAdmin.BackColor = Temas.BotonPrimario;
            btnAdmin.ForeColor = Temas.TextoClaro;
            btnAdmin.FlatStyle = FlatStyle.Flat;
            btnAdmin.FlatAppearance.BorderSize = 0;

            //  Botón Usuario 
            btnUsuario.BackColor = Temas.BotonPrimario;
            btnUsuario.ForeColor = Temas.TextoClaro;
            btnUsuario.FlatStyle = FlatStyle.Flat;
            btnUsuario.FlatAppearance.BorderSize = 0;

            //  Botón Registrar Admin 
            btnRegistrarAdmin.BackColor = Temas.BotonPrimario;
            btnRegistrarAdmin.ForeColor = Temas.TextoClaro;
            btnRegistrarAdmin.FlatStyle = FlatStyle.Flat;
            btnRegistrarAdmin.FlatAppearance.BorderSize = 0;

            // AGREGA EL ESTILO AL TÍTULO
            lblTitulo.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblTitulo.ForeColor = Temas.TextoOscuro;

            lblMensajeAdmin.ForeColor = Temas.TextoOscuro;

          
        
        }

        private void ActualizarMensajeAdmin()
        {
            // Mostramos el mensaje SOLO si NO existe admin todavía
            lblMensajeAdmin.Visible = !_adminRegistrado;
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            if (!_adminRegistrado)
            {
                var respuesta = MessageBox.Show(
                    "Todavía no hay ningún administrador registrado.\n\n" +
                    "¿Desea registrar un administrador ahora?",
                    "Administrador no encontrado",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    btnRegistrarAdmin_Click(sender, e);
                }

                return;
            }

            // Si ya existe admin , abrir login admin
            using (var login = new FormLoginAdmin())
            {
                login.ShowDialog();
            }
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Pantalla de usuario aún no implementada.",
                "Información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnRegistrarAdmin_Click(object sender, EventArgs e)
        {
            using (var formRegistrar = new FormRegistrarAdmin())
            {
                var resultado = formRegistrar.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    _adminRegistrado = true;
                    ActualizarMensajeAdmin();
                }
            }
        }

      
    }
}
