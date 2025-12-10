using System.Collections.Generic;
using System.Data.SQLite;
using SQLiteUtil;

public class HeroTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BaseHP { get; set; }
    public int BaseATK { get; set; }
    public int BasePotions { get; set; }
}

public class HeroTemplateDAO
{
    public List<HeroTemplate> GetAll()
    {
        List<HeroTemplate> list = new List<HeroTemplate>();

        using (var con = SQLiteConnectionExtensions.GetConnection())
        {
            con.Open();

            using (var cmd = new SQLiteCommand("SELECT id, name, base_hp, base_atk, base_potions FROM hero_templates", con))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new HeroTemplate
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        BaseHP = reader.GetInt32(2),
                        BaseATK = reader.GetInt32(3),
                        BasePotions = reader.GetInt32(4)
                    });
                }
            }
        }

        return list;
    }
}
