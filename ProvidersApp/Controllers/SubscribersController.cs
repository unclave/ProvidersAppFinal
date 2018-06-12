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
    public class SubscribersController : Controller
    {
        private providersEntities db = new providersEntities();

        // GET: Subscribers
        [AllowAnonymous]
        public ActionResult Index()
        {
            var subscribers = db.subscribers.Include(s => s.personal_data).Include(s => s.regions);
            return View(subscribers.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(string search)
        {
            var result = db.subscribers
                .Include(a => a.personal_data)
                .Include(a => a.regions)
                .Where(a => a.full_name.ToLower().Contains(search.Trim().ToLower()) ||
                            a.telephone_number.ToLower().Contains(search.Trim().ToLower()) ||
                            a.regions.name.ToLower().Contains(search.Trim().ToLower()))
                .ToList();
            return View(result);
        }

        // GET: Subscribers/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscribers subscribers = db.subscribers.Find(id);
            if (subscribers == null)
            {
                return HttpNotFound();
            }
            return View(subscribers);
        }

        // GET: Subscribers/Create
        public ActionResult Create()
        {
            ViewBag.id_personal_data = new SelectList(db.personal_data, "id", "address");
            ViewBag.id_region = new SelectList(db.regions, "id", "name");
            return View();
        }

        // POST: Subscribers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_personal_data,full_name,telephone_number,id_region")] subscribers subscribers)
        {
            if (ModelState.IsValid)
            {
                db.subscribers.Add(subscribers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_personal_data = new SelectList(db.personal_data, "id", "address", subscribers.id_personal_data);
            ViewBag.id_region = new SelectList(db.regions, "id", "name", subscribers.id_region);
            return View(subscribers);
        }

        // GET: Subscribers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscribers subscribers = db.subscribers.Find(id);
            if (subscribers == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_personal_data = new SelectList(db.personal_data, "id", "address", subscribers.id_personal_data);
            ViewBag.id_region = new SelectList(db.regions, "id", "name", subscribers.id_region);
            return View(subscribers);
        }

        // POST: Subscribers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_personal_data,full_name,telephone_number,id_region")] subscribers subscribers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscribers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_personal_data = new SelectList(db.personal_data, "id", "address", subscribers.id_personal_data);
            ViewBag.id_region = new SelectList(db.regions, "id", "name", subscribers.id_region);
            return View(subscribers);
        }

        // GET: Subscribers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscribers subscribers = db.subscribers.Find(id);
            if (subscribers == null)
            {
                return HttpNotFound();
            }
            return View(subscribers);
        }

        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subscribers subscribers = db.subscribers.Find(id);
            db.subscribers.Remove(subscribers);
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
