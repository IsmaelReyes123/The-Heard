using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using SQLiteUtil;

public class Monster
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BaseHP { get; set; }
    public int BaseATK { get; set; }
    public int CurrentHP { get; set; }
    public bool IsBoss => (Id % 5) == 0;
}

public class PlayDAO
{
    private static readonly string[] CandidateTableNames = new[] { "monster_templates", "monsters", "monster", "mob_templates", "mobs" };
    private Random rng = new Random();

    public List<Monster> GetAll()
    {
        string table = DetectMonsterTable();
        if (table == null) return new List<Monster>();

        var result = new List<Monster>();
        using (var con = SQLiteConnectionExtensions.GetConnection())
        {
            con.Open();
            var mapping = BuildColumnMapping(table, con);
            string sql = $"SELECT * FROM {table};";
            using (var cmd = new SQLiteCommand(sql, con))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(ReadMonsterFromReader(reader, mapping));
                }
            }
        }
        return result;
    }

    public Monster GetByUid(int uid)
    {
        string table = DetectMonsterTable();
        if (table == null) return null;

        using (var con = SQLiteConnectionExtensions.GetConnection())
        {
            con.Open();
            var mapping = BuildColumnMapping(table, con);
            string sql = $"SELECT * FROM {table} WHERE {mapping.IdColumn} = @uid LIMIT 1;";
            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@uid", uid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return ReadMonsterFromReader(reader, mapping);
                }
            }
        }
        return null;
    }

    // Esta cosa los hace aleatorios intente que no se repetieran pero fue mucho pedo
    public Monster GetRandomInRange(int startInclusive, int endInclusive)
    {
        if (startInclusive > endInclusive) throw new ArgumentException("startInclusive must be <= endInclusive");

        // Intentos aleatorios
        int attempts = endInclusive - startInclusive + 1;
        for (int i = 0; i < attempts; i++)
        {
            int uid = rng.Next(startInclusive, endInclusive + 1);
            var m = GetByUid(uid);
            if (m != null) return m;
        }

        // Fallback: intenta secuencialmente
        for (int uid = startInclusive; uid <= endInclusive; uid++)
        {
            var m = GetByUid(uid);
            if (m != null) return m;
        }

        return null;
    }

    private string DetectMonsterTable()
    {
        using (var con = SQLiteConnectionExtensions.GetConnection())
        {
            con.Open();
            // Buscar nombres candidatos exactos
            foreach (var name in CandidateTableNames)
            {
                using (var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND lower(name)=@name LIMIT 1;", con))
                {
                    cmd.Parameters.AddWithValue("@name", name.ToLower());
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read()) return r.GetString(0);
                    }
                }
            }

            // Buscar cualquier tabla que contenga 'monster' o 'mob'
            using (var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND (lower(name) LIKE '%monster%' OR lower(name) LIKE '%mob%') LIMIT 1;", con))
            using (var r = cmd.ExecuteReader())
            {
                if (r.Read()) return r.GetString(0);
            }
        }
        return null;
    }

    private ColumnMapping BuildColumnMapping(string tableName, SQLiteConnection con)
    {
        var colsOrig = new List<string>();
        var colsLower = new List<string>();
        using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName});", con))
        using (var r = cmd.ExecuteReader())
        {
            while (r.Read())
            {
                var cname = r.GetString(1);
                colsOrig.Add(cname);
                colsLower.Add(cname.ToLower());
            }
        }

        var mapping = new ColumnMapping();
        if (colsOrig.Count == 0)
        {
            mapping.IdColumn = "id";
            mapping.NameColumn = "name";
            mapping.BaseHpColumn = "base_hp";
            mapping.BaseAtkColumn = "base_atk";
            return mapping;
        }

        int idx;
        idx = colsLower.IndexOf("id");
        if (idx < 0) idx = colsLower.IndexOf("uid");
        mapping.IdColumn = idx >= 0 ? colsOrig[idx] : colsOrig[0];

        idx = colsLower.IndexOf("name");
        if (idx < 0) idx = colsLower.IndexOf("monster_name");
        mapping.NameColumn = idx >= 0 ? colsOrig[idx] : colsOrig.FirstOrDefault(c => !string.Equals(c, mapping.IdColumn, StringComparison.OrdinalIgnoreCase)) ?? mapping.IdColumn;

        idx = colsLower.IndexOf("base_hp");
        if (idx < 0) idx = colsLower.IndexOf("hp");
        if (idx < 0) idx = colsLower.IndexOf("max_hp");
        mapping.BaseHpColumn = idx >= 0 ? colsOrig[idx] : colsOrig.FirstOrDefault(c => !string.Equals(c, mapping.IdColumn, StringComparison.OrdinalIgnoreCase) && !string.Equals(c, mapping.NameColumn, StringComparison.OrdinalIgnoreCase)) ?? mapping.IdColumn;

        idx = colsLower.IndexOf("base_atk");
        if (idx < 0) idx = colsLower.IndexOf("atk");
        if (idx < 0) idx = colsLower.IndexOf("attack");
        mapping.BaseAtkColumn = idx >= 0 ? colsOrig[idx] : colsOrig.FirstOrDefault(c => !string.Equals(c, mapping.IdColumn, StringComparison.OrdinalIgnoreCase) && !string.Equals(c, mapping.NameColumn, StringComparison.OrdinalIgnoreCase) && !string.Equals(c, mapping.BaseHpColumn, StringComparison.OrdinalIgnoreCase)) ?? mapping.IdColumn;

        return mapping;
    }

    private Monster ReadMonsterFromReader(SQLiteDataReader reader, ColumnMapping mapping)
    {
        var m = new Monster();
        m.Id = SafeGetInt(reader, mapping.IdColumn, 0);
        m.Name = SafeGetString(reader, mapping.NameColumn, "Monstruo " + m.Id);
        m.BaseHP = SafeGetInt(reader, mapping.BaseHpColumn, 50 + m.Id * 10);
        m.BaseATK = SafeGetInt(reader, mapping.BaseAtkColumn, 5 + m.Id * 2);
        m.CurrentHP = m.BaseHP;
        return m;
    }

    private int SafeGetInt(SQLiteDataReader r, string col, int fallback)
    {
        try
        {
            int ord = r.GetOrdinal(col);
            if (r.IsDBNull(ord)) return fallback;
            return Convert.ToInt32(r.GetValue(ord));
        }
        catch
        {
            return fallback;
        }
    }

    private string SafeGetString(SQLiteDataReader r, string col, string fallback)
    {
        try
        {
            int ord = r.GetOrdinal(col);
            if (r.IsDBNull(ord)) return fallback;
            return Convert.ToString(r.GetValue(ord));
        }
        catch
        {
            return fallback;
        }
    }

    private class ColumnMapping
    {
        public string IdColumn;
        public string NameColumn;
        public string BaseHpColumn;
        public string BaseAtkColumn;
    }
}
