using Npgsql;

namespace Construction.Models
{
	public class Connexion
	{
		public static NpgsqlConnection getConnection()
		{
			string config = "Host=localhost;Database=construction;Username=postgres;Password=Tahiry1849";
			NpgsqlConnection connect;
			try
			{
				connect = new NpgsqlConnection(config);
				connect.Open();
				return connect;
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}
