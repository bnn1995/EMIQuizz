using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Voltaire.Models;

namespace Voltaire.Controllers
{
    [Authorize]
    public class DifficultesController : Controller
    {
        //context de ma base de donnee
        private VoltaireContext db = new VoltaireContext();
        //context par defaut de aspnet
        private static ApplicationDbContext dbu = new ApplicationDbContext();
        //permet d acceder a la table user
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbu));

        // GET: Difficultes

        public ActionResult Index()
        {
            //utilisateur courant
            var currentUser = manager.FindById(User.Identity.GetUserId());
            Session["email"] = currentUser.Email;
            string em = currentUser.Email;
            try
            {
                // si l email n est pas trouve ds la bd on initilise les tables
                string emailuser = db.StatUsers.Where(x => x.email == em).FirstOrDefault().email;
            }
            catch (Exception e)
            {
                var te = from c in db.Questions
                    from a in db.Niveaux
                    where c.NiveauId == a.id
                    select new {Question = c, Niveau = a};
                var q = te.DistinctBy(x => x.Question.id_regle).ToList();
                var pop = te.DistinctBy(x => x.Question.NiveauId).ToList();
                
                foreach (var item in pop)
                {
                    ScoreNiveau n = new ScoreNiveau();
                   n.usr_email = em;
                   n.id_niveau = item.Question.NiveauId;
                   n.id_diff = item.Niveau.DifficulteId;
                   n.score_niveau = 0;
                   db.ScoreNiveaus.Add(n);
                   db.SaveChanges();
                }
                foreach (var item in q)
                {
              

                    StatUser u = new StatUser();
                    u.email = em;
                    u.score_regle = 0;
                    u.id_regle = item.Question.id_regle;
                    u.id_niveau = item.Question.NiveauId;
                    u.id_difficulte = item.Niveau.DifficulteId;
                    db.StatUsers.Add(u);
                    db.SaveChanges();
                }
            }

            //selectionner les difficultes 
            List<Difficulte> difficultes = db.Difficultes.ToList();

            return View(difficultes);
        }


        // GET: Difficultes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Difficulte difficulte = db.Difficultes.Find(id);
            if (difficulte == null)
            {
                return HttpNotFound();
            }
            return View(difficulte);
        }

        // GET: Difficultes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Difficultes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,module,description_diff")] Difficulte difficulte)
        {
            if (ModelState.IsValid)
            {
                db.Difficultes.Add(difficulte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(difficulte);
        }

        // GET: Difficultes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Difficulte difficulte = db.Difficultes.Find(id);
            if (difficulte == null)
            {
                return HttpNotFound();
            }
            return View(difficulte);
        }

        // POST: Difficultes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,module,description_diff")] Difficulte difficulte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(difficulte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(difficulte);
        }

        // GET: Difficultes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Difficulte difficulte = db.Difficultes.Find(id);
            if (difficulte == null)
            {
                return HttpNotFound();
            }
            return View(difficulte);
        }

        // POST: Difficultes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Difficulte difficulte = db.Difficultes.Find(id);
            db.Difficultes.Remove(difficulte);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
