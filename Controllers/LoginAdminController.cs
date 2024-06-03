using Construction.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Construction.Controllers
{
    public class LoginAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

		public IActionResult TraitementLogin(string email, string password)
		{
			if (email == null || password == null)
			{
				TempData["ErreurAdmin"] = "Existence de champ vide";
				return RedirectToAction("Index");
			}
			NpgsqlConnection con = Connexion.getConnection();

			try
			{
				if (Utilisateur.checkLogIn(con, email.Trim(), Utilisateur.GetHashSha256(password)) == true)
				{
					int id = Utilisateur.getIdByEmailPassword(null, email, Utilisateur.GetHashSha256(password));
					Console.WriteLine(id);
					HttpContext.Session.SetInt32("idAdmin", id);
					return RedirectToAction("Index", "Admin");
				}
				else
				{
					TempData["ErreurAdmin"] = "Email ou mot de passe incorrect";
					return RedirectToAction("Index");
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
			if (HttpContext.Session.GetInt32("idAdmin") != null) HttpContext.Session.Remove("idAdmin");
			return RedirectToAction("Index");
		}
	}
}
