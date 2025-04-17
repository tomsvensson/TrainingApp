using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;
using TrainingApp.Models;

namespace TrainingApp
{
    public class Database
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=TrainingDB;Integrated Security=True";

        public Database()
        {
            EnsureDatabaseAndTable();
        }

        private void EnsureDatabaseAndTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    // Check if the database exists
                    cmd.CommandText = @"
                        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TrainingDB')
                        BEGIN
                            CREATE DATABASE TrainingDB;
                        END";
                    cmd.ExecuteNonQuery();

                    // Check if the table exists
                    cmd.CommandText = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Sessions' and xtype='U')
                        BEGIN
                            CREATE TABLE Sessions (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Exercise VARCHAR(50),
                                Sets INT,
                                Reps INT,
                                Weight INT
                            );
                        END";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Session> GetSessions()
        {
            SqlCommand cmd = GetDbCommand();
            cmd.CommandText = "SELECT * FROM Sessions";
            var reader = cmd.ExecuteReader();
            var sessions = new List<Session>();
                while (reader.Read())
                {
                    int id = int.Parse(reader["Id"].ToString());
                    string exercise = (reader["Exercise"].ToString());
                    int sets = int.Parse(reader["Sets"].ToString());
                    int reps = int.Parse(reader["Reps"].ToString());
                    int weight = int.Parse(reader["Weight"].ToString());

                    sessions.Add(new Session()
                    {
                        Id = id,
                        Exercise = exercise,
                        Sets = sets,
                        Reps = reps,
                        Weight = weight
                    });
                }
            return sessions;
        }

        private SqlCommand GetDbCommand()
        {
            
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        public void SaveSession(string exercise, int sets, int reps, int weight)
        {
            SqlCommand cmd = GetDbCommand();
            cmd.CommandText = "INSERT INTO Sessions (Exercise, Sets, Reps, Weight) VALUES (@Exercise, @Sets, @Reps, @Weight)";
            cmd.Parameters.AddWithValue("@Exercise", exercise);
            cmd.Parameters.AddWithValue("@Sets", sets);
            cmd.Parameters.AddWithValue("@Reps", reps);
            cmd.Parameters.AddWithValue("@Weight", weight);

            cmd.ExecuteNonQuery();
        }

        public void UpdateSession(Session session)
        {
            SqlCommand cmd = GetDbCommand();
            cmd.CommandText = "UPDATE Sessions SET Exercise = @Exercise, Sets = @Sets, Reps = @Reps, Weight = @Weight WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Exercise", session.Exercise);
            cmd.Parameters.AddWithValue("@Sets", session.Sets);
            cmd.Parameters.AddWithValue("@Reps", session.Reps);
            cmd.Parameters.AddWithValue("@Weight", session.Weight);
            cmd.Parameters.AddWithValue("@Id", session.Id);

            cmd.ExecuteNonQuery();
        }

        public void DeleteSession(int sessionId)
        {
            SqlCommand cmd = GetDbCommand();
            cmd.CommandText = "DELETE FROM Sessions WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", sessionId);

            cmd.ExecuteNonQuery();
        }
    }
}
