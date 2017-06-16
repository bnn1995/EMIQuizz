using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Voltaire.Models;

namespace Voltaire.Controllers
{
    public class NiveauxController : Controller
    {
        private VoltaireContext db = new VoltaireContext();

        [HttpPost]
        // Post: Niveaux
        
        public ActionResult Index(int diff)
        {
            Session["diff"] = diff;
          
            List<Niveau> niveaux = db.Niveaux.Where(s => s.DifficulteId == diff).ToList();
            Session["ere"] = niveaux;
            string em = (string)Session["email"];
            List<int> v = db.ScoreNiveaus.Where(p => p.id_diff == diff & p.usr_email.Equals(em)).Select(s=>s.score_niveau).ToList();
            List<int> l = new List<int>();
            for (int i=0;i<v.Count;i++){
                if (i == 0)
                {
                    l.Add(1);
                }
                else
                {
                    if (v[i - 1] > 5)
                    {
                        l.Add(1);
                    }
                    else
                    {
                        l.Add(0);
                    }
                }
            }
            Session["list"] = l;
            Session["listDesScores"] = v;

            return View(niveaux);
        }

        public ActionResult Renit(int diff)
        {
            string em = (string)Session["email"];
            var query = (from u in db.ScoreNiveaus where u.usr_email.Equals(em) & u.id_diff == diff select u);
            int c = query.Count();
            foreach (ScoreNiveau ord in query)
            {
                ord.score_niveau = 0;
      
            }
            db.SaveChanges();
            var query2 = (from u in db.StatUsers where u.email.Equals(em) select u);
            foreach (StatUser ord in query2)
            {
                ord.score_regle = 0;

            }
            db.SaveChanges();
            List<int> l2 = new List<int>();List<int> l1 = new List<int>();
            for (int i = 0; i < c + 1; i++)
            {
                l2.Add(0);
                l1.Add(0);
            }
            l1[0] = 1;
            Session["list"] = l1;
            Session["listDesScores"] = l2;
            List<Niveau> l = Session["ere"] as List<Niveau>;
            return View("Index",l);
        }
        // GET: Niveaux/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Niveau niveau = db.Niveaux.Find(id);
            if (niveau == null)
            {
                return HttpNotFound();
            }
            return View(niveau);
        }

        // GET: Niveaux/Create
        public ActionResult Create()
        {
            ViewBag.DifficulteId = new SelectList(db.Difficultes, "id", "module");
            return View();
        }

        // POST: Niveaux/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,titre_niv,description_niv,DifficulteId")] Niveau niveau)
        {
            if (ModelState.IsValid)
            {
                db.Niveaux.Add(niveau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DifficulteId = new SelectList(db.Difficultes, "id", "module", niveau.DifficulteId);
            return View(niveau);
        }

        // GET: Niveaux/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Niveau niveau = db.Niveaux.Find(id);
            if (niveau == null)
            {
                return HttpNotFound();
            }
            ViewBag.DifficulteId = new SelectList(db.Difficultes, "id", "module", niveau.DifficulteId);
            return View(niveau);
        }

        // POST: Niveaux/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,titre_niv,description_niv,DifficulteId")] Niveau niveau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(niveau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DifficulteId = new SelectList(db.Difficultes, "id", "module", niveau.DifficulteId);
            return View(niveau);
        }

        // GET: Niveaux/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Niveau niveau = db.Niveaux.Find(id);
            if (niveau == null)
            {
                return HttpNotFound();
            }
            return View(niveau);
        }

        // POST: Niveaux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Niveau niveau = db.Niveaux.Find(id);
            db.Niveaux.Remove(niveau);
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
