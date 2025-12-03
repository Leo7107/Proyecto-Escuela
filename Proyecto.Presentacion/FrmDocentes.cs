using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sistema.Negocio;
using Proyecto.Entidades;

namespace Proyecto.Presentacion
{
    public partial class FrmDocentes : Form
    {
        private bool esNuevo = true;

        public FrmDocentes()
        {
            InitializeComponent();
        }

        private void FrmDocentes_Load(object sender, EventArgs e)
        {
            Listar();
            LimpiarControles();
            ActivarControles(false);
        }

        private void Listar()
        {
            try
            {
                dgvDocentes.DataSource = NDocente.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error listando docentes: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDocentes.DataSource = NDocente.Buscar(txtBuscar.Text.Trim());
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
            txtApellido.Text = "";
            txtDocumento.Text = "";
            txtEspecialidad.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
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
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim();
                string documento = txtDocumento.Text.Trim();
                string especialidad = txtEspecialidad.Text.Trim();
                string telefono = txtTelefono.Text.Trim();
                string correo = txtCorreo.Text.Trim();

                if (esNuevo)
                {
                    string r = NDocente.Insertar(nombre, apellido, documento, especialidad, telefono, correo);
                    MessageBox.Show(r);
                }
                else
                {
                    if (!int.TryParse(txtId.Text, out int id)) { MessageBox.Show("Id inválido."); return; }
                    string r = NDocente.Actualizar(id, nombre, apellido, documento, especialidad, telefono, correo);
                    MessageBox.Show(r);
                }

                Listar();
                LimpiarControles();
                ActivarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando docente: " + ex.Message);
            }
        }

        private void dgvDocentes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            CargarFilaAControles(e.RowIndex);
            esNuevo = false;
            ActivarControles(true);
        }

        private void CargarFilaAControles(int rowIndex)
        {
            var row = dgvDocentes.Rows[rowIndex];
            if (row.Cells["ID_Docente"] != null) txtId.Text = row.Cells["ID_Docente"].Value?.ToString();
            else if (row.Cells["Id"] != null) txtId.Text = row.Cells["Id"].Value?.ToString();

            txtNombre.Text = row.Cells["Nombre"]?.Value?.ToString() ?? "";
            txtApellido.Text = row.Cells["Apellido"]?.Value?.ToString() ?? "";
            txtDocumento.Text = row.Cells["Documento"]?.Value?.ToString() ?? "";
            txtEspecialidad.Text = row.Cells["Especialidad"]?.Value?.ToString() ?? "";
            txtTelefono.Text = row.Cells["Telefono"]?.Value?.ToString() ?? "";
            txtCorreo.Text = row.Cells["Correo"]?.Value?.ToString() ?? "";
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

            if (MessageBox.Show("¿Eliminar el docente seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = int.Parse(txtId.Text);
                    string r = NDocente.Eliminar(id);
                    MessageBox.Show(r);
                    Listar();
                    LimpiarControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminando docente: " + ex.Message);
                }
            }
        }
    }
}
