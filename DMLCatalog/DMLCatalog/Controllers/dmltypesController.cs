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
    public class dmltypesController : Controller
    {
        private dmldbEntities db = new dmldbEntities();

        // GET: dmltypes
        public ActionResult Index()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            return View(db.dmltype.ToList());
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

        // GET: dmltypes/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmltype dmltype = db.dmltype.Find(id);
            if (dmltype == null)
            {
                return HttpNotFound();
            }
            return View(dmltype);
        }

        // GET: dmltypes/Create
        public ActionResult Create()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmltype dm = new dmltype();
            var id =
                        (from c in db.dmltype
                         select c.id).ToArray().LastOrDefault();
            dm.id = id + 1;
            return View(dm);
        }

        // POST: dmltypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,code,name,description")] dmltype dmltype)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                try { 
                db.dmltype.Add(dmltype);
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

            return View(dmltype);
        }

        // GET: dmltypes/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmltype dmltype = db.dmltype.Find(id);
            if (dmltype == null)
            {
                return HttpNotFound();
            }
            return View(dmltype);
        }

        // POST: dmltypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,code,name,description")] dmltype dmltype)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Entry(dmltype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dmltype);
        }

        // GET: dmltypes/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmltype dmltype = db.dmltype.Find(id);
            if (dmltype == null)
            {
                return HttpNotFound();
            }
            return View(dmltype);
        }

        // POST: dmltypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmltype dmltype = db.dmltype.Find(id);
            db.dmltype.Remove(dmltype);
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
