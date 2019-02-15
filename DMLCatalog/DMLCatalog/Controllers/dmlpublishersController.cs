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
    public class dmlpublishersController : Controller
    {
        private dmldbEntities db = new dmldbEntities();

        // GET: dmlpublishers
        public ActionResult Index()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            return View(db.dmlpublisher.ToList());
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

        // GET: dmlpublishers/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlpublisher dmlpublisher = db.dmlpublisher.Find(id);
            if (dmlpublisher == null)
            {
                return HttpNotFound();
            }
            return View(dmlpublisher);
        }

        // GET: dmlpublishers/Create
        public ActionResult Create()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmlpublisher dm = new dmlpublisher();
            var id =
                        (from c in db.dmlpublisher
                         select c.id).ToArray().LastOrDefault();
            dm.id = id + 1;
            return View(dm);
        }

        // POST: dmlpublishers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] dmlpublisher dmlpublisher)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                try { 
                db.dmlpublisher.Add(dmlpublisher);
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

            return View(dmlpublisher);
        }

        // GET: dmlpublishers/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlpublisher dmlpublisher = db.dmlpublisher.Find(id);
            if (dmlpublisher == null)
            {
                return HttpNotFound();
            }
            return View(dmlpublisher);
        }

        // POST: dmlpublishers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] dmlpublisher dmlpublisher)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Entry(dmlpublisher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dmlpublisher);
        }

        // GET: dmlpublishers/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlpublisher dmlpublisher = db.dmlpublisher.Find(id);
            if (dmlpublisher == null)
            {
                return HttpNotFound();
            }
            return View(dmlpublisher);
        }

        // POST: dmlpublishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmlpublisher dmlpublisher = db.dmlpublisher.Find(id);
            db.dmlpublisher.Remove(dmlpublisher);
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
