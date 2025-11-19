using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartStockClienteWinForms.Dtos.Usuarios;
using SmartStockClienteWinForms.Helpers;

namespace SmartStockClienteWinForms
{
    public partial class FormPanelAdmin : Form
    {
        private readonly AdminResponseDto _admin;

        public FormPanelAdmin(AdminResponseDto admin)
        {
            InitializeComponent();
            _admin = admin;
            AplicarTemas();
            CargarBienvenida();
        }

        private void AplicarTemas()
        {
            this.BackColor = Temas.FondoPrincipal;

            panelMenu.BackColor = Temas.PanelLateral;
            panelPrincipal.BackColor = Temas.FondoPrincipal;

            foreach (Control ctrl in panelMenu.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = Temas.BarraSuperior;
                    btn.ForeColor = Temas.TextoClaro;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                }
            }

        }

        private void CargarBienvenida()
        {
            var lbl = new Label();
            lbl.Text = $"Bienvenido, {_admin.NombreUsuario}";
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lbl.ForeColor = Temas.TextoOscuro;
            lbl.Location = new Point(40, 40);

            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(lbl);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gestión de productos aún no implementada.");
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gestión de usuarios aún no implementada.");
        }
    }
}
