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
    public class PaymentsController : Controller
    {
        private providersEntities db = new providersEntities();

        // GET: Payments
        [AllowAnonymous]
        public ActionResult Index()
        {
            var payments = db.payments.Include(p => p.subscriber_tariff_list);
            return View(payments.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(string search)
        {
            List<payments> result = new List<payments>();
            result = db.payments
                .Include(a => a.subscriber_tariff_list)
                .Where(a => a.payment_value.ToString().ToLower().Contains(search.Trim().ToLower()) ||
                            a.subscriber_tariff_list.subscribers.full_name.ToLower().Contains(search.Trim().ToLower()))
                .ToList();
            if (DateTime.TryParse(search, out var val))
            {
                try
                {
                    result.AddRange(db.payments
                        .Include(a => a.subscriber_tariff_list)
                        .Where(a => a.date == val)
                        .ToList());
                }
                catch (Exception)
                {

                }
            }
            return View(result);
        }

        // GET: Payments/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payments payments = db.payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.id_subscriber_tariff = new SelectList(db.subscriber_tariff_list, "id", "id");
            return View();
        }

        // POST: Payments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,date,payment_value,id_subscriber_tariff")] payments payments)
        {
            if (ModelState.IsValid)
            {
                db.payments.Add(payments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_subscriber_tariff = new SelectList(db.subscriber_tariff_list, "id", "id", payments.id_subscriber_tariff);
            return View(payments);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payments payments = db.payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_subscriber_tariff = new SelectList(db.subscriber_tariff_list, "id", "id", payments.id_subscriber_tariff);
            return View(payments);
        }

        // POST: Payments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,date,payment_value,id_subscriber_tariff")] payments payments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_subscriber_tariff = new SelectList(db.subscriber_tariff_list, "id", "id", payments.id_subscriber_tariff);
            return View(payments);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payments payments = db.payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            payments payments = db.payments.Find(id);
            db.payments.Remove(payments);
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
