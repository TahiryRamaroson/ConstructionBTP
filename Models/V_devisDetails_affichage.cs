using Npgsql;

namespace Construction.Models
{
	public class V_devisDetails_affichage
	{
		public int idDevis {  get; set; }
		public double quantite { get; set; }
		public double prixUnitaire { get; set; }
		public string nomTravaux { get; set; }
		public string nomUnite { get; set; }

		public V_devisDetails_affichage() { }
		public V_devisDetails_affichage(int idDevis, double quantite, double prixUnitaire, string nomTravaux, string nomUnite)
		{
			this.idDevis = idDevis;
			this.quantite = quantite;
			this.prixUnitaire = prixUnitaire;
			this.nomTravaux = nomTravaux;
			this.nomUnite = nomUnite;
		}

		public static List<V_devisDetails_affichage> GetByDevis(NpgsqlConnection connect, int devis)
		{
			Boolean iscreated = false;
			List<V_devisDetails_affichage> all = new List<V_devisDetails_affichage>();
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "SELECT * FROM V_devisDetails_affichage where idDevis= " + devis;
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					V_devisDetails_affichage details = new V_devisDetails_affichage(reader.GetInt32(0), reader.GetDouble(1), reader.GetDouble(2), reader.GetString(3), reader.GetString(4));
					all.Add(details);
				}
				reader.Close();
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

        public static void insertDetailsDevis(NpgsqlConnection connect, int devis, int travaux, double quantite, double pu)
        {
            Boolean iscreated = false;
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }

                NpgsqlCommand sql = new NpgsqlCommand($"insert into DevisDetails values(@devis, @travaux, @quantite, @pu)", connect);
				sql.Parameters.AddWithValue("@devis", devis);
                sql.Parameters.AddWithValue("@travaux", travaux);
                sql.Parameters.AddWithValue("@quantite", quantite);
                sql.Parameters.AddWithValue("@pu", pu);
                Console.WriteLine(sql.CommandText);


                sql.ExecuteNonQuery();

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
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
