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
    public partial class FormLoginAdmin : Form
    {
        private readonly ApiAutenticacionCliente _apiAuth;

        public FormLoginAdmin()
        {
            InitializeComponent();
            _apiAuth = new ApiAutenticacionCliente();
            AplicarTemas();
        }

        private void AplicarTemas()
        {
            this.BackColor = Temas.FondoPrincipal;

            lblTitulo.ForeColor = Temas.TextoOscuro;
            lblUsuario.ForeColor = Temas.TextoOscuro;
            lblContraseña.ForeColor = Temas.TextoOscuro;

            btnIngresar.BackColor = Temas.BotonPrimario;
            btnIngresar.ForeColor = Temas.TextoClaro;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.FlatAppearance.BorderSize = 0;

            btnCancelar.BackColor = Temas.BotonSecundario;
            btnCancelar.ForeColor = Temas.TextoClaro;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;


        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            string usuario = txtUsuario.Text.Trim();
            string contraseña = txtContraseña.Text.Trim();

            if (usuario == "" || contraseña == "")
            {
                lblError.Text = "Debe completar usuario y contraseña.";
                return;
            }

            try
            {
                var dto = new LoginAdminRequestDto
                {
                    NombreAdmin = usuario,
                    ContraseñaPlano = contraseña
                };

                var admin = await _apiAuth.LoginAdminAsync(dto);

                // Login OK , abrir panel admin
                this.Hide();
                var panelAdmin = new FormPanelAdmin(admin);
                panelAdmin.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
