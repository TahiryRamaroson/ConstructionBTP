using Npgsql;

namespace Construction.Models
{
	public class PaiementCsv
	{
		public long lineNumber { get; set; }
		public string ref_devis { get; set; }
		public string ref_paiement { get; set; }
		public DateOnly date_paiement { get; set; }
		private double _montant;
		public double montant
		{
			get { return _montant; }
			set { if (value < 0) { throw new Exception("Montant ne peut pas être négatif"); } _montant = value; }
		}

		public PaiementCsv() { }

		public PaiementCsv(long lineNumber, string ref_devis, string ref_paiement, DateOnly date_paiement, double montant)
		{
			this.lineNumber = lineNumber;
			this.ref_devis = ref_devis;
			this.ref_paiement = ref_paiement;
			this.date_paiement = date_paiement;
			this.montant = montant;
		}

		public void insert(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand($"insert into PaiementCsv values(@ref_devis, @ref_paiement, @date_paiement, @montant)", connect);
				sql.Parameters.AddWithValue("@ref_devis", this.ref_devis);
				sql.Parameters.AddWithValue("@ref_paiement", this.ref_paiement);
				sql.Parameters.AddWithValue("@date_paiement", this.date_paiement);
				sql.Parameters.AddWithValue("@montant", this.montant);
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

		public static void deleteAll(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("delete from PaiementCsv", connect);
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

		public static Boolean checkReference(NpgsqlConnection connect, string reference)
		{
			Boolean iscreated = false;

			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("select * from PaiementCsv where numero='" + reference + "'", connect);
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
	}
}
