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
    public class ScoreNiveauxController : Controller
    {
        private VoltaireContext db = new VoltaireContext();

        // GET: ScoreNiveaux
        public ActionResult Index()
        {
            return View(db.ScoreNiveaus.ToList());
        }

        // GET: ScoreNiveaux/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreNiveau scoreNiveau = db.ScoreNiveaus.Find(id);
            if (scoreNiveau == null)
            {
                return HttpNotFound();
            }
            return View(scoreNiveau);
        }

        // GET: ScoreNiveaux/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScoreNiveaux/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,usr_email,id_niveau,id_diff,score_niveau")] ScoreNiveau scoreNiveau)
        {
            if (ModelState.IsValid)
            {
                db.ScoreNiveaus.Add(scoreNiveau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoreNiveau);
        }

        // GET: ScoreNiveaux/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreNiveau scoreNiveau = db.ScoreNiveaus.Find(id);
            if (scoreNiveau == null)
            {
                return HttpNotFound();
            }
            return View(scoreNiveau);
        }

        // POST: ScoreNiveaux/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,usr_email,id_niveau,id_diff,score_niveau")] ScoreNiveau scoreNiveau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoreNiveau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoreNiveau);
        }

        // GET: ScoreNiveaux/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreNiveau scoreNiveau = db.ScoreNiveaus.Find(id);
            if (scoreNiveau == null)
            {
                return HttpNotFound();
            }
            return View(scoreNiveau);
        }

        // POST: ScoreNiveaux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoreNiveau scoreNiveau = db.ScoreNiveaus.Find(id);
            db.ScoreNiveaus.Remove(scoreNiveau);
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
