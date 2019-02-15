using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DMLCatalog.Models;

namespace DMLCatalog.Controllers
{
    public class dmllocalizesController : Controller
    {
        private dmldbEntities db = new dmldbEntities();

        // GET: dmllocalizes
        public ActionResult Index()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            return View(db.dmllocalize.ToList());
        }

        public List<string> GetGroupNames(string domainName, string userName)
        {
            List<string> result = new List<string>();

            using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, domainName))
            {
                using (PrincipalSearchResult<Principal> src = UserPrincipal.FindByIdentity(principalContext, userName).GetGroups())
                {
                    src.ToList().ForEach(sr => result.Add(sr.SamAccountName));
                }
            }

            return result;
        }

        // GET: dmllocalizes/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmllocalize dmllocalize = db.dmllocalize.Find(id);
            if (dmllocalize == null)
            {
                return HttpNotFound();
            }
            return View(dmllocalize);
        }

        // GET: dmllocalizes/Create
        public ActionResult Create()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmllocalize dm = new dmllocalize();
            var id =
                        (from c in db.dmllocalize
                         select c.id).ToArray().LastOrDefault();
            dm.id = id + 1;
            return View(dm);
        }

        // POST: dmllocalizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] dmllocalize dmllocalize)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                try {
                db.dmllocalize.Add(dmllocalize);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        Response.Write("Object: " + validationError.Entry.Entity.ToString());
                        Response.Write(" ");
                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            Response.Write(err.ErrorMessage + " ");
                        }
                    }
                }
            }

            return View(dmllocalize);
        }

        // GET: dmllocalizes/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmllocalize dmllocalize = db.dmllocalize.Find(id);
            if (dmllocalize == null)
            {
                return HttpNotFound();
            }
            return View(dmllocalize);
        }

        // POST: dmllocalizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] dmllocalize dmllocalize)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Entry(dmllocalize).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dmllocalize);
        }

        // GET: dmllocalizes/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmllocalize dmllocalize = db.dmllocalize.Find(id);
            if (dmllocalize == null)
            {
                return HttpNotFound();
            }
            return View(dmllocalize);
        }

        // POST: dmllocalizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmllocalize dmllocalize = db.dmllocalize.Find(id);
            db.dmllocalize.Remove(dmllocalize);
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
