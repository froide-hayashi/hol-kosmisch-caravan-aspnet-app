﻿using MyWebApp.Helpers;
using MyWebApp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyWebApp.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private MyContext db = new MyContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,ProfileFileName")] User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var file = Request.Files["profile"];
            if (file != null && file.ContentLength > 0)
            {
                var path = HttpContext.Server.MapPath("~/temp/");
                FileHelper.Create(path, file);
                user.ProfileFileName = file.FileName;
            }

            user.Id = Guid.NewGuid();
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,ProfileFileName")] User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var file = Request.Files["profile"];
            if (file != null && file.ContentLength > 0 && file.FileName != user.ProfileFileName)
            {
                var path = HttpContext.Server.MapPath("~/temp/");
                FileHelper.Create(path, file);
                user.ProfileFileName = file.FileName;
            }

            db.Entry(user).State = EntityState.Modified;
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