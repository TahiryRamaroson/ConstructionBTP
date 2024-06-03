using Npgsql;
using System.Text;
using System.Security.Cryptography;

namespace Construction.Models
{
	public class Utilisateur
	{
		public int id { get; set; }
		public string nom { get; set; }
		public string prenom { get; set; }
		public DateOnly naissance { get; set; }
		public string numTel { get; set; }
		public string email { get; set; }
		public string mdp { get; set; }
		public string role { get; set; }
		public Utilisateur() { }
		public Utilisateur(int id, string nom, string prenom, DateOnly naissance, string numTel, string email, string password, string role)
		{
			this.id = id;
			this.nom = nom;
			this.prenom = prenom;
			this.naissance = naissance;
			this.numTel = numTel;
			this.email = email;
			this.mdp = password;
			this.role = role;
		}
		public static Boolean checkLogIn(NpgsqlConnection connect, String mail, String password)
		{
			Boolean iscreated = false;

			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand($"select * from Utilisateur where email= @mel and mdp= @password", connect);
				sql.Parameters.AddWithValue("@mel", mail);
				sql.Parameters.AddWithValue("@password", password);
				Console.WriteLine(sql.CommandText);
				foreach (NpgsqlParameter param in sql.Parameters)
				{
					Console.WriteLine($"{param.ParameterName}: {param.Value}");
				}
				NpgsqlDataReader reader = sql.ExecuteReader();
				if (reader.HasRows) return true;

				reader.Close();

				return false;
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
			return false;
		}
		public static string GetHashSha256(string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			SHA256Managed hashstring = new SHA256Managed();
			byte[] hash = hashstring.ComputeHash(bytes);
			string hashString = string.Empty;
			foreach (byte x in hash)
			{
				hashString += String.Format("{0:x2}", x);
			}
			return hashString;
		}

		public static int getIdByEmailPassword(NpgsqlConnection connect, string mail, string password)
		{
			Boolean iscreated = false;
			int rep = 0;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				NpgsqlCommand sql = new NpgsqlCommand($"select id from Utilisateur where email= @mel and mdp= @password", connect);
				sql.Parameters.AddWithValue("@mel", mail);
				sql.Parameters.AddWithValue("@password", password);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					rep = reader.GetInt32(0);
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

		public static Boolean checkNumTel(NpgsqlConnection connect, String telephone)
		{
			Boolean iscreated = false;

			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand($"select * from Utilisateur where numTel= @nt", connect);
				sql.Parameters.AddWithValue("@nt", telephone);
				Console.WriteLine(sql.CommandText);
				foreach (NpgsqlParameter param in sql.Parameters)
				{
					Console.WriteLine($"{param.ParameterName}: {param.Value}");
				}
				NpgsqlDataReader reader = sql.ExecuteReader();
				if (reader.HasRows) return true;

				reader.Close();

				return false;
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
			return false;
		}

		public static int getIdByNumTel(NpgsqlConnection connect, string telephone)
		{
			Boolean iscreated = false;
			int rep = 0;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				NpgsqlCommand sql = new NpgsqlCommand($"select id from Utilisateur where numTel= @nt", connect);
				sql.Parameters.AddWithValue("@nt", telephone);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					rep = reader.GetInt32(0);
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

		public static void insertClient(NpgsqlConnection connect, string telephone)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand($"insert into utilisateur values(default, null, null, null, @tel, null, null, 'CLIENT')", connect);
				sql.Parameters.AddWithValue("@tel", telephone);
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

		public static int getLastId(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			int rep = 0;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				NpgsqlCommand sql = new NpgsqlCommand($"select max(id) from Utilisateur", connect);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					rep = reader.GetInt32(0);
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
