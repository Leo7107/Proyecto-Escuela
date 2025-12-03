using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sistema.Negocio;
using Proyecto.Entidades;

namespace Proyecto.Presentacion
{
    public partial class FrmRoles : Form
    {
        private bool esNuevo = true;

        public FrmRoles()
        {
            InitializeComponent();
        }

        private void FrmRoles_Load(object sender, EventArgs e)
        {
            Listar();
            LimpiarControles();
            ActivarControles(false);
        }

        private void Listar()
        {
            try
            {
                dgvRoles.DataSource = NRol.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error listando roles: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvRoles.DataSource = NRol.Buscar(txtBuscar.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en búsqueda: " + ex.Message);
            }
        }

        private void LimpiarControles()
        {
            txtId.Text = "";
            txtRol.Text = "";
        }

        private void ActivarControles(bool activo)
        {
            groupBoxDatos.Enabled = activo;
            btnGuardar.Enabled = activo;
            btnCancelar.Enabled = activo;
            btnNuevo.Enabled = !activo;
            //btnEditar.Enabled = !activo;
            btnEliminar.Enabled = activo;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            LimpiarControles();
            ActivarControles(true);
            txtRol.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreRol = txtRol.Text.Trim();

                if (esNuevo)
                {
                    string r = NRol.Insertar(nombreRol);
                    MessageBox.Show(r);
                }
                else
                {
                    if (!int.TryParse(txtId.Text, out int id)) { MessageBox.Show("Id inválido."); return; }
                    string r = NRol.Actualizar(id, nombreRol);
                    MessageBox.Show(r);
                }

                Listar();
                LimpiarControles();
                ActivarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando rol: " + ex.Message);
            }
        }

        private void dgvRoles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            CargarFilaAControles(e.RowIndex);
            esNuevo = false;
            ActivarControles(true);
        }

        private void CargarFilaAControles(int rowIndex)
        {
            var row = dgvRoles.Rows[rowIndex];
            if (row.Cells["ID_Rol"] != null) txtId.Text = row.Cells["ID_Rol"].Value?.ToString();
            else if (row.Cells["Id"] != null) txtId.Text = row.Cells["Id"].Value?.ToString();

            txtRol.Text = row.Cells["NombreRol"]?.Value?.ToString() ?? row.Cells["Rol"]?.Value?.ToString() ?? row.Cells["Nombre"]?.Value?.ToString() ?? "";
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un registro para editar (doble clic en la grilla).");
                return;
            }
            esNuevo = false;
            ActivarControles(true);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            ActivarControles(false);
            esNuevo = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un registro para eliminar (doble clic en la grilla).");
                return;
            }

            if (MessageBox.Show("¿Eliminar el rol seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = int.Parse(txtId.Text);
                    string r = NRol.Eliminar(id);
                    MessageBox.Show(r);
                    Listar();
                    LimpiarControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminando rol: " + ex.Message);
                }
            }
        }
    }
}
