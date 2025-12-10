using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace The_Heart
{
    public partial class MenuScores : Form
    {
        private MenuInicial menuInicial;
        private ScoreDAO scoreDAO = new ScoreDAO();

        public MenuScores(MenuInicial menuInicial)
        {
            InitializeComponent();

            this.menuInicial = menuInicial;

            // AQUÍ agregas los eventos
            btnBack.Click += btnBack_Click;
            this.Load += MenuScores_Load;
        }

        private void MenuScores_Load(object sender, EventArgs e)
        {
            LoadScores();
        }

        private void LoadScores()
        {
            var scores = scoreDAO.GetTopScores();
            DataGridViewScores.DataSource = scores;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            menuInicial.Show();
            this.Close();
        }
    }
}
