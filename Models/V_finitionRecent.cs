using Npgsql;

namespace Construction.Models
{
    public class V_finitionRecent
    {
        public int id { get; set; }
        public string nom { get; set;}
        public double taux { get; set; }
        public V_finitionRecent() { }
        public V_finitionRecent(int id, string nom, double taux)
        {
            this.id = id;
            this.nom = nom;
            this.taux = taux;
        }

        public static List<V_finitionRecent> GetAll(NpgsqlConnection connect)
        {
            Boolean iscreated = false;
            List<V_finitionRecent> all = new List<V_finitionRecent>();
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "select * from V_finitionRecent";
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    V_finitionRecent bdc = new V_finitionRecent(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2));
                    all.Add(bdc);
                }
                reader.Close();
                return all;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                try
                {
                    if (iscreated == true) connect.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return all;
        }

        public static double GetTauxbyIdFinition(NpgsqlConnection connect, int fini)
        {
            Boolean iscreated = false;
            double rep = 0;
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "select taux from V_finitionRecent where id=" + fini;
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    rep = reader.GetDouble(0);
                }
                reader.Close();
                return rep;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                try
                {
                    if (iscreated == true) connect.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return rep;
        }

        public static string GetNombyIdFinition(NpgsqlConnection connect, int fini)
        {
            Boolean iscreated = false;
            string rep = "";
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "select nom from V_finitionRecent where id="+fini;
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    rep = reader.GetString(0);
                }
                reader.Close();
                return rep;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                try
                {
                    if (iscreated == true) connect.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return rep;
        }
    }
}
