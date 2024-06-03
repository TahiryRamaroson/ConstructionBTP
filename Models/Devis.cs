using Npgsql;

namespace Construction.Models
{
	public class Devis
	{
		public int id {  get; set; }
		public string numero { get; set; }
		public int idClient { get; set; }
		public int idMaison { get; set; }
		public string nomMaison { get; set; }
		public double montantTravaux { get; set; }
		public double tauxFinition { get; set; }
		public string nomFinition { get; set; }
		public DateTime debutTravaux { get; set; }
		public DateOnly dateCreation { get; set; }
		public string lieu {  get; set; }
		public Devis() { }
		public Devis(int id, string numero, int idClient, int idMaison, string nomMaison, double montantTravaux, double tauxFinition, string nomFinition, DateTime debutTravaux, DateOnly dateCreation, string lieu)
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
			this.lieu = lieu;
		}

		public static int GetMyTotalCount(NpgsqlConnection connect, int idcli)
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
				String script = "SELECT COUNT(*) FROM Devis where idClient= " + idcli;
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

		public static List<Devis> GetMyDevisPage(NpgsqlConnection connect, int pageNumber, int pageSize, int client)
		{
			Boolean iscreated = false;
			List<Devis> page = new List<Devis>();
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "SELECT * FROM Devis where idClient= @cli ORDER BY id LIMIT @PageSize OFFSET @Offset";
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				sql.Parameters.AddWithValue("@PageSize", pageSize);
				sql.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
				sql.Parameters.AddWithValue("@cli", client);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					Devis dv = new Devis(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4), reader.GetDouble(5), reader.GetDouble(6), reader.GetString(7), reader.GetDateTime(8), DateOnly.FromDateTime(reader.GetDateTime(9)), reader.GetString(10));
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

		public static double GetTauxByDevis(NpgsqlConnection connect, int devis)
		{
			Boolean iscreated = false;
			double taux = 0;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "SELECT tauxFinition FROM Devis where id= " + devis;
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					taux = reader.GetDouble(0);
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
			return taux;
		}

		public static double GetMontantTotal(NpgsqlConnection connect)
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
				String script = "select sum(montantTravaux + montantTravaux * tauxFinition / 100) from Devis";
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

                NpgsqlCommand sql = new NpgsqlCommand($"insert into Devis values(default, @numero ,@idClient ,@idMaison ,@nomMaison ,@montantTravaux ,@tauxFinition ,@nomFinition ,@debutTravaux ,@dateCreation ,@lieu)", connect);
				sql.Parameters.AddWithValue("@numero", this.numero);
                sql.Parameters.AddWithValue("@idClient", this.idClient);
                sql.Parameters.AddWithValue("@idMaison", this.idMaison);
                sql.Parameters.AddWithValue("@nomMaison", this.nomMaison);
                sql.Parameters.AddWithValue("@montantTravaux", this.montantTravaux);
                sql.Parameters.AddWithValue("@tauxFinition", this.tauxFinition);
                sql.Parameters.AddWithValue("@nomFinition", this.nomFinition);
                sql.Parameters.AddWithValue("@debutTravaux", this.debutTravaux);
                sql.Parameters.AddWithValue("@dateCreation", this.dateCreation);
                sql.Parameters.AddWithValue("@lieu", this.lieu);
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

        public static int GetLastId(NpgsqlConnection connect)
        {
            Boolean iscreated = false;
            int last = 0;
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }
                String script = "select max(id) from Devis";
                NpgsqlCommand sql = new NpgsqlCommand(script, connect);
                NpgsqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    last = reader.GetInt32(0);
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
            return last;
        }

		public static List<double> GetMontantParAnnee(NpgsqlConnection connect, int annee)
		{
			Boolean iscreated = false;
			List<double> all = new List<double>();
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}
				String script = "WITH months AS (SELECT generate_series(1, 12) AS month) SELECT months.month,'"+annee.ToString()+"'AS year, COALESCE(SUM(d.montantTravaux + d.montantTravaux * d.tauxFinition / 100), 0) AS montant_total FROM months LEFT JOIN Devis d ON months.month = EXTRACT(MONTH FROM d.dateCreation) AND EXTRACT(YEAR FROM d.dateCreation) = '"+annee.ToString()+"' GROUP BY months.month ORDER BY months.month";
				NpgsqlCommand sql = new NpgsqlCommand(script, connect);
				NpgsqlDataReader reader = sql.ExecuteReader();
				while (reader.Read())
				{
					double stat = reader.GetDouble(2);
					all.Add(stat);
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
	}
}
