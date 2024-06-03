using Construction.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Construction.Controllers
{
    public class LoginClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

		public IActionResult TraitementLogin(string numTel)
		{
			if (numTel == null)
			{
				TempData["ErreurClient"] = "Existence de champ vide";
				return RedirectToAction("Index");
			}
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				if (Utilisateur.checkNumTel(con, numTel.Trim()) == true)
				{
					int id = Utilisateur.getIdByNumTel(null, numTel.Trim());
					Console.WriteLine(id + " id client");
					HttpContext.Session.SetInt32("idClient", id);
					return RedirectToAction("Index", "Client");
				}
				else
				{
					Utilisateur.insertClient(con, numTel);
					int lastid = Utilisateur.getLastId(con);
					Console.WriteLine(lastid + "id farany");
					HttpContext.Session.SetInt32("idClient", lastid);
					return RedirectToAction("Index", "Client");
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
			return RedirectToAction("Index");
		}

		public IActionResult Deconnexion()
		{
			if (HttpContext.Session.GetInt32("idClient") != null) HttpContext.Session.Remove("idClient");
			return RedirectToAction("Index");
		}
	}
}
