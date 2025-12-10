using System;
using System.Windows.Forms;

namespace The_Heart
{
    public partial class MenuInicial : Form
    {
        public MenuInicial()
        {
            InitializeComponent();
            // Si quieres, configura aquí estilos adicionales.
        }

        // Botón Jugar -> abrir MenuSeleccion
        private void btnPlay_Click(object sender, EventArgs e)
        {
            var menuSeleccion = new MenuSeleccion();
            menuSeleccion.Show();
            this.Hide();
        }


        // Botón Scores -> abrir MenuScores

        private void btnScores_Click(object sender, EventArgs e)
        {
            var scoresWindow = new MenuScores(this);
            scoresWindow.Show();
            this.Hide();   // o mantener abierto según tu diseño

        }


        // Botón Salir -> cierra la aplicación
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Confirmar salida si el usuario cierra con la X
        private void MenuInicial_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Opcional: preguntar antes de salir
            var res = MessageBox.Show("¿Desea salir del juego?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
