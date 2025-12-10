using System;
using System.Collections.Generic;
using System.Data.SQLite;   // ← importante: System.Data.SQLite
using SQLiteUtil;           // ← si tu SQLiteConnectionExtensions está en este namespace

public class PlayerCharacterDAO
{
    public void CreatePlayer(string playerName, HeroTemplate hero)
    {
        // Usamos la helper que creó la conexión correctamente
        using (var con = SQLiteConnectionExtensions.GetConnection())
        {
            con.Open();

            string sql = @"
INSERT INTO player_characters
(player_name, template_id, current_hp, max_hp, atk, potions, level)
VALUES (@name, @template, @hp, @hpmax, @atk, @potions, 1);";

            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@name", playerName);
                cmd.Parameters.AddWithValue("@template", hero.Id);
                cmd.Parameters.AddWithValue("@hp", hero.BaseHP);
                cmd.Parameters.AddWithValue("@hpmax", hero.BaseHP);
                cmd.Parameters.AddWithValue("@atk", hero.BaseATK);
                cmd.Parameters.AddWithValue("@potions", hero.BasePotions);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
