using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Heart
{
    public partial class MenuSeleccion : Form
    {
        private PlayerCharacterDAO playerDAO;
        private HeroTemplate selectedHero;
        private BindingSource heroBindingSource = new BindingSource();
        private HeroTemplateDAO heroDAO = new HeroTemplateDAO();
        private MenuInicial menuInicial;

        public MenuSeleccion()
        {
            InitializeComponent();

            this.dataGridViewHeroes.CellClick += dataGridViewHeroes_CellClick;

            heroDAO = new HeroTemplateDAO();
            heroBindingSource.DataSource = heroDAO.GetAll();

            dataGridViewHeroes.DataSource = heroBindingSource;

            playerDAO = new PlayerCharacterDAO();
        }

        // Constructor sobrecargado para recibir la referencia al MenuInicial
        public MenuSeleccion(MenuInicial menuInicial) : this()
        {
            this.menuInicial = menuInicial;
        }

        private void LoadHeroes()
        {
            var heroes = heroDAO.GetAll();
            heroBindingSource.DataSource = heroes;
            dataGridViewHeroes.DataSource = heroBindingSource;
        }

        private void dataGridViewHeroes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedHero = (HeroTemplate)heroBindingSource[e.RowIndex];

                MessageBox.Show($"Seleccionaste: {selectedHero.Name}");

                ShowHeroImage(selectedHero.Name);
            }
        }

        private void ShowHeroImage(string heroName)
        {
            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sprites", heroName + ".png");

            if (File.Exists(imgPath))
            {
                picHero.Image = Image.FromFile(imgPath);
            }
            else
            {
                picHero.Image = null;
                MessageBox.Show("No se encontró la imagen para " + heroName);
            }
        }

        private void btnSelectHero_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Escribe un nombre antes de continuar.");
                return;
            }

            if (selectedHero == null)
            {
                MessageBox.Show("Selecciona un héroe en la tabla.");
                return;
            }

            playerDAO.CreatePlayer(txtPlayerName.Text, selectedHero);

            MessageBox.Show($"Jugador {txtPlayerName.Text} ha elegido {selectedHero.Name}.");

            // Abrir MenuPlay y ocultar este menú de selección
            var playWindow = new MenuPlay(menuInicial);
            playWindow.Show();
            this.Hide();
        }

    }
}