using Npgsql;

namespace Construction.Models
{
	public class DevisCsv
	{
		public long lineNumber {  get; set; }
		public string client {  get; set; }             //eto raha mila verification numero regex 
        public string ref_devis {  get; set; }   
        public string type_maison {  get; set; }
        public string finition {  get; set; }
        private double _taux_finition;
        public double taux_finition
        {
            get { return _taux_finition; }
			set { if (value < 0) { throw new Exception("Taux finition ne peut pas être négatif"); } _taux_finition = value; }
		}
        private DateOnly _date_devis;
        public DateOnly date_devis
        {
            get { return _date_devis; }
			set { if (value < date_debut) { throw new Exception("La date du devis ne peut pas être supérieure à la date_debut"); } _date_devis = value; }
		}  
        public DateOnly date_debut { get; set; }  
        public string lieu {  get; set; }    
		public DevisCsv() { }

        public DevisCsv(long lineNumber, string client, string ref_devis, string type_maison, string finition, double taux_finition, DateOnly date_devis, DateOnly date_debut, string lieu)
		{
			this.lineNumber = lineNumber;
			this.client = client;
			this.ref_devis = ref_devis;
			this.type_maison = type_maison;
			this.finition = finition;
			this.taux_finition = taux_finition;
			this.date_devis = date_devis;
			this.date_debut = date_debut;
			this.lieu = lieu;
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
				            
				NpgsqlCommand sql = new NpgsqlCommand($"insert into DevisCsv values(@client, @ref_devis, @type_maison, @finition, @taux_finition, @date_devis, @date_debut, @lieu)", connect);
				sql.Parameters.AddWithValue("@client", this.client);
				sql.Parameters.AddWithValue("@ref_devis", this.ref_devis);
				sql.Parameters.AddWithValue("@type_maison", this.type_maison);
				sql.Parameters.AddWithValue("@finition", this.finition);
				sql.Parameters.AddWithValue("@taux_finition", this.taux_finition);
				sql.Parameters.AddWithValue("@date_devis", this.date_devis);
				sql.Parameters.AddWithValue("@date_debut", this.date_debut);
				sql.Parameters.AddWithValue("@lieu", this.lieu);
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

				NpgsqlCommand sql = new NpgsqlCommand("delete from DevisCsv", connect);
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
