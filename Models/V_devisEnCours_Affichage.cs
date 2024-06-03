using Npgsql;

namespace Construction.Models
{
	public class V_devisEnCours_Affichage
	{
		public int id { get; set; }
		public string numero { get; set; }
		public int idClient { get; set; }
		public int idMaison { get; set; }
		public string nomMaison { get; set; }
		public double montantTravaux { get; set; }
		public double tauxFinition { get; set; }
		public string nomFinition { get; set; }
		public DateTime debutTravaux { get; set; }
		public DateOnly dateCreation { get; set; }
		public double totalPaiement { get; set; }
		public double montantTotal { get; set; }
		public string numTel {  get; set; }

		public V_devisEnCours_Affichage() { }
		public V_devisEnCours_Affichage(int id, string numero, int idClient, int idMaison, string nomMaison, double montantTravaux, double tauxFinition, string nomFinition, DateTime debutTravaux, DateOnly dateCreation, double totalPaiement, double montantTotal, string numTel)
		{
			this.id = id;
			this.numero = numero;
			this.idClient = idClient;
			this.idMaison = idMaison;
			this.nomMaison = nomMaison;
			this.montantTravaux = montantTravaux;
			this.tauxFinition = tauxFinition;
			this.nomFinition = nomFinition;
			this.debutTravaux = debutTravaux;
			this.dateCreation = dateCreation;
			this.totalPaiement = totalPaiement;
			this.montantTotal = montantTotal;
			this.numTel = numTel;
		}

		public static List<V_devisEnCours_Affichage> GetDevisEnCoursPage(NpgsqlConnection connect, int pageNumber, int pageSize)
		{
			Boolean iscreated = false;
			List<V_devisEnCours_Affichage> page = new List<V_devisEnCours_Affichage>();
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "SELECT * FROM V_devisEnCours_Affichage ORDER BY id LIMIT @PageSize OFFSET @Offset";
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				sql.Parameters.AddWithValue("@PageSize", pageSize);
				sql.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					V_devisEnCours_Affichage dv = new V_devisEnCours_Affichage(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4), reader.GetDouble(5), reader.GetDouble(6), reader.GetString(7), reader.GetDateTime(8), DateOnly.FromDateTime(reader.GetDateTime(9)), reader.GetDouble(10), reader.GetDouble(11), reader.GetString(12));
					page.Add(dv);
				}
				reader.Close();
				return page;
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
			return page;
		}

		public static int GetTotalCount(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			int totalCount = 0;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "SELECT COUNT(*) FROM V_devisEnCours_Affichage";
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					totalCount = reader.GetInt32(0);
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
			return totalCount;
		}
	}
}
