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
    public class Subscriber_tariff_listController : Controller
    {
        private providersEntities db = new providersEntities();

        // GET: Subscriber_tariff_list
        [AllowAnonymous]
        public ActionResult Index()
        {
            var subscriber_tariff_list = db.subscriber_tariff_list.Include(s => s.subscribers).Include(s => s.tariffs);
            return View(subscriber_tariff_list.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(string search)
        {
            List<subscriber_tariff_list> result = new List<subscriber_tariff_list>();
            result = db.subscriber_tariff_list
                .Include(a => a.subscribers)
                .Include(a => a.tariffs)
                .Where(a => a.subscribers.full_name.ToLower().Contains(search.Trim().ToLower()) ||
                            a.tariffs.name.ToLower().Contains(search.Trim().ToLower()))
                .ToList();
            if (DateTime.TryParse(search, out var val))
            {
                try
                {
                    result.AddRange(db.subscriber_tariff_list
                        .Where(a => a.date == val).ToList());
                }
                catch (Exception)
                {

                }
            }

            return View(result);
        }

        // GET: Subscriber_tariff_list/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscriber_tariff_list subscriber_tariff_list = db.subscriber_tariff_list.Find(id);
            if (subscriber_tariff_list == null)
            {
                return HttpNotFound();
            }
            return View(subscriber_tariff_list);
        }

        // GET: Subscriber_tariff_list/Create
        public ActionResult Create()
        {
            ViewBag.id_subscriber = new SelectList(db.subscribers, "id", "full_name");
            ViewBag.id_tariff = new SelectList(db.tariffs, "id", "name");
            return View();
        }

        // POST: Subscriber_tariff_list/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_tariff,id_subscriber,date")] subscriber_tariff_list subscriber_tariff_list)
        {
            if (ModelState.IsValid)
            {
                db.subscriber_tariff_list.Add(subscriber_tariff_list);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_subscriber = new SelectList(db.subscribers, "id", "full_name", subscriber_tariff_list.id_subscriber);
            ViewBag.id_tariff = new SelectList(db.tariffs, "id", "name", subscriber_tariff_list.id_tariff);
            return View(subscriber_tariff_list);
        }

        // GET: Subscriber_tariff_list/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscriber_tariff_list subscriber_tariff_list = db.subscriber_tariff_list.Find(id);
            if (subscriber_tariff_list == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_subscriber = new SelectList(db.subscribers, "id", "full_name", subscriber_tariff_list.id_subscriber);
            ViewBag.id_tariff = new SelectList(db.tariffs, "id", "name", subscriber_tariff_list.id_tariff);
            return View(subscriber_tariff_list);
        }

        // POST: Subscriber_tariff_list/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_tariff,id_subscriber,date")] subscriber_tariff_list subscriber_tariff_list)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscriber_tariff_list).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_subscriber = new SelectList(db.subscribers, "id", "full_name", subscriber_tariff_list.id_subscriber);
            ViewBag.id_tariff = new SelectList(db.tariffs, "id", "name", subscriber_tariff_list.id_tariff);
            return View(subscriber_tariff_list);
        }

        // GET: Subscriber_tariff_list/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscriber_tariff_list subscriber_tariff_list = db.subscriber_tariff_list.Find(id);
            if (subscriber_tariff_list == null)
            {
                return HttpNotFound();
            }
            return View(subscriber_tariff_list);
        }

        // POST: Subscriber_tariff_list/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subscriber_tariff_list subscriber_tariff_list = db.subscriber_tariff_list.Find(id);
            db.subscriber_tariff_list.Remove(subscriber_tariff_list);
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
