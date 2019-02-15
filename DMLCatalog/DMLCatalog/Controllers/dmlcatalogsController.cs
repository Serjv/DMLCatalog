using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using DMLCatalog.Models;


namespace DMLCatalog.Controllers
{
    public class dmlcatalogsController : Controller
    {
        private dmldbEntities db = new dmldbEntities();
        
        // GET: dmlcatalogs
        public ActionResult Index(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id > 0)
            {
                var name =
                        (from c in db.dmlcatalog
                         where c.typeid == id
                         select c);

                return View(name.ToList());
            }
            var dmlcatalogs = db.dmlcatalog.Include(d => d.dmllocalize).Include(d => d.dmlpublisher).Include(d => d.dmlstate).Include(d => d.dmltype);
            
            return View(dmlcatalogs.ToList());
        }

        // GET: dmlcatalogs/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlcatalog dmlcatalog = db.dmlcatalog.Find(id);
            if (dmlcatalog == null)
            {
                return HttpNotFound();
            }
            return View(dmlcatalog);
        }

        // GET: dmlcatalogs/Create
        public ActionResult Create()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmlcatalog dm = new dmlcatalog();
            var id =
                        (from c in db.dmlcatalog
                         select c.id).ToArray().LastOrDefault();
            dm.id = id + 1;
            Guid g;
            g = Guid.NewGuid();
            dm.guid = g.ToString();
            ViewBag.localizeid = new SelectList(db.dmllocalize, "id", "name");
            ViewBag.publisherid = new SelectList(db.dmlpublisher, "id", "name");
            ViewBag.stateid = new SelectList(db.dmlstate, "id", "name");
            ViewBag.typeid = new SelectList(db.dmltype, "id", "name");
            
            return View(dm);
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmlcatalog dm = new dmlcatalog();
            if (search != "")
            { 
            
            var name =
                        (from c in db.dmlcatalog
                         where c.name == search
                         select c);

            return View("Index", name.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public ActionResult Upload()
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            try {
            dmlcatalog dm = new dmlcatalog();
            if (upload != null)
            {
                // получаем имя файла
                FileAttributes attributes = System.IO.File.GetAttributes(upload.FileName);
                // сохраняем файл в папку Files в проекте
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(upload.FileName);
                
                dm.name = myFileVersionInfo.ProductName;
                dm.version = myFileVersionInfo.ProductVersion;
                dm.description = myFileVersionInfo.FileDescription;

                ViewBag.localizeid = new SelectList(db.dmllocalize, "id", "name");
                ViewBag.publisherid = new SelectList(db.dmlpublisher, "id", "name");
                ViewBag.stateid = new SelectList(db.dmlstate, "id", "name");
                ViewBag.typeid = new SelectList(db.dmltype, "id", "name");

                var id =
            (from c in db.dmlcatalog
             select c.id).ToArray().LastOrDefault();
                dm.id = id + 1;
                Guid g;
                g = Guid.NewGuid();
                dm.guid = g.ToString();
                ViewBag.path = (upload.FileName).Remove(upload.FileName.LastIndexOf('\\'));
            }
            return View("Create", dm);
            }
            catch(Exception ex)
            {
                ServiceReference1.DMLServiceClient dml = new ServiceReference1.DMLServiceClient();
                dml.Error(ex.Message);
                return View("Create");
            }
        }


        // POST: dmlcatalogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,guid,version,description,stateid,typeid,publisherid,localizeid")] dmlcatalog dmlcatalog)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            string target = @"\\market-200-040\DML\" + dmlcatalog.guid;
            if (ModelState.IsValid)
            {
                try
                {
                    ServiceReference1.DMLServiceClient dml = new ServiceReference1.DMLServiceClient();
                    dml.CreateFolder(target);
                    dmlcatalog.path = target;
                    dmlcatalog.locked = false;

                    db.dmlcatalog.Add(dmlcatalog);
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


            //выполнение в случае ошибки
            ViewBag.path = target;
            ViewBag.guid = Guid.NewGuid();
            ViewBag.localizeid = new SelectList(db.dmllocalize, "id", "name", dmlcatalog.localizeid);
            ViewBag.publisherid = new SelectList(db.dmlpublisher, "id", "name", dmlcatalog.publisherid);
            ViewBag.stateid = new SelectList(db.dmlstate, "id", "name", dmlcatalog.stateid);
            ViewBag.typeid = new SelectList(db.dmltype, "id", "name", dmlcatalog.typeid);
            return View(dmlcatalog);
        }

        // GET: dmlcatalogs/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlcatalog dmlcatalog = db.dmlcatalog.Find(id);
            if (dmlcatalog == null)
            {
                return HttpNotFound();
            }
            TempData["state"] = dmlcatalog;

            ViewBag.localizeid = new SelectList(db.dmllocalize, "id", "name", dmlcatalog.localizeid);
            ViewBag.publisherid = new SelectList(db.dmlpublisher, "id", "name", dmlcatalog.publisherid);
            ViewBag.stateid = new SelectList(db.dmlstate, "id", "name", dmlcatalog.stateid);
            ViewBag.typeid = new SelectList(db.dmltype, "id", "code", dmlcatalog.typeid);

            
            

            return View(dmlcatalog);
        }

        // POST: dmlcatalogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,guid,version,description,stateid,typeid,publisherid,localizeid,path,locked")] dmlcatalog dmlcatalog)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            //считать текущие значения
            Dictionary<int,string> st = new Dictionary<int,string>();
            st.Add(0,dmlcatalog.name);
            st.Add(1,dmlcatalog.version);
            st.Add(2,dmlcatalog.description);
            st.Add(3,dmlcatalog.stateid.ToString());
            st.Add(4,dmlcatalog.typeid.ToString());
            st.Add(5,dmlcatalog.publisherid.ToString());
            st.Add(6,dmlcatalog.localizeid.ToString());
            st.Add(7, dmlcatalog.locked.ToString());
            var state = TempData["state"] as dmlcatalog;
            Dictionary<int, string> st1 = new Dictionary<int, string>();
            st1.Add(0,state.name);
            st1.Add(1,state.version);
            st1.Add(2,state.description);
            st1.Add(3,state.stateid.ToString());
            st1.Add(4,state.typeid.ToString());
            st1.Add(5,state.publisherid.ToString());
            st1.Add(6,state.localizeid.ToString());
            st1.Add(7,state.locked.ToString());
            var list3 = st.Except(st1).ToList();
            List<string> param = new List<string>();
            param.Add("name");
            param.Add("version");
            param.Add("description");
            param.Add("stateid");
            param.Add("typeid");
            param.Add("publisherid");
            param.Add("localizid");
            param.Add("locked");
            


            foreach (KeyValuePair<int, string> s in list3)
            {
                var id =
            (from c in db.dmlhistory
             select c.id).ToArray().LastOrDefault();
                dmlhistory dh = new dmlhistory();
                dh.id = id + 1;
                dh.paramname = param[s.Key];
                dh.paramnew = s.Value;
                dh.paramold = st1[s.Key];
                dh.eventtext = "Изменился параметр " + param[s.Key];
                dh.datetime = DateTime.Now;
                dh.catalogid = dmlcatalog.id;
                dh.owner = User.Identity.Name;
                db.dmlhistory.Add(dh);
            }
            db.SaveChanges();
            
            if (dmlcatalog.locked == true)
            {
                ServiceReference1.DMLServiceClient dml = new ServiceReference1.DMLServiceClient();
                dml.EditAccess(dmlcatalog.path);
            }

            if (ModelState.IsValid)
            {
                db.Entry(dmlcatalog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.localizeid = new SelectList(db.dmllocalize, "id", "name", dmlcatalog.localizeid);
            ViewBag.publisherid = new SelectList(db.dmlpublisher, "id", "name", dmlcatalog.publisherid);
            ViewBag.stateid = new SelectList(db.dmlstate, "id", "name", dmlcatalog.stateid);
            ViewBag.typeid = new SelectList(db.dmltype, "id", "code", dmlcatalog.typeid);
            return View(dmlcatalog);
        }



        // GET: dmlcatalogs/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dmlcatalog dmlcatalog = db.dmlcatalog.Find(id);
            if (dmlcatalog == null)
            {
                return HttpNotFound();
            }
            return View(dmlcatalog);
        }

        // POST: dmlcatalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.groups = GetGroupNames("atbmarket", User.Identity.Name);
            dmlcatalog dmlcatalog = db.dmlcatalog.Find(id);
            db.dmlcatalog.Remove(dmlcatalog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
