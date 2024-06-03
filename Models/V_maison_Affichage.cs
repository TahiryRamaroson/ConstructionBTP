using Npgsql;

namespace Construction.Models
{
    public class V_maison_Affichage
    {
        public int id { get; set; }
        public string nom {  get; set; }
        public string description { get; set;}
        public double duree { get; set; }
        public double surface { get; set; }
        public double sommeTravaux { get; set; }

        public V_maison_Affichage() { }

        public V_maison_Affichage(int id, string nom, string description, double duree, double surface, double sommeTravaux)
        {
            this.id = id;
            this.nom = nom;
            this.description = description;
            this.duree = duree;
            this.surface = surface;
            this.sommeTravaux = sommeTravaux;
        }

        public static List<V_maison_Affichage> GetAll(NpgsqlConnection connect)
        {
            Boolean iscreated = false;
            List<V_maison_Affichage> all = new List<V_maison_Affichage>();
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "select * from V_maison_Affichage";
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    V_maison_Affichage bdc = new V_maison_Affichage(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDouble(3), reader.GetDouble(4), reader.GetDouble(5));
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

        public static string GetNomById(NpgsqlConnection connect, int idtrano)
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
                String script = "select nom from V_maison_Affichage where id="+ idtrano;
                Console.WriteLine(script);
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

        public static double GetMontantByIdMaison(NpgsqlConnection connect, int idtrano)
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
                String script = "select sommeTravaux from V_montantMaison where idMaison=" + idtrano;
                Console.WriteLine(script);
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

        public static List<Tuple<int, double>> GetIdTravauxAndQuantiteByIdMaison(NpgsqlConnection connect, int idm)
        {
            Boolean iscreated = false;
            List<Tuple<int, double>> all = new List<Tuple<int, double>>(); 
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "select IdTravaux, quantite from TravauxMaison where idMaison=" + idm;
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    Tuple<int, double> rep = new Tuple<int, double>(reader.GetInt32(0), reader.GetDouble(1));
                    all.Add(rep);
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

        public static double GetPuByIdTravaux(NpgsqlConnection connect, int idtrav)
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
                String script = "select prixUnitaire from v_tarifRecent where idTravaux=" + idtrav;
                Console.WriteLine(script);
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
    }
}
