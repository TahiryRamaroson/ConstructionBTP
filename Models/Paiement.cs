using Npgsql;
using System.Collections.Generic;

namespace Construction.Models
{
	public class Paiement
	{
		public int id {  get; set; }
		public double montant { get; set; }
		public int idDevis { get; set; }
		public DateTime datePaiement { get; set; }
		public string numero { get; set; }
		public Paiement() { }
		public Paiement(int id, double montant, int idDevis, DateTime datePaiement, string numero)
		{
			this.id = id;
			this.montant = montant;
			this.idDevis = idDevis;
			this.datePaiement = datePaiement;
			this.numero = numero;
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

				NpgsqlCommand sql = new NpgsqlCommand("insert into Paiement values(default, " + this.montant +", "+ this.idDevis+", '"+ this.datePaiement.ToString()+"', 'A45PF1')", connect);
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

        public static double GetTotalPaiement(NpgsqlConnection connect)
        {
            Boolean iscreated = false;
            double total = 0;
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "SELECT SUM(montant) FROM Paiement";
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    total = reader.GetDouble(0);
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
            return total;
        }

        public static List<Paiement> GetPaiementbydevis(NpgsqlConnection connect, int devis)
        {
            Boolean iscreated = false;
            List<Paiement> all = new List<Paiement>();
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "SELECT * FROM Paiement where iddevis="+ devis;
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    Paiement rep = new Paiement(reader.GetInt32(0), reader.GetDouble(1), reader.GetInt32(2), reader.GetDateTime(3), reader.GetString(4));
                    all.Add(rep);
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

        public static double GetTotalPaiementbyDevis(NpgsqlConnection connect, int devis)
        {
            Boolean iscreated = false;
            double total = 0;
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "SELECT SUM(montant) FROM Paiement where iddevis="+devis;
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    total = reader.GetDouble(0);
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
            return total;
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

				NpgsqlCommand sql = new NpgsqlCommand("select * from Paiement where numero='"+ reference +"'", connect);
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
