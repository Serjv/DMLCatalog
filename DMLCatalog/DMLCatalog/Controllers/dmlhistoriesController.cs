using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DMLCatalog.Models;

namespace DMLCatalog.Controllers
{
    public class dmlhistoriesController : Controller
    {
        private dmldbEntities db = new dmldbEntities();

        // GET: dmlhistories
        public ActionResult Index()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            var dmlhistory = db.dmlhistory.Include(d => d.dmlcatalog);
            return View(dmlhistory.ToList());
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

        // GET: dmlhistories/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlhistory dmlhistory = db.dmlhistory.Find(id);
            if (dmlhistory == null)
            {
                return HttpNotFound();
            }
            return View(dmlhistory);
        }

        // GET: dmlhistories/Create
        public ActionResult Create()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            ViewBag.catalogid = new SelectList(db.dmlcatalog, "id", "name");
            return View();
        }

        // POST: dmlhistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,catalogid,datetime,owner,eventtext,paramname,paramold,paramnew")] dmlhistory dmlhistory)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.dmlhistory.Add(dmlhistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.catalogid = new SelectList(db.dmlcatalog, "id", "name", dmlhistory.catalogid);
            return View(dmlhistory);
        }

        // GET: dmlhistories/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlhistory dmlhistory = db.dmlhistory.Find(id);
            if (dmlhistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.catalogid = new SelectList(db.dmlcatalog, "id", "name", dmlhistory.catalogid);
            return View(dmlhistory);
        }

        // POST: dmlhistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,catalogid,datetime,owner,eventtext,paramname,paramold,paramnew")] dmlhistory dmlhistory)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Entry(dmlhistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.catalogid = new SelectList(db.dmlcatalog, "id", "name", dmlhistory.catalogid);
            return View(dmlhistory);
        }

        // GET: dmlhistories/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlhistory dmlhistory = db.dmlhistory.Find(id);
            if (dmlhistory == null)
            {
                return HttpNotFound();
            }
            return View(dmlhistory);
        }

        // POST: dmlhistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmlhistory dmlhistory = db.dmlhistory.Find(id);
            db.dmlhistory.Remove(dmlhistory);
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
