using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sistema.Negocio;
using Proyecto.Entidades;

namespace Proyecto.Presentacion
{
    public partial class FrmUsuarios : Form
    {
        private bool esNuevo = true;

        public FrmUsuarios()
        {
            InitializeComponent();
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            CargarRoles();
            Listar();
            LimpiarControles();
            ActivarControles(false);
        }

        private void CargarRoles()
        {
            try
            {
                DataTable dt = NRol.Listar();
                cmbRol.DataSource = dt;
                // Ajusta DisplayMember según la columna devuelta por tu SP (comúnmente "Rol" o "NombreRol")
                if (dt.Columns.Contains("Rol")) cmbRol.DisplayMember = "Rol";
                else if (dt.Columns.Contains("NombreRol")) cmbRol.DisplayMember = "NombreRol";
                else if (dt.Columns.Contains("Nombre")) cmbRol.DisplayMember = "Nombre";
                cmbRol.ValueMember = "ID_Rol";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando roles: " + ex.Message);
            }
        }

        private void Listar()
        {
            try
            {
                dgvUsuarios.DataSource = NUsuario.Listar();
                // Ocultar columna de contraseña si existe
                if (dgvUsuarios.Columns.Cast<DataGridViewColumn>().Any(c => c.Name == "Contrasena" || c.HeaderText.ToLower().Contains("contras")))
                {
                    var col = dgvUsuarios.Columns.Cast<DataGridViewColumn>().First(c => c.Name == "Contrasena" || c.HeaderText.ToLower().Contains("contras"));
                    col.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error listando usuarios: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvUsuarios.DataSource = NUsuario.Buscar(txtBuscar.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en búsqueda: " + ex.Message);
            }
        }

        private void LimpiarControles()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtUsuario.Text = "";
            txtContrasena.Text = "";
            if (cmbRol.Items.Count > 0) cmbRol.SelectedIndex = 0;
        }

        private void ActivarControles(bool activo)
        {
            groupBoxDatos.Enabled = activo;
            btnGuardar.Enabled = activo;
            btnCancelar.Enabled = activo;
            btnNuevo.Enabled = !activo;
            btnEditar.Enabled = !activo;
            btnEliminar.Enabled = !activo;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            LimpiarControles();
            ActivarControles(true);
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string usuario = txtUsuario.Text.Trim();
                string contrasena = txtContrasena.Text; // si vacío en actualizar -> no cambiar
                int idRol = cmbRol.SelectedValue != null ? Convert.ToInt32(cmbRol.SelectedValue) : 0;

                if (esNuevo)
                {
                    string r = NUsuario.Insertar(nombre, usuario, contrasena, idRol);
                    MessageBox.Show(r);
                }
                else
                {
                    int id = int.Parse(txtId.Text);
                    string r = NUsuario.Actualizar(id, nombre, usuario, string.IsNullOrWhiteSpace(contrasena) ? null : contrasena, idRol);
                    MessageBox.Show(r);
                }

                Listar();
                LimpiarControles();
                ActivarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando usuario: " + ex.Message);
            }
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            CargarFilaAControles(e.RowIndex);
            esNuevo = false;
            ActivarControles(true);
        }

        private void CargarFilaAControles(int rowIndex)
        {
            var row = dgvUsuarios.Rows[rowIndex];
            if (row.Cells["ID_Usuario"] != null) txtId.Text = row.Cells["ID_Usuario"].Value?.ToString();
            else if (row.Cells["Id"] != null) txtId.Text = row.Cells["Id"].Value?.ToString();

            txtNombre.Text = row.Cells["Nombre"]?.Value?.ToString() ?? "";
            // el SP puede devolver columna "Usuario" o "UsuarioLogin"
            txtUsuario.Text = row.Cells["Usuario"]?.Value?.ToString() ?? row.Cells["UsuarioLogin"]?.Value?.ToString() ?? "";

            // No colocar la contraseña en claro; dejar el campo vacío para que, si se rellena, se actualice.
            txtContrasena.Text = "";

            // Seleccionar rol en combo por valor
            object idRolVal = null;
            if (row.Cells["ID_Rol"] != null) idRolVal = row.Cells["ID_Rol"].Value;
            else if (row.Cells["IDRol"] != null) idRolVal = row.Cells["IDRol"].Value;
            if (idRolVal != null && int.TryParse(idRolVal.ToString(), out int idRol))
            {
                cmbRol.SelectedValue = idRol;
            }
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

            if (MessageBox.Show("¿Eliminar el registro seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = int.Parse(txtId.Text);
                    string r = NUsuario.Eliminar(id);
                    MessageBox.Show(r);
                    Listar();
                    LimpiarControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminando: " + ex.Message);
                }
            }
        }
    }
}
