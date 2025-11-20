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
    public partial class FormRegistrarAdmin : Form
    {
        private readonly ApiUsuariosCliente _apiUsuariosCliente;

        public FormRegistrarAdmin()
        {
            InitializeComponent();
            _apiUsuariosCliente = new ApiUsuariosCliente();
            AplicarTemas();
        }

        private void AplicarTemas()
        {
            this.BackColor = Temas.FondoPrincipal;

         
            lblTitulo.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            lblTitulo.ForeColor = Temas.TextoOscuro;

            lblNombreUsuario.ForeColor = Temas.TextoOscuro;
            lblContraseña.ForeColor = Temas.TextoOscuro;

            btnRegistrar.BackColor = Temas.BotonPrimario;
            btnRegistrar.ForeColor = Temas.TextoClaro;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.FlatAppearance.BorderSize = 0;

            btnCancelar.BackColor = Temas.BotonPrimario;  
            btnCancelar.ForeColor = Temas.TextoClaro;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;

            lblError.ForeColor = System.Drawing.Color.Red;
        }

        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            string nombreUsuario = txtNombreUsuario.Text.Trim();
            string contraseña = txtContraseña.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contraseña))
            {
                lblError.Text = "Debe completar nombre de usuario y contraseña.";
                return;
            }

            var dto = new AltaAdminRequestDto
            {
                NombreUsuario = nombreUsuario,
                ContraseñaPlano = contraseña
            };

            try
            {
                btnRegistrar.Enabled = false;

                var adminCreado = await _apiUsuariosCliente.RegistrarAdminAsync(dto);

                MessageBox.Show(
                    $"Admin registrado correctamente.\n\n" +
                    $"Id: {adminCreado.IdUsuario}\n" +
                    $"Nombre: {adminCreado.NombreUsuario}",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Indicamos que todo salió bien → SEÑALAMOS A FormInicio QUE YA HAY UN ADMIN
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            finally
            {
                btnRegistrar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
