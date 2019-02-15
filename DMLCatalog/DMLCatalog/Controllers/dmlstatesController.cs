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
    public class dmlstatesController : Controller
    {
        private dmldbEntities db = new dmldbEntities();

        // GET: dmlstates
        public ActionResult Index()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            return View(db.dmlstate.ToList());
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

        // GET: dmlstates/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlstate dmlstate = db.dmlstate.Find(id);
            if (dmlstate == null)
            {
                return HttpNotFound();
            }
            return View(dmlstate);
        }

        // GET: dmlstates/Create
        public ActionResult Create()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmlstate dm = new dmlstate();
            var id =
                        (from c in db.dmlstate
                         select c.id).ToArray().LastOrDefault();
            dm.id = id + 1;
            return View(dm);
        }

        // POST: dmlstates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description")] dmlstate dmlstate)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                try { 
                db.dmlstate.Add(dmlstate);
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

            return View(dmlstate);
        }

        // GET: dmlstates/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlstate dmlstate = db.dmlstate.Find(id);
            if (dmlstate == null)
            {
                return HttpNotFound();
            }
            return View(dmlstate);
        }

        // POST: dmlstates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description")] dmlstate dmlstate)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Entry(dmlstate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dmlstate);
        }

        // GET: dmlstates/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlstate dmlstate = db.dmlstate.Find(id);
            if (dmlstate == null)
            {
                return HttpNotFound();
            }
            return View(dmlstate);
        }

        // POST: dmlstates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmlstate dmlstate = db.dmlstate.Find(id);
            db.dmlstate.Remove(dmlstate);
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
