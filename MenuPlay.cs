using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SQLiteUtil;

namespace The_Heart
{
    public partial class MenuPlay : Form
    {
        private MenuInicial menuInicial;
        private Random rng = new Random();
        private PlayerData player;
        private MonsterData currentMonster;
        private Queue<int> encounterQueue;

        public MenuPlay()
        {
            InitializeComponent();
            this.btnBackPlay.Click += btnBackPlay_Click;
            InitializeGame();
        }

        public MenuPlay(MenuInicial menuInicial) : this()
        {
            this.menuInicial = menuInicial;
        }

        private class PlayerData
        {
            public int Id;
            public string Name;
            public int TemplateId;
            public int CurrentHP;
            public int MaxHP;
            public int Atk;
            public int Potions;
            public int Level;

            // Aqui lo llame otra vez para asignar estos fatos al ataque y asi ****NO TOCAR******
            public int BaseHP;
            public int BaseATK;
            public int BasePotions;
            public string HeroName;
        }

        private class MonsterData
        {
            public int Uid;
            public string Name;
            public int BaseHP;
            public int BaseATK;
            public int CurrentHP;

            //aqui com todos los jefes son multiplos de 5 puse que si es multiplo de 5 pues es jefe xd si jalo
            public bool IsBoss { get { return (Uid % 5) == 0; } }
        }

        private void InitializeGame()
        {
            try
            {
                if (!LoadPlayerFromDb())
                {
                    MessageBox.Show("No se encontró un jugador en la base de datos. Vuelve al menú inicial y crea uno.");
                    BackToMenuInitial();
                    return;
                }

                BuildEncounterQueue();
                StartNextEncounter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error iniciando la partida: " + ex.Message);
                BackToMenuInitial();
            }
        }

        private bool LoadPlayerFromDb()
        {
            using (var con = SQLiteConnectionExtensions.GetConnection())
            {
                con.Open();

                // Intentamos obtener el último jugador creado y su plantilla
                string sql = @"
SELECT pc.id, pc.player_name, pc.template_id, pc.current_hp, pc.max_hp, pc.atk, pc.potions, pc.level,
       ht.base_hp, ht.base_atk, ht.base_potions, ht.name
FROM player_characters pc
LEFT JOIN hero_templates ht ON pc.template_id = ht.id
ORDER BY pc.id DESC
LIMIT 1;";

                using (var cmd = new SQLiteCommand(sql, con))
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return false;

                    //aqui asigno los valores a el player, 
                    player = new PlayerData
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        TemplateId = reader.GetInt32(2),
                        CurrentHP = reader.GetInt32(3),
                        MaxHP = reader.GetInt32(4),
                        Atk = reader.GetInt32(5),
                        Potions = reader.GetInt32(6),
                        Level = reader.GetInt32(7),
                        BaseHP = reader.IsDBNull(8) ? reader.GetInt32(4) : reader.GetInt32(8),
                        BaseATK = reader.IsDBNull(9) ? reader.GetInt32(5) : reader.GetInt32(9),
                        BasePotions = reader.IsDBNull(10) ? reader.GetInt32(6) : reader.GetInt32(10),
                        HeroName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                    };

                    // Asegurar rangos válidos
                    if (player.MaxHP <= 0) player.MaxHP = Math.Max(1, player.BaseHP);
                    if (player.CurrentHP > player.MaxHP) player.CurrentHP = player.MaxHP;
                }
            }

            UpdateUI();
            return true;
        }

        private void SavePlayerToDb()
        {
            if (player == null) return;

            using (var con = SQLiteConnectionExtensions.GetConnection())
            {
                con.Open();
                string sql = @"
UPDATE player_characters
SET current_hp = @current_hp,
    max_hp = @max_hp,
    potions = @potions,
    level = @level
WHERE id = @id;";
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@current_hp", player.CurrentHP);
                    cmd.Parameters.AddWithValue("@max_hp", player.MaxHP);
                    cmd.Parameters.AddWithValue("@potions", player.Potions);
                    cmd.Parameters.AddWithValue("@level", player.Level);
                    cmd.Parameters.AddWithValue("@id", player.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void BuildEncounterQueue()
        {
            encounterQueue = new Queue<int>();

            // aqui hice en un for que se enfrente a 4 normales y un jefe, asi 5 veces bloques 1-5, 6-10, 11-15, 16-20, 21-25
            for (int block = 0; block < 5; block++)
            {
                int start = block * 5 + 1;
                int boss = start + 4;

                // 4 enemigos en el rango start..start+3 (pueden repetirse)
                for (int i = 0; i < 4; i++)
                {
                    int enemyUid = rng.Next(start, start + 4); // [start, start+3]
                    encounterQueue.Enqueue(enemyUid);
                }

                // luego el jefe
                encounterQueue.Enqueue(boss);
            }
        }

        //aqui es cuando ya derroto a todos y gano yupi
        private void StartNextEncounter()
        {
            if (encounterQueue == null || encounterQueue.Count == 0)
            {
                // El jugador venció a todos
                MessageBox.Show("Has completado el juego. ¡Felicidades!");
                // Dar nivel final porque si y para que en score diga un nivel mas si acabaste
                BackToMenuInitial();
                return;
            }

            int uid = encounterQueue.Dequeue();
            currentMonster = LoadMonsterByUid(uid);

            if (currentMonster == null)
            {
                // Si no se encontró en BD, creamos uno por defecto
                var dao = new PlayDAO();
                var m = dao.GetRandomInRange(1, 4); // enemigo aleatorio entre uids 1..4
                if (m != null)
                {
                    currentMonster = new MonsterData
                    {
                        Uid = m.Id,
                        Name = m.Name,
                        BaseHP = m.BaseHP,
                        BaseATK = m.BaseATK,
                        CurrentHP = m.BaseHP
                    };
                    TxtNombreMounstro.Text = m.Name;
                    UpdateUI();
                }
            }

            // Inicializar la vida
            if (currentMonster.BaseHP <= 0) currentMonster.BaseHP = 1;
            currentMonster.CurrentHP = currentMonster.BaseHP;

            TxtNombreMounstro.Text = currentMonster.Name;
            ShowMonsterImage(currentMonster.Name);
            UpdateUI();
        }

        private MonsterData LoadMonsterByUid(int uid)
        {
            // Buscamos una tabla que contenga 'monster' en su nombre
            string tableName = null;
            using (var con = SQLiteConnectionExtensions.GetConnection())
            {
                con.Open();
                using (var cmd = new SQLiteCommand(
                    "SELECT name FROM sqlite_master WHERE type='table' AND lower(name) LIKE '%monster%' LIMIT 1;", con))
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read()) tableName = r.GetString(0);
                }

                // fallback: buscar 'mob' o 'mounst' por si el nombre es distinto
                if (string.IsNullOrEmpty(tableName))
                {
                    using (var cmd = new SQLiteCommand(
                        "SELECT name FROM sqlite_master WHERE type='table' AND (lower(name) LIKE '%mounst%' OR lower(name) LIKE '%mob%') LIMIT 1;", con))
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read()) tableName = r.GetString(0);
                    }
                }

                if (string.IsNullOrEmpty(tableName))
                {
                    // No hay tabla identificable; devolvemos null para que se cree un monstruo por defecto
                    return null;
                }

                // Obtener columnas de la tabla para mapear nombres comunes
                var columns = new List<string>();
                using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName});", con))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        columns.Add(r.GetString(1).ToLower());
                    }
                }

                string colId = columns.Contains("id") ? "id" : columns.Contains("uid") ? "uid" : columns.FirstOrDefault();
                string colName = columns.Contains("name") ? "name" :
                                 (columns.Contains("monster_name") ? "monster_name" : columns.FirstOrDefault(c => c != colId) ?? colId);
                string colBaseHp = columns.Contains("base_hp") ? "base_hp" :
                                    (columns.Contains("hp") ? "hp" : columns.FirstOrDefault(c => c != colId && c != colName) ?? "base_hp");
                string colBaseAtk = columns.Contains("base_atk") ? "base_atk" :
                                     (columns.Contains("atk") ? "atk" : columns.FirstOrDefault(c => c != colId && c != colName && c != colBaseHp) ?? "base_atk");

                // Aqui consultamos el monstruo por uid
                string sql = $"SELECT {colId}, {colName}, {colBaseHp}, {colBaseAtk} FROM {tableName} WHERE {colId} = @uid LIMIT 1;";
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@uid", uid);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            var m = new MonsterData();
                            // Intentamos leer en orden aleatorio y si falla asignamos valores por defecto
                            try { m.Uid = Convert.ToInt32(r[0]); } catch { m.Uid = uid; }
                            try { m.Name = r.IsDBNull(1) ? "Monstruo " + uid : r.GetString(1); } catch { m.Name = "Monstruo " + uid; }
                            try { m.BaseHP = Convert.ToInt32(r[2]); } catch { m.BaseHP = 50 + uid * 10; }
                            try { m.BaseATK = Convert.ToInt32(r[3]); } catch { m.BaseATK = 5 + uid * 2; }
                            m.CurrentHP = m.BaseHP;
                            return m;
                        }
                    }
                }
            }

            return null;
        }

        private void ShowMonsterImage(string monsterName) // No Jalo se supone que muestra la imagen del monstruo pero dejenlo asi
        {
            try
            {
                string imgPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sprites", monsterName + ".png");
                if (System.IO.File.Exists(imgPath))
                {
                    picMonster.Image = Image.FromFile(imgPath);
                }
                else
                {
                    picMonster.Image = null;
                }
            }
            catch
            {
                picMonster.Image = null;
            }
        }

        private void UpdateUI()
        {
            // Monster HP bar
            if (currentMonster != null)
            {
                HpMounster.Maximum = Math.Max(1, currentMonster.BaseHP);
                int mVal = Math.Max(0, Math.Min(currentMonster.CurrentHP, HpMounster.Maximum));
                HpMounster.Value = mVal;
                TxtNombreMounstro.Text = currentMonster.Name;
            }
            else
            {
                HpMounster.Maximum = 1;
                HpMounster.Value = 0;
                TxtNombreMounstro.Text = "Sin monstruo";
            }

            // Player HP bar
            if (player != null)
            {
                HpPlayer.Maximum = Math.Max(1, player.MaxHP);
                int pVal = Math.Max(0, Math.Min(player.CurrentHP, HpPlayer.Maximum));
                HpPlayer.Value = pVal;
            }
            else
            {
                HpPlayer.Maximum = 1;
                HpPlayer.Value = 0;
            }
        }

        private void BackToMenuInitial()
        {
            // Mostrar el menú inicial y cerrar otras ventanas
            if (menuInicial == null)
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f is MenuInicial)
                    {
                        menuInicial = (MenuInicial)f;
                        break;
                    }
                }
            }

            if (menuInicial != null)
            {
                menuInicial.Show();
            }

            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                var form = Application.OpenForms[i];
                if (form != menuInicial)
                {
                    form.Close();
                }
            }
        }

        private void btnAtacar_Click(object sender, EventArgs e)
        {
            if (!ValidateBattleState()) return;

            int damageToMonster = player.Atk * 2 * player.Level;
            currentMonster.CurrentHP -= damageToMonster;
            if (currentMonster.CurrentHP < 0) currentMonster.CurrentHP = 0;

            UpdateUI();

            if (currentMonster.CurrentHP <= 0)
            {
                OnMonsterDefeated();
                return;
            }

            // Monster contraataca con daño completo
            int monsterDamage = currentMonster.BaseATK;
            player.CurrentHP -= monsterDamage;
            if (player.CurrentHP < 0) player.CurrentHP = 0;

            SavePlayerToDb();
            UpdateUI();
            CheckPlayerDeath();
        }

        private void btnEsquivar_Click(object sender, EventArgs e)
        {
            if (!ValidateBattleState()) return;

            bool success = rng.Next(100) < 50; // pasa la mitad de las vecess
            if (success)
            {
                int damageToMonster = player.Atk * 2 * player.Level;
                currentMonster.CurrentHP -= damageToMonster;
                if (currentMonster.CurrentHP < 0) currentMonster.CurrentHP = 0;
            }
            else
            {
                // Fallas y recibes el ataque completo adfsfagag tremendo noob
                int monsterDamage = currentMonster.BaseATK;
                player.CurrentHP -= monsterDamage;
                if (player.CurrentHP < 0) player.CurrentHP = 0;
            }

            UpdateUI();

            if (currentMonster.CurrentHP <= 0)
            {
                OnMonsterDefeated();
                return;
            }

            SavePlayerToDb();
            CheckPlayerDeath();
        }

        private void btnBloquear_Click(object sender, EventArgs e)
        {
            if (!ValidateBattleState()) return;

            int roll = rng.Next(100);
            int damageToMonster = 0;
            int damageToPlayer = 0;

            if (roll < 75)
            {
                // 3/4 reduces el dano recibido pero tambien ti ataque, a la mitad
                damageToPlayer = currentMonster.BaseATK / 2;
                damageToMonster = player.Atk * player.Level; // mitad del ataque normal, si el normal es ataque por 2 niveles, este es solo por noveles
            }
            else
            {
                // 1/4 no recibe daño y hace ataque doble si roto
                damageToPlayer = 0;
                damageToMonster = player.Atk * 4 * player.Level; // doble de un ataque normal por lo mismo
            }

            //aqui es para que el enemigo sepa que esta uerto y venfa otro
            currentMonster.CurrentHP -= damageToMonster;
            if (currentMonster.CurrentHP < 0) currentMonster.CurrentHP = 0;

            player.CurrentHP -= damageToPlayer;
            if (player.CurrentHP < 0) player.CurrentHP = 0;

            UpdateUI();

            if (currentMonster.CurrentHP <= 0)
            {
                OnMonsterDefeated();
                return;
            }

            SavePlayerToDb();
            CheckPlayerDeath();
        }

        private void btnPociones_Click(object sender, EventArgs e)
        {
            if (!ValidateBattleState()) return;

            if (player.Potions <= 0)
            {
                MessageBox.Show("No tienes pociones.");
                return;
            }

            player.Potions -= 1;
            int heal = 20 * player.Level;
            player.CurrentHP += heal;
            if (player.CurrentHP > player.MaxHP) player.CurrentHP = player.MaxHP;

            // Luego el monstruo ataca
            int monsterDamage = currentMonster.BaseATK;
            player.CurrentHP -= monsterDamage;
            if (player.CurrentHP < 0) player.CurrentHP = 0;

            SavePlayerToDb();
            UpdateUI();
            CheckPlayerDeath();
        }

        private bool ValidateBattleState()
        {
            if (player == null)
            {
                MessageBox.Show("No hay jugador cargado.");
                return false;
            }
            if (currentMonster == null)
            {
                MessageBox.Show("No hay monstruo actual.");
                return false;
            }
            return true;
        }

        private void OnMonsterDefeated()
        {
            bool wasBoss = currentMonster.IsBoss;
            if (wasBoss)
            {
                // Subir de nivel
                player.Level += 1;

                // MaxHP = base_hp * 2 * nivel por conveniencia
                player.MaxHP = Math.Max(1, player.BaseHP * 2 * player.Level);
                player.CurrentHP = player.MaxHP;

                // Restaurar pociones al valor base de la plantilla
                player.Potions = player.BasePotions;

                SavePlayerToDb();
                UpdateUI();

                MessageBox.Show($"Has derrotado al jefe {currentMonster.Name}. Has subido al nivel {player.Level}.");
            }
            else
            {
                MessageBox.Show($"Monstruo {currentMonster.Name} derrotado.");
            }

            // Cuando derrotas al BOSS final se acaba
            if (wasBoss && currentMonster.Uid == 25)
            {
                MessageBox.Show("Has derrotado al jefe final. Fin del juego.");
                // Regresar al menú inicial (podrías guardar score aquí)
                BackToMenuInitial();
                return;
            }

            // Continuar al siguiente encuentro por si quieres seguir jugando
            StartNextEncounter();
        }

        private void CheckPlayerDeath()
        {
            if (player.CurrentHP <= 0)
            {
                MessageBox.Show("Has muerto. Regresas al menú inicial.");
                BackToMenuInitial();
            }
        }

        private void btnBackPlay_Click(object sender, EventArgs e)
        {
            BackToMenuInitial();
        }
        private void progressBar2_Click(object sender, EventArgs e) // doble clikc por error no borrar
        {
        }

        private void HpPlayer_Click(object sender, EventArgs e) //No hace nada pero si lo elimio no compila, ***NO TOCAR***
        {
        }

        private void HpMounster_Click(object sender, EventArgs e) //Lo mismo que el de arriba, ***NO TOCAR***
        {
        }

        private void picMonster_Click(object sender, EventArgs e)//Lo mismo que el de arriba, ***NO TOCAR***
        {
        }
    }
}
