using Npgsql;

namespace Construction.Models
{
	public class Finition
	{
		public int id { get; set; }
		public string nom { get; set; }
		public double taux {  get; set; }
		public Finition() { }
		public Finition(int id, string nom, double taux)
		{
			this.id = id;
			this.nom = nom;
			this.taux = taux;
		}
		public static List<Finition> GetPage(NpgsqlConnection connect, int pageNumber, int pageSize)
		{
			Boolean iscreated = false;
			List<Finition> page = new List<Finition>();
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "SELECT * FROM Finition ORDER BY id LIMIT @PageSize OFFSET @Offset";
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				sql.Parameters.AddWithValue("@PageSize", pageSize);
				sql.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					Finition bdc = new Finition(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2));
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
				String script = "SELECT COUNT(*) FROM Finition";
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
		public static void update(NpgsqlConnection connect, int idfinition, double taux)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand($"update Finition set taux= @pourcentage where id=@fin", connect);
				sql.Parameters.AddWithValue("@pourcentage", taux);
				sql.Parameters.AddWithValue("@fin", idfinition);
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
