using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SQLiteUtil;

public class Score
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int ReachedLevel { get; set; }
    public int RemainingHP { get; set; }
    public string Result { get; set; } // "WIN" o "LOSE"
    public DateTime FinishDate { get; set; }
}

public class ScoreDAO
{
    // INSERTAR un score nuevo
    public void InsertScore(Score s)
    {
        using (var con = SQLiteConnectionExtensions.GetConnection())
        {
            con.Open();
            using (var cmd = new SQLiteCommand(
                "INSERT INTO scores(player_name, reached_level, remaining_hp, result, finish_date) " +
                "VALUES (@name, @lvl, @hp, @result, @date)", con))
            {
                cmd.Parameters.AddWithValue("@name", s.PlayerName);
                cmd.Parameters.AddWithValue("@lvl", s.ReachedLevel);
                cmd.Parameters.AddWithValue("@hp", s.RemainingHP);
                cmd.Parameters.AddWithValue("@result", s.Result);
                cmd.Parameters.AddWithValue("@date", s.FinishDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
        }
    }

    // OBTENER TOP SCORES
    public List<Score> GetTopScores(int limit = 50)
    {
        List<Score> list = new List<Score>();

        using (var con = SQLiteConnectionExtensions.GetConnection())
        {
            con.Open();

            using (var cmd = new SQLiteCommand(
                   "SELECT id, player_name, reached_level, remaining_hp, finished_at, result FROM scores ORDER BY reached_level DESC;", con))

            /*
             *     var cmd = new SQLiteCommand(
                "SELECT id, player_name, reached_level, remaining_hp, finished_at, result " +
                "FROM scores ORDER BY reached_level DESC;",con);

             */
            {
                cmd.Parameters.AddWithValue("@limit", limit);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Score
                        {
                            Id = reader.GetInt32(0),
                            PlayerName = reader.GetString(1),
                            ReachedLevel = reader.GetInt32(2),
                            RemainingHP = reader.GetInt32(3),
                            FinishDate = DateTime.Parse(reader.GetString(4)),
                            Result = reader.GetString(5)
                        });
                    }
                }
            }
        }

        return list;
    }
}
