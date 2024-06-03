using Construction.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Rotativa.AspNetCore;
using System.Collections.Generic;

namespace Construction.Controllers
{
	public class ClientController : Controller
	{
		public IActionResult Index(int pg = 1)
		{
			if(HttpContext.Session.GetInt32("idClient") == null) return RedirectToAction("Index", "LoginClient");
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				
				const int pageSize = 5;
				int idClient = HttpContext.Session.GetInt32("idClient") ?? 0;
				Console.WriteLine("idUtilisateur: " + idClient);
				int recsCount = Devis.GetMyTotalCount(con, idClient);

				ViewBag.PageCount = (recsCount + pageSize - 1) / pageSize;
				ViewBag.CurrentPage = pg;
				return View(Devis.GetMyDevisPage(con, pg, pageSize, idClient));
				

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
		public IActionResult TraitementPaiement(DateTime datePaiement, double montant, int devis)
		{
            if (HttpContext.Session.GetInt32("idClient") == null) return RedirectToAction("Index", "LoginClient");
            if (datePaiement < DateTime.Now)
			{
				TempData["ErreurPaiement"] = "Impossible de payer avec une date inférieure à aujourd'hui";
				return RedirectToAction("Index");
			}
			if (montant < 0)
			{
				TempData["ErreurPaiement"] = "Le montant ne peut pas être négatif";
				return RedirectToAction("Index");
			}

			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				double reste = V_devisAdmin_Affichage.GetResteAPayerById(null, devis);

				if (reste > 0) return Json(new { errors = "Le montant inseré est supérieur au resta à payer" });
				Paiement paie = new Paiement(0, montant, devis, datePaiement, "");
				paie.insert(con);

				string redirectUrl = Url.Action("Index", "Client");
				return Json(new { redirectUrl });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
				string redirectUrl = Url.Action("Index", "Client");
				return Json(new { redirectUrl });
			}
			finally
			{
				con.Close();
			}
			
			return RedirectToAction("Index");
		}
		public IActionResult GeneratePDF(int devis)
		{
            if (HttpContext.Session.GetInt32("idClient") == null) return RedirectToAction("Index", "LoginClient");
            NpgsqlConnection con = Connexion.getConnection();

			try
			{
				List<Paiement> paiements = Paiement.GetPaiementbydevis(null, devis);
				double totalpaye = Paiement.GetTotalPaiementbyDevis(null, devis);


                List<V_devisDetails_affichage> listDetails = V_devisDetails_affichage.GetByDevis(con, devis);
				double montantTravaux = 0;
				double taux = Devis.GetTauxByDevis(con, devis);
				Console.WriteLine("taux finition: " + taux);
                foreach (V_devisDetails_affichage item in listDetails)
                {
					montantTravaux += item.quantite * item.prixUnitaire;
                }
				Console.WriteLine("montant travaux : " + montantTravaux);
				double total = montantTravaux + (montantTravaux * taux / 100);
				Tuple<List<V_devisDetails_affichage>, double, double, double, int, List<Paiement>, double> info = new Tuple<List<V_devisDetails_affichage>, double, double, double, int, List<Paiement>, double>(listDetails, montantTravaux, taux, total, devis, paiements, totalpaye);
				return new ViewAsPdf("~/Views/Client/Pdf.cshtml", info)
				{
					CustomSwitches = "--disable-smart-shrinking"
				};
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
		public IActionResult CreationDevis()
		{
            if (HttpContext.Session.GetInt32("idClient") == null) return RedirectToAction("Index", "LoginClient");
            NpgsqlConnection con = Connexion.getConnection();

            try
            {
                List<V_maison_Affichage> listmaisons = V_maison_Affichage.GetAll(null);
				ViewBag.Maisons = listmaisons;
                List<V_finitionRecent> listfinitions = V_finitionRecent.GetAll(null);
				ViewBag.Finitions = listfinitions;
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

        public IActionResult TraitementDevis(int maison, int finition, DateTime debutTravaux, string lieu)
        {
            if (HttpContext.Session.GetInt32("idClient") == null) return RedirectToAction("Index", "LoginClient");
            NpgsqlConnection con = Connexion.getConnection();

            try
            {
                int client = HttpContext.Session.GetInt32("idClient") ?? 0;
				string nommaison = V_maison_Affichage.GetNomById(con, maison);
				double montant = V_maison_Affichage.GetMontantByIdMaison(con, maison);
				double taux = V_finitionRecent.GetTauxbyIdFinition(con, finition);
				string nomfinition = V_finitionRecent.GetNombyIdFinition(con, finition);

				Devis dv = new Devis(0, "Dxx1", client, maison, nommaison, montant, taux, nomfinition, debutTravaux, DateOnly.FromDateTime(DateTime.Now), lieu);
				dv.insert(con);

				int iddevis = Devis.GetLastId(con);
				List<Tuple<int, double>> alltravaux = V_maison_Affichage.GetIdTravauxAndQuantiteByIdMaison(con, maison);
                foreach (var item in alltravaux)
                {
					V_devisDetails_affichage.insertDetailsDevis(con, iddevis, item.Item1, item.Item2, V_maison_Affichage.GetPuByIdTravaux(null, item.Item1));
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
            return RedirectToAction("CreationDevis");
        }

    }
}
