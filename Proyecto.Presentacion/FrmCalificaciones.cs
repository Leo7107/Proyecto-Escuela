using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sistema.Negocio;
using Proyecto.Entidades;

namespace Proyecto.Presentacion
{
    public partial class FrmCalificaciones : Form
    {
        private bool esNuevo = true;

        public FrmCalificaciones()
        {
            InitializeComponent();
        }

        private void FrmCalificaciones_Load(object sender, EventArgs e)
        {
            CargarEstudiantes();
            CargarAsignaturas();
            Listar();
            LimpiarControles();
            ActivarControles(false);
        }

        private void CargarEstudiantes()
        {
            try
            {
                DataTable dt = NEstudiante.Listar();

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
                    cmbEstudiante.DisplayMember = "NombreCompleto";
                }
                else if (dt.Columns.Contains("Nombre"))
                {
                    cmbEstudiante.DisplayMember = "Nombre";
                }

                cmbEstudiante.ValueMember = dt.Columns.Contains("ID_Estudiante") ? "ID_Estudiante" : dt.Columns[0].ColumnName;
                cmbEstudiante.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando estudiantes: " + ex.Message);
            }
        }

        private void CargarAsignaturas()
        {
            try
            {
                DataTable dt = NAsignatura.Listar();
                if (dt.Columns.Contains("Nombre")) cmbAsignatura.DisplayMember = "Nombre";
                cmbAsignatura.ValueMember = dt.Columns.Contains("ID_Asignatura") ? "ID_Asignatura" : dt.Columns[0].ColumnName;
                cmbAsignatura.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando asignaturas: " + ex.Message);
            }
        }

        private void Listar()
        {
            try
            {
                DataTable dt = NCalificacion.Listar();

                // Si el SP devuelve Nota1/2/3 y Promedio, los mostramos.
                if (!dt.Columns.Contains("Notas")) dt.Columns.Add("Notas", typeof(string));
                if (!dt.Columns.Contains("NombreEstudiante"))
                {
                    DataTable dtEst = NEstudiante.Listar();
                    dt.Columns.Add("NombreEstudiante", typeof(string));
                    foreach (DataRow r in dt.Rows)
                    {
                        var idEstObj = r["ID_Estudiante"] ?? r["IDEstudiante"];
                        if (idEstObj != null && int.TryParse(idEstObj.ToString(), out int idEst))
                        {
                            var found = dtEst.AsEnumerable().FirstOrDefault(x => x["ID_Estudiante"].ToString() == idEst.ToString());
                            if (found != null)
                                r["NombreEstudiante"] = (found.Table.Columns.Contains("NombreCompleto") ? found["NombreCompleto"] : (found.Table.Columns.Contains("Nombre") ? found["Nombre"].ToString() + " " + (found.Table.Columns.Contains("Apellido") ? found["Apellido"].ToString() : "") : ""));
                        }

                        // Formar columna Notas: preferimos mostrar Nota1,Nota2,Nota3 si existen
                        if (dt.Columns.Contains("Nota1") && dt.Columns.Contains("Nota2") && dt.Columns.Contains("Nota3"))
                        {
                            r["Notas"] = string.Format("{0},{1},{2}",
                                decimal.TryParse(r["Nota1"]?.ToString(), out decimal a) ? a.ToString("0.00") : "0.00",
                                decimal.TryParse(r["Nota2"]?.ToString(), out decimal b) ? b.ToString("0.00") : "0.00",
                                decimal.TryParse(r["Nota3"]?.ToString(), out decimal c) ? c.ToString("0.00") : "0.00");
                        }
                        else if (r.Table.Columns.Contains("Promedio"))
                        {
                            r["Notas"] = r["Promedio"]?.ToString() ?? "";
                        }
                        else
                        {
                            r["Notas"] = "";
                        }
                    }
                }

                dgvCalificaciones.DataSource = dt;

                // ordenar columnas: ID, NombreEstudiante, ID_Asignatura, Notas, Promedio, Estado
                if (dgvCalificaciones.Columns["ID_Calificacion"] != null) dgvCalificaciones.Columns["ID_Calificacion"].DisplayIndex = 0;
                if (dgvCalificaciones.Columns["NombreEstudiante"] != null) dgvCalificaciones.Columns["NombreEstudiante"].DisplayIndex = 1;
                if (dgvCalificaciones.Columns["ID_Asignatura"] != null) dgvCalificaciones.Columns["ID_Asignatura"].DisplayIndex = 2;
                if (dgvCalificaciones.Columns["Notas"] != null) dgvCalificaciones.Columns["Notas"].DisplayIndex = 3;
                if (dgvCalificaciones.Columns["Promedio"] != null) dgvCalificaciones.Columns["Promedio"].DisplayIndex = 4;
                if (dgvCalificaciones.Columns["Estado"] != null) dgvCalificaciones.Columns["Estado"].DisplayIndex = 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error listando calificaciones: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvCalificaciones.DataSource = NCalificacion.Buscar(txtBuscar.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en búsqueda: " + ex.Message);
            }
        }

        private void LimpiarControles()
        {
            txtIdCalificacion.Text = "";
            if (cmbEstudiante.Items.Count > 0) cmbEstudiante.SelectedIndex = 0;
            if (cmbAsignatura.Items.Count > 0) cmbAsignatura.SelectedIndex = 0;
            nudNota1.Value = 0;
            nudNota2.Value = 0;
            nudNota3.Value = 0;
            cmbEstado.SelectedIndex = 0;
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

        private decimal CalcularPromedio(decimal a, decimal b, decimal c)
        {
            return Math.Round((a + b + c) / 3m, 2);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            LimpiarControles();
            ActivarControles(true);
            cmbEstudiante.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int idEstudiante = cmbEstudiante.SelectedValue != null && int.TryParse(cmbEstudiante.SelectedValue.ToString(), out int ve) ? ve : 0;
                int idAsignatura = cmbAsignatura.SelectedValue != null && int.TryParse(cmbAsignatura.SelectedValue.ToString(), out int va) ? va : 0;
                decimal n1 = nudNota1.Value;
                decimal n2 = nudNota2.Value;
                decimal n3 = nudNota3.Value;

                if (n1 < 0m || n1 > 10m || n2 < 0m || n2 > 10m || n3 < 0m || n3 > 10m)
                {
                    MessageBox.Show("Las notas deben estar entre 0 y 10.");
                    return;
                }

                bool estado = (cmbEstado.SelectedItem?.ToString() ?? "Activo") == "Activo";

                if (esNuevo)
                {
                    string r = NCalificacion.Insertar(idEstudiante, idAsignatura, n1, n2, n3, estado);
                    MessageBox.Show(r);
                }
                else
                {
                    if (!int.TryParse(txtIdCalificacion.Text, out int id))
                    {
                        MessageBox.Show("Id inválido.");
                        return;
                    }

                    string r = NCalificacion.Actualizar(id, idEstudiante, idAsignatura, n1, n2, n3, estado);
                    MessageBox.Show(r);
                }


                Listar();
                LimpiarControles();
                ActivarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando calificación: " + ex.Message);
            }
        }

        private void dgvCalificaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            CargarFilaAControles(e.RowIndex);
            esNuevo = false;
            ActivarControles(true);
        }

        private void CargarFilaAControles(int rowIndex)
        {
            var row = dgvCalificaciones.Rows[rowIndex];
            if (row.Cells["ID_Calificacion"] != null) txtIdCalificacion.Text = row.Cells["ID_Calificacion"].Value?.ToString();
            else if (row.Cells["Id"] != null) txtIdCalificacion.Text = row.Cells["Id"].Value?.ToString();

            object idEstVal = row.Cells["ID_Estudiante"]?.Value ?? row.Cells["IDEstudiante"]?.Value;
            if (idEstVal != null && int.TryParse(idEstVal.ToString(), out int idEst)) cmbEstudiante.SelectedValue = idEst;

            object idAsigVal = row.Cells["ID_Asignatura"]?.Value ?? row.Cells["IDAsignatura"]?.Value;
            if (idAsigVal != null && int.TryParse(idAsigVal.ToString(), out int idAsig)) cmbAsignatura.SelectedValue = idAsig;

            // Si el SP devuelve Nota1/2/3 muéstralas; si no, muestra la Nota (promedio) en nudNota1 y deja las otras a 0
            if (dgvCalificaciones.DataSource is DataTable dt && dt.Columns.Contains("Nota1") && dt.Columns.Contains("Nota2") && dt.Columns.Contains("Nota3"))
            {
                nudNota1.Value = row.Cells["Nota1"].Value is DBNull ? 0 : Convert.ToDecimal(row.Cells["Nota1"].Value);
                nudNota2.Value = row.Cells["Nota2"].Value is DBNull ? 0 : Convert.ToDecimal(row.Cells["Nota2"].Value);
                nudNota3.Value = row.Cells["Nota3"].Value is DBNull ? 0 : Convert.ToDecimal(row.Cells["Nota3"].Value);

            }
            else
            {
                var notaVal = row.Cells["Nota"]?.Value;
                if (notaVal != null && decimal.TryParse(notaVal.ToString(), out decimal nota))
                {
                    nudNota1.Value = Math.Min(Math.Max(nota, nudNota1.Minimum), nudNota1.Maximum);
                    nudNota2.Value = 0;
                    nudNota3.Value = 0;
                }
            }

            // Estado: puede ser bit/int/booleano
            var estadoObj = row.Cells["Estado"]?.Value;
            if (estadoObj != null)
            {
                string s = estadoObj.ToString();
                if (s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase)) cmbEstado.SelectedItem = "Activo";
                else cmbEstado.SelectedItem = "Inactivo";
            }
            else
            {
                cmbEstado.SelectedIndex = 0;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdCalificacion.Text))
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
            if (string.IsNullOrWhiteSpace(txtIdCalificacion.Text))
            {
                MessageBox.Show("Seleccione un registro para eliminar (doble clic en la grilla).");
                return;
            }

            if (MessageBox.Show("¿Eliminar la calificación seleccionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = int.Parse(txtIdCalificacion.Text);
                    string r = NCalificacion.Eliminar(id);
                    MessageBox.Show(r);
                    Listar();
                    LimpiarControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminando calificación: " + ex.Message);
                }
            }
        }

        private void lblNota_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
