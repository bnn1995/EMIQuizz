using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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
    public class QuestionsController : Controller
    {
        private VoltaireContext db = new VoltaireContext();



        [HttpPost]
        // Post: Questions
        public ActionResult Index(string niv)
        {

            TempData["niv"] = niv;
            List<Question> questions = db.Questions.Where(s => s.Niveau.titre_niv == niv).ToList();

            return View(questions);
        }

        public ActionResult Select(int niv)
        {
            Session["scoreniveau"] = 0;
            Session["scoreregle"] = 0;
            Session["niv"] = niv;
            var te = (from qa in db.StatUsers where qa.id_niveau== niv & qa.score_regle==0 select qa.id_regle);
            List<int> po = te.ToList();
            var qu = (from q in db.Questions where q.NiveauId == niv & po.Contains(q.id_regle) select q);
            int a = qu.ToList().Last().id;
            Session["last"] = a;
         
            var questions = qu.ToList().FirstOrDefault();
            string e = questions.enonce;
            @ViewBag.id = questions.id;

            String[] enonc = e.Split(' ');
            return View(enonc);

        }
        public ActionResult Suivant(int id, int niv)
        {
            @ViewBag.id = id;
            int difff = db.Niveaux.Where(o => o.id == niv).FirstOrDefault().DifficulteId;
            string diffe = db.Difficultes.Where(v => v.id == difff).FirstOrDefault().module;
            int maxid = db.Questions.Where(p => p.NiveauId == niv).Max(i => i.id);
            if (id > maxid)
            {
                return RedirectToAction("Index", "Niveaux", new { diff = diffe });
            }
            else
            {
                var questions = (from q in db.Questions where q.id == id && q.NiveauId == niv select q).ToList().FirstOrDefault();
                string e = questions.enonce;
                @ViewBag.id = id;
                String[] enonc = e.Split(' ');
                return View(enonc);
            }
        }

        public PartialViewResult Check(int id, string reponse)
        {

            //je selectionne id de la regle courante 
            int idRegle = db.Questions.FirstOrDefault(x => x.id == id).id_regle;
            //je selectionne nbr de question de la regle courante 
            int nbrQuestionRegle = db.Questions.Count(x => x.id_regle == idRegle);
            //je selectionne id de la premiere question de la regle courante 
            int premiereQuestIdRegleId = db.Questions.FirstOrDefault(x => x.id_regle == idRegle).id;
            @ViewBag.reg = idRegle;
            @ViewBag.next = id + 1;
            @ViewBag.count = nbrQuestionRegle;
            //je selectionne la question a checker
            var reponses = db.Questions.Where(q => q.id == id).ToList().FirstOrDefault();
            //je recupere la correction
            string r = reponses.correction;
            ViewBag.phrasecorrecte = reponses.phrasecorrecte;
            ViewBag.explication = reponses.explication;
            ViewBag.regle = reponses.regle;
            int y = (int)Session["niv"];

            int nbrQuestionNiveau = db.Questions.Count(x => x.NiveauId == y);

            //je compare la correction a la reponse de l utilisateur
            if (r.Equals(reponse))
            {
                @ViewBag.message = "bravo";
                //percentage de la question
                @ViewBag.s = 100 / nbrQuestionRegle;
                //je stocke le score de la regle courante 
                Session["scoreregle"] = (int)Session["scoreregle"] + 1;
                Session["scoreniveau"] = (int)Session["scoreniveau"] + 1;

            }
            else
            {
                @ViewBag.message = "erreur";

            }

            //je recupere l email de l utilisateur courant
            string em = (string)Session["email"];
            //je recupere le user correspondant a cet email et dont la regle est courante
            var user = (from u in db.StatUsers where u.email.Equals(em) && u.id_regle == idRegle select u).FirstOrDefault();
            var usern = (from u in db.ScoreNiveaus where u.usr_email.Equals(em) & u.id_niveau == y select u).FirstOrDefault();


            int v = (int)Session["last"];
            if (id == v)
            {
                int g = (int)Session["scoreniveau"];
                usern.score_niveau = g;
                db.SaveChanges();
                Session["scoreniveau"] = 0;


            }
            ViewBag.score = user.score_regle;
            ViewBag.email = em;
            ViewBag.i = id;
            ViewBag.p = premiereQuestIdRegleId;
            ViewBag.g = premiereQuestIdRegleId + nbrQuestionRegle - 1;
            //si on arrive a la derniere question de la regle courante et le score et de 100 alors on stock le score ds StatUser
            int a = nbrQuestionRegle + premiereQuestIdRegleId - 1;
            if (id == a)
            {

                if ((int)Session["scoreregle"] == nbrQuestionRegle)
                {

                    user.score_regle = 100;
                    db.SaveChanges();
                    Session["scoreregle"] = 0;
                } 
                else
                {
                    Session["scoreregle"] = 0;
                }

            }

            return PartialView("check");
        }

        public ActionResult ModalPopUp()
        {
            return View();
        }
        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            ViewBag.NiveauId = new SelectList(db.Niveaux, "id", "titre_niv");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,enonce,correction,regle,phrasecorrecte,NiveauId")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NiveauId = new SelectList(db.Niveaux, "id", "titre_niv", question.NiveauId);
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.NiveauId = new SelectList(db.Niveaux, "id", "titre_niv", question.NiveauId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,enonce,correction,regle,phrasecorrecte,NiveauId")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NiveauId = new SelectList(db.Niveaux, "id", "titre_niv", question.NiveauId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
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
