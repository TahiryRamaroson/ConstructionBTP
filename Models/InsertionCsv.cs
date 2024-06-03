using Npgsql;

namespace Construction.Models
{
	public class InsertionCsv
	{
		public static void insertMaison(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO Maison (nom, description, duree, surface) SELECT distinct(type_maison), description, CAST(duree_travaux AS double precision), CAST(surface AS double precision) FROM MaisonTravauxCsv", connect);
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

		public static void insertUnite(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO Unite (nom) SELECT distinct unite FROM MaisonTravauxCsv", connect);
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

		public static void insertTravaux(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO Travaux (numero, nom, prixUnitaire, idunite) SELECT distinct code_travaux, type_travaux, CAST(prixunitaire AS double precision), Unite.id FROM MaisonTravauxCsv Join Unite on MaisonTravauxCsv.unite = Unite.nom", connect);
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

		public static void insertTravauxMaison(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO TravauxMaison (idMaison, idTravaux, quantite, dateinsertion) SELECT distinct Maison.id as idmaison, Travaux.id as idtravaux, CAST(MaisonTravauxCsv.quantite AS double precision), LOCALTIMESTAMP FROM MaisonTravauxCsv Join Maison on MaisonTravauxCsv.type_maison = Maison.nom Join Travaux on MaisonTravauxCsv.type_travaux = Travaux.nom", connect);
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

		public static void insertUtilisateurClient(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO Utilisateur (numTel) SELECT distinct client FROM DevisCsv", connect);
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

		public static void insertFinition(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO Finition (nom, taux) SELECT distinct finition, CAST(taux_finition AS double precision) FROM DevisCsv", connect);
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

		public static void insertDevis(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO Devis (numero, idClient, idMaison, nomMaison, tauxFinition, nomFinition, debutTravaux, dateCreation, lieu) SELECT ref_devis, (SELECT id FROM Utilisateur WHERE numTel = client), (SELECT id FROM Maison WHERE nom = type_maison), type_maison, CAST(taux_finition AS double precision), finition, TO_TIMESTAMP(date_debut, 'YYYY-MM-DD'), TO_DATE(date_devis, 'YYYY-MM-DD'), lieu FROM DevisCsv", connect);
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

		public static void insertDevisDetails(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO DevisDetails (idDevis, idTravaux, quantite, prixUnitaire) SELECT (SELECT id FROM Devis WHERE numero = ref_devis), idTravaux, quantite, (SELECT prixUnitaire FROM Travaux WHERE id = idTravaux) FROM DevisCsv, TravauxMaison WHERE DevisCsv.type_maison = (SELECT nom FROM Maison WHERE id = TravauxMaison.idMaison)", connect);
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

		public static void updateMontantDevis(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("UPDATE Devis SET montantTravaux = (SELECT SUM(quantite * prixUnitaire) FROM DevisDetails WHERE Devis.id = DevisDetails.idDevis) WHERE EXISTS ( SELECT 1 FROM DevisDetails WHERE Devis.id = DevisDetails.idDevis)", connect);
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

		public static void insertPaiement(NpgsqlConnection connect)
		{
			Boolean iscreated = false;
			try
			{
				if (connect == null)
				{
					connect = Connexion.getConnection();
					iscreated = true;
				}

				NpgsqlCommand sql = new NpgsqlCommand("INSERT INTO Paiement (montant, idDevis, datePaiement, numero) SELECT CAST(montant AS double precision), Devis.id, TO_TIMESTAMP(date_paiement, 'YYYY-MM-DD'), ref_paiement FROM PaiementCsv JOIN Devis on PaiementCsv.ref_devis = Devis.numero", connect);
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

        public static void delete(NpgsqlConnection connect, string table)
        {
            Boolean iscreated = false;
            try
            {
                if (connect == null)
                {
                    connect = Connexion.getConnection();
                    iscreated = true;
                }

                NpgsqlCommand sql = new NpgsqlCommand("DELETE FROM " + table, connect);
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
		
	}
}
