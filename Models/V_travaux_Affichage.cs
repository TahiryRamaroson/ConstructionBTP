using Npgsql;

namespace Construction.Models
{
	public class V_travaux_Affichage
	{
		public int id {  get; set; }
		public string numero { get; set; }
		public string nom { get; set; }
		public int idUnite { get; set; }
		public double prixUnitaire { get; set; }
		public string nomUnite {  get; set; }
		public V_travaux_Affichage() { }
		public V_travaux_Affichage(int id, string numero, string nom, int idUnite, double prixUnitaire, string nomUnite)
		{
			this.id = id;
			this.numero = numero;
			this.nom = nom;
			this.idUnite = idUnite;
			this.prixUnitaire = prixUnitaire;
			this.nomUnite = nomUnite;
		}

		public static List<V_travaux_Affichage> GetPage(NpgsqlConnection connect, int pageNumber, int pageSize)
		{
			Boolean iscreated = false;
			List<V_travaux_Affichage> page = new List<V_travaux_Affichage>();
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "SELECT * FROM V_travaux_Affichage ORDER BY id LIMIT @PageSize OFFSET @Offset";
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				sql.Parameters.AddWithValue("@PageSize", pageSize);
				sql.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					V_travaux_Affichage bdc = new V_travaux_Affichage(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDouble(4), reader.GetString(5));
					page.Add(bdc);
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
				String script = "SELECT COUNT(*) FROM V_travaux_Affichage";
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

		public static void update(NpgsqlConnection connect, int idtravaux, string nom, string refe, int unite, double prixunitaire)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand($"update Travaux set numero= @refe , nom=@anarana, idUnite=@iut, prixUnitaire=@pu where id=@trav", connect);
				sql.Parameters.AddWithValue("@refe", refe);
				sql.Parameters.AddWithValue("@anarana", nom);
				sql.Parameters.AddWithValue("@iut", unite);
				sql.Parameters.AddWithValue("@pu", prixunitaire);
				sql.Parameters.AddWithValue("@trav", idtravaux);
				Console.WriteLine(sql.CommandText);
				foreach (NpgsqlParameter param in sql.Parameters)
				{
					Console.WriteLine($"{param.ParameterName}: {param.Value}");
				}

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
