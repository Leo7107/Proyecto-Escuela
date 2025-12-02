using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sistema.Negocio;
using Proyecto.Entidades;

namespace Proyecto.Presentacion
{
    public partial class FrmMatriculas : Form
    {
        private bool esNuevo = true;

        public FrmMatriculas()
        {
            InitializeComponent();
        }

        private void FrmMatriculas_Load(object sender, EventArgs e)
        {
            CargarEstudiantes();
            CargarAsignaturas();
            Listar();
            LimpiarControles();
            ActivarControles(false);

            // Ajustar valores por defecto
            nudAnio.Value = DateTime.Now.Year;
        }

        private void CargarEstudiantes()
        {
            try
            {
                DataTable dt = NEstudiante.Listar();

                // Si el SP devuelve Nombre + Apellido, crear columna NombreCompleto
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
                if (dt.Columns.Contains("ID_Docente")) { }
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
                DataTable dt = NMatricula.Listar();

                // Añadir NombreEstudiante y NombreDocente para mejor UX 
                DataTable dtEst = NEstudiante.Listar();
                DataTable dtAsig = NAsignatura.Listar();
                DataTable dtDoc = NDocente.Listar();

                if (!dt.Columns.Contains("NombreEstudiante")) dt.Columns.Add("NombreEstudiante", typeof(string));
                if (!dt.Columns.Contains("NombreDocente")) dt.Columns.Add("NombreDocente", typeof(string));

                foreach (DataRow r in dt.Rows)
                {
                    var idEstObj = r["ID_Estudiante"] ?? r["IDEstudiante"];
                    if (idEstObj != null && int.TryParse(idEstObj.ToString(), out int idEst))
                    {
                        var foundEst = dtEst.AsEnumerable().FirstOrDefault(x => x["ID_Estudiante"].ToString() == idEst.ToString());
                        if (foundEst != null)
                        {
                            r["NombreEstudiante"] = foundEst.Table.Columns.Contains("NombreCompleto")
                                ? foundEst["NombreCompleto"]
                                : (foundEst.Table.Columns.Contains("Nombre") ? foundEst["Nombre"].ToString() + " " + (foundEst.Table.Columns.Contains("Apellido") ? foundEst["Apellido"].ToString() : "") : "");
                        }
                    }

                    var idAsigObj = r["ID_Asignatura"] ?? r["IDAsignatura"];
                    if (idAsigObj != null && int.TryParse(idAsigObj.ToString(), out int idAsig))
                    {
                        var foundAsig = dtAsig.AsEnumerable().FirstOrDefault(x => x["ID_Asignatura"].ToString() == idAsig.ToString());
                        if (foundAsig != null)
                        {
                            // buscar docente por ID_Docente en la tabla Asignaturas y luego en dtDoc
                            if (foundAsig.Table.Columns.Contains("ID_Docente") && int.TryParse(foundAsig["ID_Docente"]?.ToString(), out int idDoc))
                            {
                                var foundDoc = dtDoc.AsEnumerable().FirstOrDefault(d => d["ID_Docente"].ToString() == idDoc.ToString());
                                if (foundDoc != null)
                                {
                                    r["NombreDocente"] = foundDoc.Table.Columns.Contains("Nombre") ? (foundDoc["Nombre"].ToString() + " " + (foundDoc.Table.Columns.Contains("Apellido") ? foundDoc["Apellido"].ToString() : "")) : foundDoc[0].ToString();
                                }
                            }
                        }
                    }
                }

                dgvMatriculas.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error listando matrículas: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvMatriculas.DataSource = NMatricula.Buscar(txtBuscar.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en búsqueda: " + ex.Message);
            }
        }

        private void LimpiarControles()
        {
            txtIdMatricula.Text = "";
            if (cmbEstudiante.Items.Count > 0) cmbEstudiante.SelectedIndex = 0;
            if (cmbAsignatura.Items.Count > 0) cmbAsignatura.SelectedIndex = 0;
            nudAnio.Value = DateTime.Now.Year;
            nudGrado.Value = 1;
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

        // Helper para obtener int del SelectedValue (DataRowView / string / int)
        private int GetSelectedId(ComboBox cb)
        {
            if (cb.SelectedValue == null) return 0;
            var sv = cb.SelectedValue;
            if (sv is DataRowView drv)
            {
                var firstCol = drv.Row.Table.Columns.Contains("ID_Estudiante") ? "ID_Estudiante" : drv.Row.Table.Columns[0].ColumnName;
                if (int.TryParse(drv[firstCol]?.ToString(), out int v)) return v;
            }
            else
            {
                if (int.TryParse(sv.ToString(), out int v)) return v;
            }
            return 0;
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
                int idEstudiante = GetSelectedId(cmbEstudiante);
                int idAsignatura = GetSelectedId(cmbAsignatura);
                int anio = Convert.ToInt32(nudAnio.Value);
                int grado = Convert.ToInt32(nudGrado.Value);

                if (idEstudiante <= 0) { MessageBox.Show("Seleccione un estudiante válido."); return; }
                if (idAsignatura <= 0) { MessageBox.Show("Seleccione una asignatura válida."); return; }

                if (esNuevo)
                {
                    string r = NMatricula.Insertar(idEstudiante, idAsignatura, anio, grado);
                    MessageBox.Show(r);
                }
                else
                {
                    if (!int.TryParse(txtIdMatricula.Text, out int id)) { MessageBox.Show("Id inválido."); return; }
                    string r = NMatricula.Actualizar(id, idEstudiante, idAsignatura, anio, grado);
                    MessageBox.Show(r);
                }

                Listar();
                LimpiarControles();
                ActivarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando matrícula: " + ex.Message);
            }
        }

        private void dgvMatriculas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            CargarFilaAControles(e.RowIndex);
            esNuevo = false;
            ActivarControles(true);
        }

        private void CargarFilaAControles(int rowIndex)
        {
            var row = dgvMatriculas.Rows[rowIndex];
            if (row.Cells["ID_Matricula"] != null) txtIdMatricula.Text = row.Cells["ID_Matricula"].Value?.ToString();
            else if (row.Cells["Id"] != null) txtIdMatricula.Text = row.Cells["Id"].Value?.ToString();

            object idEstVal = row.Cells["ID_Estudiante"]?.Value ?? row.Cells["IDEstudiante"]?.Value;
            if (idEstVal != null && int.TryParse(idEstVal.ToString(), out int idEst)) cmbEstudiante.SelectedValue = idEst;

            object idAsigVal = row.Cells["ID_Asignatura"]?.Value ?? row.Cells["IDAsignatura"]?.Value;
            if (idAsigVal != null && int.TryParse(idAsigVal.ToString(), out int idAsig)) cmbAsignatura.SelectedValue = idAsig;

            object anioVal = row.Cells["Año"]?.Value ?? row.Cells["Anio"]?.Value ?? row.Cells["Year"]?.Value;
            if (anioVal != null && int.TryParse(anioVal.ToString(), out int anio)) nudAnio.Value = Math.Min(Math.Max(anio, nudAnio.Minimum), nudAnio.Maximum);

            object gradoVal = row.Cells["Grado"]?.Value?.ToString();
            if (gradoVal != null && int.TryParse(gradoVal.ToString(), out int grado)) nudGrado.Value = Math.Min(Math.Max(grado, (int)nudGrado.Minimum), (int)nudGrado.Maximum);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdMatricula.Text))
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
            if (string.IsNullOrWhiteSpace(txtIdMatricula.Text))
            {
                MessageBox.Show("Seleccione un registro para eliminar (doble clic en la grilla).");
                return;
            }

            if (MessageBox.Show("¿Eliminar la matrícula seleccionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = int.Parse(txtIdMatricula.Text);
                    string r = NMatricula.Eliminar(id);
                    MessageBox.Show(r);
                    Listar();
                    LimpiarControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminando matrícula: " + ex.Message);
                }
            }
        }
    }
}
