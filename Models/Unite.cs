using Npgsql;

namespace Construction.Models
{
	public class Unite
	{
		public int id { get; set; }
		public string nom { get; set; }
		public Unite() { }
		public Unite(int id, string nom)
		{
			this.id = id;
			this.nom = nom;
		}
		public static List<Unite> getAll(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			List<Unite> all = new List<Unite>();
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "select * from Unite";
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					Unite bdc = new Unite(reader.GetInt32(0), reader.GetString(1));
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

		
	}
}
