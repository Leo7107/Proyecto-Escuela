using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sistema.Negocio;
using Proyecto.Entidades;

namespace Proyecto.Presentacion
{
    public partial class FrmAsignaturas : Form
    {
        private bool esNuevo = true;

        public FrmAsignaturas()
        {
            InitializeComponent();
        }

        private void FrmAsignaturas_Load(object sender, EventArgs e)
        {
            CargarDocentes();
            Listar();
            LimpiarControles();
            ActivarControles(false);
        }

        private void CargarDocentes()
        {
            try
            {
                DataTable dt = NDocente.Listar();

                // Si el SP devuelve Nombre y Apellido, crear NombreCompleto
                if (dt.Columns.Contains("Nombre") && dt.Columns.Contains("Apellido"))
                {
                    if (!dt.Columns.Contains("NombreCompleto"))
                    {
                        dt.Columns.Add("NombreCompleto", typeof(string));
                        foreach (DataRow r in dt.Rows)
                        {
                            r["NombreCompleto"] = string.Format("{0} {1}", r["Nombre"]?.ToString() ?? "", r["Apellido"]?.ToString() ?? "").Trim();
                        }
                    }
                    cmbDocente.DisplayMember = "NombreCompleto";
                }
                else if (dt.Columns.Contains("Nombre"))
                {
                    cmbDocente.DisplayMember = "Nombre";
                }
                cmbDocente.ValueMember = dt.Columns.Contains("ID_Docente") ? "ID_Docente" : dt.Columns[0].ColumnName;
                cmbDocente.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando docentes: " + ex.Message);
            }
        }

        private void Listar()
        {
            try
            {
                dgvAsignaturas.DataSource = NAsignatura.Listar();

                // ocultar columnas técnicas si existen
                foreach (DataGridViewColumn c in dgvAsignaturas.Columns.Cast<DataGridViewColumn>())
                {
                    if (c.Name.ToLower().Contains("contras") || c.HeaderText.ToLower().Contains("contras")) c.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error listando asignaturas: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvAsignaturas.DataSource = NAsignatura.Buscar(txtBuscar.Text.Trim());
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
            txtDescripcion.Text = "";
            nudCreditos.Value = 3;
            if (cmbDocente.Items.Count > 0) cmbDocente.SelectedIndex = 0;
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
                string descripcion = txtDescripcion.Text.Trim();
                int creditos = Convert.ToInt32(nudCreditos.Value);
                int idDocente = cmbDocente.SelectedValue != null ? Convert.ToInt32(cmbDocente.SelectedValue) : 0;

                if (esNuevo)
                {
                    string r = NAsignatura.Insertar(nombre, descripcion, creditos, idDocente);
                    MessageBox.Show(r);
                }
                else
                {
                    if (!int.TryParse(txtId.Text, out int id)) { MessageBox.Show("Id inválido."); return; }
                    string r = NAsignatura.Actualizar(id, nombre, descripcion, creditos, idDocente);
                    MessageBox.Show(r);
                }

                Listar();
                LimpiarControles();
                ActivarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando asignatura: " + ex.Message);
            }
        }

        private void dgvAsignaturas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            CargarFilaAControles(e.RowIndex);
            esNuevo = false;
            ActivarControles(true);
        }

        private void CargarFilaAControles(int rowIndex)
        {
            var row = dgvAsignaturas.Rows[rowIndex];
            if (row.Cells["ID_Asignatura"] != null) txtId.Text = row.Cells["ID_Asignatura"].Value?.ToString();
            else if (row.Cells["Id"] != null) txtId.Text = row.Cells["Id"].Value?.ToString();

            txtNombre.Text = row.Cells["Nombre"]?.Value?.ToString() ?? row.Cells["NombreAsignatura"]?.Value?.ToString() ?? "";
            txtDescripcion.Text = row.Cells["Descripcion"]?.Value?.ToString() ?? row.Cells["DescripcionAsignatura"]?.Value?.ToString() ?? "";

            object creditosVal = row.Cells["Creditos"]?.Value ?? row.Cells["Créditos"]?.Value;
            if (creditosVal != null && int.TryParse(creditosVal.ToString(), out int creditos)) nudCreditos.Value = Math.Min(Math.Max(creditos, (int)nudCreditos.Minimum), (int)nudCreditos.Maximum);

            object idDocVal = row.Cells["ID_Docente"]?.Value ?? row.Cells["IDDocente"]?.Value;
            if (idDocVal != null && int.TryParse(idDocVal.ToString(), out int idDoc)) cmbDocente.SelectedValue = idDoc;
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

            if (MessageBox.Show("¿Eliminar la asignatura seleccionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = int.Parse(txtId.Text);
                    string r = NAsignatura.Eliminar(id);
                    MessageBox.Show(r);
                    Listar();
                    LimpiarControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminando asignatura: " + ex.Message);
                }
            }
        }
    }
}
