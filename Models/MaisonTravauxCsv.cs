using Npgsql;
using System.ComponentModel;

namespace Construction.Models
{
	public class MaisonTravauxCsv
	{
		public long lineNumber {  get; set; }
		public string type_maison {  get; set; }
        public string description { get; set; }
		private double _surface;
		public double surface
		{
			get { return _surface; }
			set { if (value < 0) { throw new Exception("Surface ne peut pas être négatif"); } _surface = value; }
		}
		public string code_travaux { get; set; }
		public string type_travaux { get; set; }
		public string unite { get; set; }
		private double _prixunitaire;
		public double prixunitaire 
		{
			get { return _prixunitaire; }
			set { if (value < 0) { throw new Exception("Prix Unitaire ne peut pas être négatif"); } _prixunitaire = value; }
		}
		private double _quantite;
		public double quantite 
		{
			get { return _quantite; }
			set { if (value < 0) { throw new Exception("Quantité ne peut pas être négatif"); } _quantite = value; }
		}
		private double _duree_travaux;
		public double duree_travaux 
		{
			get { return _duree_travaux; }
			set { if (value < 0) { throw new Exception("Durée de travaux ne peut pas être négatif"); } _duree_travaux = value; }
		}

		public MaisonTravauxCsv() { }
		public MaisonTravauxCsv(long lineNumber, string type_maison, string description, double surface, string code_travaux, string type_travaux, string unite, double prixunitaire, double quantite, double duree_travaux)
		{
			this.lineNumber = lineNumber;
			this.type_maison = type_maison;
			this.description = description;
			this.surface = surface;
			this.code_travaux = code_travaux;
			this.type_travaux = type_travaux;
			this.unite = unite;
			this.prixunitaire = prixunitaire;
			this.quantite = quantite;
			this.duree_travaux = duree_travaux;
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

				NpgsqlCommand sql = new NpgsqlCommand($"insert into MaisonTravauxCsv values(@maison, @desc, @surface, @codeTravaux, @typeTravaux, @unite, @prixu, @quantite, @dureeTravaux)", connect);
				sql.Parameters.AddWithValue("@maison", this.type_maison);
				sql.Parameters.AddWithValue("@desc", this.description);
				sql.Parameters.AddWithValue("@surface", this.surface);
				sql.Parameters.AddWithValue("@codeTravaux", this.code_travaux);
				sql.Parameters.AddWithValue("@typeTravaux", this.type_travaux);
				sql.Parameters.AddWithValue("@unite", this.unite);
				sql.Parameters.AddWithValue("@prixu", this.prixunitaire);
				sql.Parameters.AddWithValue("@quantite", this.quantite);
				sql.Parameters.AddWithValue("@dureeTravaux", this.duree_travaux);
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

				NpgsqlCommand sql = new NpgsqlCommand("delete from MaisonTravauxCsv", connect);
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
