using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProvidersApp.Models;

namespace ProvidersApp.Controllers
{
    [Authorize]
    public class OperatorsController : Controller
    {
        private providersEntities db = new providersEntities();

        // GET: Operators
        [AllowAnonymous]
        public ActionResult Index()
        {
            var operators = db.operators.Include(o => o.regions);
            return View(operators.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(string search)
        {
            var result = db.operators
                .Include(a => a.regions)
                .Where(a => a.full_name.ToLower().Contains(search.Trim().ToLower()) || a.experience.ToString().ToLower().Contains(search.Trim().ToLower()) || a.salary.ToString().ToLower().Contains(search.Trim().ToLower()) || a.regions.name.ToLower().Contains(search.Trim().ToLower()))
                .ToList();
            return View(result);
        }

        // GET: Operators/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            operators operators = db.operators.Find(id);
            if (operators == null)
            {
                return HttpNotFound();
            }
            return View(operators);
        }

        // GET: Operators/Create
        public ActionResult Create()
        {
            ViewBag.id_region = new SelectList(db.regions, "id", "name");
            return View();
        }

        // POST: Operators/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,full_name,experience,salary,id_region")] operators operators)
        {
            if (ModelState.IsValid)
            {
                db.operators.Add(operators);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_region = new SelectList(db.regions, "id", "name", operators.id_region);
            return View(operators);
        }

        // GET: Operators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            operators operators = db.operators.Find(id);
            if (operators == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_region = new SelectList(db.regions, "id", "name", operators.id_region);
            return View(operators);
        }

        // POST: Operators/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,full_name,experience,salary,id_region")] operators operators)
        {
            if (ModelState.IsValid)
            {
                db.Entry(operators).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_region = new SelectList(db.regions, "id", "name", operators.id_region);
            return View(operators);
        }

        // GET: Operators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            operators operators = db.operators.Find(id);
            if (operators == null)
            {
                return HttpNotFound();
            }
            return View(operators);
        }

        // POST: Operators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            operators operators = db.operators.Find(id);
            db.operators.Remove(operators);
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
