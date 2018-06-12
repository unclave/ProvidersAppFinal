using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using NpgsqlTypes;
using ProvidersApp.Models;
using ProvidersApp.Classes;

namespace ProvidersApp.Controllers
{
    public class AllQueriesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Query1()
        {
            List<Query1Data> result;
            using (var db = new providersEntities())
            {
                string sql = @"SELECT s.full_name, p_d.sex, p_d.birthday
FROM subscribers s
JOIN personal_data p_d ON s.id_personal_data = p_d.id
";

                result = db.Database.SqlQuery<Query1Data>(sql).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query1Result");
        }

        [HttpPost]
        public ActionResult Query2()
        {
            List<Query2Data> result;
            using (var db = new providersEntities())
            {
                string sql = @"SELECT SUM(p.payment_value) as ""summary"", s.full_name

                FROM subscribers s
                    JOIN personal_data p_d ON s.id_personal_data = p_d.id

                JOIN subscriber_tariff_list s_t_l ON s_t_l.id_subscriber = s.id

                JOIN payments p ON p.id_subscriber_tariff = s_t_l.id

                GROUP BY s.id
                ";
                result = db.Database.SqlQuery<Query2Data>(sql).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query2Result");
        }

        [HttpPost]
        public ActionResult Query3(DateTime date1, DateTime date2)
        {
            List<Query3Data> result;
            using (var db = new providersEntities())
            {
                var param = new NpgsqlParameter("@date1", date1)
                {
                    NpgsqlDbType = NpgsqlDbType.Date
                };
                var param2 = new NpgsqlParameter("@date2", date2)
                {
                    NpgsqlDbType = NpgsqlDbType.Date
                };

                string sql = @"SELECT SUM(p.payment_value) as ""summary""
FROM payments p
WHERE p.date BETWEEN @date1 AND @date2
";
                result = db.Database.SqlQuery<Query3Data>(sql, param, param2).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query3Result");
        }

        [HttpPost]
        public ActionResult Query4()
        {
            List<Query4Data> result;
            using (var db = new providersEntities())
            {
                string sql = @"SELECT DISTINCT o.full_name, r.name
FROM operators o
JOIN regions r ON o.id_region = r.id
UNION (
SELECT DISTINCT s.full_name, r.name
FROM subscribers s
JOIN regions r ON s.id_region = r.id
)
";
                result = db.Database.SqlQuery<Query4Data>(sql).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query4Result");
        }

        [HttpPost]
        public ActionResult Query5()
        {
            List<Query5Data> result;
            using (var db = new providersEntities())
            {
                string sql =
                    @"WITH minmax_prices AS ( SELECT MIN(price) as ""minp"", MAX(price) as ""maxp"" FROM tariffs )
                SELECT t.name, t.minute_number, t.sms_number, t.price
                    FROM tariffs t, minmax_prices
                    WHERE t.price = minmax_prices.minp
                    UNION(
                    SELECT t.name, t.minute_number, t.sms_number, t.price
                    FROM tariffs t, minmax_prices
                    WHERE t.price = minmax_prices.maxp
                    )";
                result = db.Database.SqlQuery<Query5Data>(sql).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query5Result");
        }

        [HttpPost]
        public ActionResult Query6()
        {
            List<Query6Data> result;
            using (var db = new providersEntities())
            {
                string sql =
                    @"SELECT DISTINCT full_name, telephone_number, address, name, minute_number, sms_number, price
FROM tariffs tar
JOIN subscriber_tariff_list stl ON stl.id_tariff = tar.id
JOIN subscribers s ON stl.id_subscriber = s.id
JOIN payments p ON p.id_subscriber_tariff = stl.id
JOIN personal_data pd ON s.id_personal_data = pd.id
";
                result = db.Database.SqlQuery<Query6Data>(sql).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query6Result");
        }

        [HttpPost]
        public ActionResult Query7(DateTime date1, DateTime date2)
        {

            var parameter = new NpgsqlParameter("@date1", date1)
            {
                NpgsqlDbType = NpgsqlDbType.Date
            };
            var parameter2 = new NpgsqlParameter("@date2", date2)
            {
                NpgsqlDbType = NpgsqlDbType.Date
            };
            List<Query7Data> result;
            using (var db = new providersEntities())
            {
                string sql = @"SELECT DISTINCT s.full_name, s.telephone_number, s_t_l.date
FROM subscribers s
JOIN subscriber_tariff_list s_t_l ON s_t_l.id_subscriber = s.id
WHERE s_t_l.date BETWEEN @date1 AND @date2
";
                result = db.Database.SqlQuery<Query7Data>(sql, parameter, parameter2).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query7Result");
        }

        [HttpPost]
        public ActionResult Query8()
        {
            List<Query8Data> result;
            using (var db = new providersEntities())
            {
                string sql = @"SELECT DISTINCT s.telephone_number, s_t_l.date
FROM subscribers s
JOIN subscriber_tariff_list s_t_l ON s_t_l.id_subscriber = s.id
";
                result = db.Database.SqlQuery<Query8Data>(sql).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query8Result");
        }

        [HttpPost]
        public ActionResult Query9()
        {
            List<Query9Data> result;
            using (var db = new providersEntities())
            {
                string sql = @"SELECT COUNT(*) as ""cnt"", t.name
FROM tariffs t
JOIN subscriber_tariff_list s_t_l ON s_t_l.id_tariff = t.id
GROUP BY t.id
";
                result = db.Database.SqlQuery<Query9Data>(sql).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query9Result");
        }

        [HttpPost]
        public ActionResult Query10(string name)
        {
            var parameter = new NpgsqlParameter("@name", name.Trim())
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            };
            List<Query10Data> result;
            using (var db = new providersEntities())
            {
                string sql = @"SELECT DISTINCT s.full_name
FROM subscribers s
JOIN subscriber_tariff_list s_t_l ON s_t_l.id_subscriber = s.id
JOIN tariffs t ON s_t_l.id_tariff = t.id
WHERE t.name = @name
";
                result = db.Database.SqlQuery<Query10Data>(sql, parameter).ToList();
            }

            TempData["result"] = result;

            return RedirectToAction("Query10Result");
        }

        public ActionResult Query1Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query2Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query3Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query4Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query5Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query6Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query7Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query8Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query9Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Query10Result()
        {
            TempData.Keep();
            return View(TempData["result"]);
        }
    }
}