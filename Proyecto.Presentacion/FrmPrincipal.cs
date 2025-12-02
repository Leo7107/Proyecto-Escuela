using System;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto.Presentacion
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            IsMdiContainer = true;
            toolStripStatusLabelUser.Text = "Usuario: Invitado";
        }

        private void AbrirFormulario(Type formType)
        {
            // Si ya está abierto lo activamos
            var opened = this.MdiChildren.FirstOrDefault(f => f.GetType() == formType);
            if (opened != null)
            {
                opened.Activate();
                return;
            }

            // Crear instancia y mostrar como MDI child
            Form form = (Form)Activator.CreateInstance(formType);
            form.MdiParent = this;
            form.Show();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmUsuarios));
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmRoles));
        }

        private void estudiantesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmEstudiantes));
        }

        private void docentesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmDocentes));
        }

        private void asignaturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmAsignaturas));
        }

        private void matriculasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmMatriculas));
        }

        private void calificacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmCalificaciones));
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void cerrarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var child in this.MdiChildren) child.Close();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Cerrar todos los formularios abiertos y resetear estado de usuario
            foreach (var child in this.MdiChildren) child.Close();
            toolStripStatusLabelUser.Text = "Usuario: Invitado";
            // Si tienes un Form de login, aquí podrías mostrarlo
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
