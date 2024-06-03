using Construction.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Rotativa.AspNetCore;
using LumenWorks.Framework.IO.Csv;

namespace Construction.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index(int pg = 1)
		{
			if (HttpContext.Session.GetInt32("idAdmin") == null) return RedirectToAction("Index", "LoginAdmin");
			NpgsqlConnection con = Connexion.getConnection();

			try
			{

				//const int pageSize = 5;

				//int recsCount = V_devisEnCours_Affichage.GetTotalCount(con);

				//ViewBag.PageCount = (recsCount + pageSize - 1) / pageSize;
				//ViewBag.CurrentPage = pg;
				//return View(V_devisEnCours_Affichage.GetDevisEnCoursPage(con, pg, pageSize));

				const int pageSize = 5;

				int recsCount = V_devisAdmin_Affichage.GetTotalCount(null);

				ViewBag.PageCount = (recsCount + pageSize - 1) / pageSize;
				ViewBag.CurrentPage = pg;
				return View(V_devisAdmin_Affichage.GetDevisAdminPage(null, pg, pageSize));


			}
            catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return View();
		}
		public IActionResult DetailsTravaux(int devis)
		{
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				List<V_devisDetails_affichage> listDetails = V_devisDetails_affichage.GetByDevis(null, devis);
				double montantTravaux = 0;
				double taux = Devis.GetTauxByDevis(con, devis);
				Console.WriteLine("taux finition: " + taux);
				foreach (V_devisDetails_affichage item in listDetails)
				{
					montantTravaux += item.quantite * item.prixUnitaire;
				}
				Console.WriteLine("montant travaux : " + montantTravaux);
				double total = montantTravaux + (montantTravaux * taux / 100);
				ViewBag.Total = total;
				ViewBag.Taux = taux;
				ViewBag.MontantTravaux = montantTravaux;
				ViewBag.Devis = devis;

				return View(listDetails);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return RedirectToAction("Index");
		}
		public IActionResult Dashboard(int annee = 2024)
		{
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				double total = Devis.GetMontantTotal(con);
				Console.WriteLine("total devis be: " + total);
				ViewBag.Total = total;
                double totalPaiement = Paiement.GetTotalPaiement(con);
                Console.WriteLine("total devis be: " + total);
                ViewBag.TotalPaiement = totalPaiement;
                List<double> statMois = Devis.GetMontantParAnnee(con, annee);
				ViewBag.Stats = statMois;
				return View();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return View();
		}

		public IActionResult Import()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ImportDonnee(IFormFile maisonTravaux, IFormFile devis)
		{
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				if ((maisonTravaux != null && maisonTravaux.Length > 0) || (devis != null && devis.Length > 0))
				{
					string maisonTravauxExtension = Path.GetExtension(maisonTravaux.FileName);
					string devisExtension = Path.GetExtension(devis.FileName);
					string[] allowedExtensions = { ".csv" };

					if (allowedExtensions.Contains(maisonTravauxExtension))
					{

						string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

						if (!Directory.Exists(uploadFolder))
						{
							Directory.CreateDirectory(uploadFolder);
						}

						string fileNameCSV = Path.GetFileName(maisonTravaux.FileName);
						string filePathCSV = Path.Combine(uploadFolder, fileNameCSV);

						using (var stream = new FileStream(filePathCSV, FileMode.Create))
						{
							maisonTravaux.CopyTo(stream);
						}

						List<ErrorCsv> errors = new List<ErrorCsv>();
						MaisonTravauxCsv content = new MaisonTravauxCsv();
						using (var csvReader = new CsvReader(new StreamReader(filePathCSV), true))
						{
							while (csvReader.ReadNextRecord())
							{
								try
								{
									                
									content = new MaisonTravauxCsv(csvReader.CurrentRecordIndex + 2, csvReader["type_maison"], csvReader["description"], double.Parse(csvReader["surface"]), csvReader["code_travaux"], csvReader["type_travaux"], csvReader["unité"], double.Parse(csvReader["prix_unitaire"]), double.Parse(csvReader["quantite"]), double.Parse(csvReader["duree_travaux"]));
									content.insert(con);
									Console.WriteLine(csvReader.CurrentRecordIndex + 2);
								}
								catch (Exception ex)
								{
									long numline = csvReader.CurrentRecordIndex + 2;
									ErrorCsv error = new ErrorCsv(numline.ToString(), ex.Message);
									errors.Add(error);
								}

							}
						}
						if (errors.Count > 0)
						{
							ViewBag.Errorcsv = errors;
							MaisonTravauxCsv.deleteAll(con);
							return View();
						}
					}

					if (allowedExtensions.Contains(devisExtension))
					{

						string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

						if (!Directory.Exists(uploadFolder))
						{
							Directory.CreateDirectory(uploadFolder);
						}

						string fileNameCSV = Path.GetFileName(devis.FileName);
						string filePathCSV = Path.Combine(uploadFolder, fileNameCSV);

						using (var stream = new FileStream(filePathCSV, FileMode.Create))
						{
							devis.CopyTo(stream);
						}

						List<ErrorCsv> errors = new List<ErrorCsv>();
						DevisCsv content = new DevisCsv();
						using (var csvReader = new CsvReader(new StreamReader(filePathCSV), true))
						{
							while (csvReader.ReadNextRecord())
							{
								try
								{

									content = new DevisCsv(csvReader.CurrentRecordIndex + 2, csvReader["client"].Trim(), csvReader["ref_devis"].Trim(), csvReader["type_maison"].Trim(), csvReader["finition"].Trim(), double.Parse(csvReader["taux_finition"].Trim().Replace("%", "")), DateOnly.Parse(csvReader["date_devis"].Trim()), DateOnly.Parse(csvReader["date_debut"].Trim()), csvReader["lieu"].Trim());
									content.insert(con);
									Console.WriteLine(csvReader.CurrentRecordIndex + 2);
								}
								catch (Exception ex)
								{
									long numline = csvReader.CurrentRecordIndex + 2;
									ErrorCsv error = new ErrorCsv(numline.ToString(), ex.Message);
									errors.Add(error);
								}

							}
						}
						if (errors.Count > 0)
						{
							ViewBag.Errorcsv = errors;
							DevisCsv.deleteAll(con);
							MaisonTravauxCsv.deleteAll(con);
							return View();
						}
					}

					InsertionCsv.insertMaison(null);
					InsertionCsv.insertUnite(null);
					InsertionCsv.insertTravaux(null);
					InsertionCsv.insertTravauxMaison(null);
					InsertionCsv.insertUtilisateurClient(null);
					InsertionCsv.insertFinition(null);
					InsertionCsv.insertDevis(null);
					InsertionCsv.insertDevisDetails(null);
					InsertionCsv.updateMontantDevis(null);

				}
				else
				{
					Console.WriteLine("tsisy fichier");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return RedirectToAction("Import");
		}

		[HttpPost]
		public IActionResult ImportPaiement(IFormFile paiement)
		{
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				if ((paiement != null && paiement.Length > 0))
				{
					
					string paiementExtension = Path.GetExtension(paiement.FileName);
					string[] allowedExtensions = { ".csv" };

					

					if (allowedExtensions.Contains(paiementExtension))
					{

						string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

						if (!Directory.Exists(uploadFolder))
						{
							Directory.CreateDirectory(uploadFolder);
						}

						string fileNameCSV = Path.GetFileName(paiement.FileName);
						string filePathCSV = Path.Combine(uploadFolder, fileNameCSV);

						using (var stream = new FileStream(filePathCSV, FileMode.Create))
						{
							paiement.CopyTo(stream);
						}

						List<ErrorCsv> errors = new List<ErrorCsv>();
						PaiementCsv content = new PaiementCsv();
						using (var csvReader = new CsvReader(new StreamReader(filePathCSV), true))
						{
							while (csvReader.ReadNextRecord())
							{
								try
								{
									if(Paiement.checkReference(null, csvReader["ref_paiement"].Trim()) == true || PaiementCsv.checkReference(null, csvReader["ref_paiement"].Trim()) == true) continue;

									content = new PaiementCsv(csvReader.CurrentRecordIndex + 2, csvReader["ref_devis"].Trim(), csvReader["ref_paiement"].Trim(), DateOnly.Parse(csvReader["date_paiement"].Trim()), double.Parse(csvReader["montant"].Trim()));
									content.insert(con);
									Console.WriteLine(csvReader.CurrentRecordIndex + 2);
								}
								catch (Exception ex)
								{
									long numline = csvReader.CurrentRecordIndex + 2;
									ErrorCsv error = new ErrorCsv(numline.ToString(), ex.Message);
									errors.Add(error);
								}

							}
						}
						//if (errors.Count > 0)
						//{
						//	ViewBag.Errorcsv = errors;
						//	PaiementCsv.deleteAll(con);
						//	return View();
						//}
					}

					InsertionCsv.insertPaiement(null);
					InsertionCsv.delete(null, "PaiementCsv");
				}
				else
				{
					Console.WriteLine("tsisy fichier");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return RedirectToAction("Import");
		}

		public IActionResult Reset()
		{
			

			try
			{
				InsertionCsv.delete(null, "PaiementCsv");
				InsertionCsv.delete(null, "DevisCsv");
				InsertionCsv.delete(null, "MaisonTravauxCsv");

				InsertionCsv.delete(null, "Paiement");
				InsertionCsv.delete(null, "DevisDetails");
				InsertionCsv.delete(null, "Devis");
				InsertionCsv.delete(null, "TravauxMaison");
				InsertionCsv.delete(null, "TravauxTarif");
				InsertionCsv.delete(null, "Travaux");
				InsertionCsv.delete(null, "FinitionTaux");
				InsertionCsv.delete(null, "Finition");
				InsertionCsv.delete(null, "MaisonDuree");
				InsertionCsv.delete(null, "Maison");
				InsertionCsv.delete(null, "Unite");
				InsertionCsv.delete(null, "Utilisateur where role != 'ADMIN'");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			return RedirectToAction("Index");
		}

		public IActionResult Travaux(int pg = 1)
		{
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				const int pageSize = 5;

				int recsCount = V_travaux_Affichage.GetTotalCount(null);

				ViewBag.PageCount = (recsCount + pageSize - 1) / pageSize;
				ViewBag.CurrentPage = pg;

				List<Unite> allunite = Unite.getAll(null);
				ViewBag.Unites = allunite;	

				return View(V_travaux_Affichage.GetPage(null, pg, pageSize));
				
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return RedirectToAction("Index");
		}

		public IActionResult UpdateTravaux(string refe, string nom, double prixunitaire, int unite, int idtravaux)
		{
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				V_travaux_Affichage.update(null, idtravaux, nom, refe, unite, prixunitaire);
				return RedirectToAction("Travaux");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return RedirectToAction("Index");
		}

		public IActionResult ListFinition(int pg = 1)
		{
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				const int pageSize = 5;

				int recsCount = Finition.GetTotalCount(null);

				ViewBag.PageCount = (recsCount + pageSize - 1) / pageSize;
				ViewBag.CurrentPage = pg;

				return View(Finition.GetPage(null, pg, pageSize));

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return RedirectToAction("Index");
		}

		public IActionResult UpdateFinition(double taux, int idfinition)
		{
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				Finition.update(null, idfinition, taux);
				return RedirectToAction("ListFinition");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				con.Close();
			}
			return RedirectToAction("Index");
		}
	}
}
