﻿using EStore.Models.EFModels;
using EStore.Models.Infra;
using EStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EStore.Controllers
{
    public class MembersController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        // GET: Members/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVm model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                ProcessRegister(model);
                return View("RegisterConfirm");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        private void ProcessRegister(RegisterVm model)
        {
            using (var db = new AppDbContext())
            {
                if(db.Members.Any(x => x.Account == model.Account))
                {
                    throw new Exception("帳號已存在");
                }

                var hashedPassword = HashUtility.ToSHA256(model.Password, HashUtility.GetSalt());
                var member = new Member
                {
                    Account = model.Account,
                    EncryptedPassword = hashedPassword,
                    Name = model.Name,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    IsConfirmed = false,
                    ConfirmCode = Guid.NewGuid().ToString("N")
                };

                db.Members.Add(member);
                db.SaveChanges();
            }
        }

        public ActionResult ActiveRegister(int memberId, string confirmCode)
        {
            ProcessActiveRegister(memberId, confirmCode);

            return View();
        }

        private void ProcessActiveRegister(int memberId, string confirmCode)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(x => x.Id == memberId && x.ConfirmCode == confirmCode && x.IsConfirmed == false);
                if (member == null) return;

                member.IsConfirmed = true;
                member.ConfirmCode = null;

                db.SaveChanges();
            }
        }

        //GET: Members/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVm model)
        {
            if(!ModelState.IsValid) return View(model);

            try
            {
                ValidateLogin(model.Account, model.Password);
                (string returnUrl, HttpCookie cookie) = ProcessLogin(model.Account);

                Response.Cookies.Add(cookie);

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        private (string returnUrl, HttpCookie cookie) ProcessLogin(string account)
        {
            var roles = string.Empty;

            var ticket =
                new FormsAuthenticationTicket
                (
                    1,
                    account,
                    DateTime.Now,
                    DateTime.Now.AddDays(2),
                    false,
                    roles,
                    "/"
                );

            string value = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

            var url = FormsAuthentication.GetRedirectUrl(account, true);

            return (url, cookie);
        }

        private void ValidateLogin(string account, string password)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(x => x.Account == account);
                if(member == null) throw new Exception("帳號或密碼錯誤");
                if(!HashUtility.verifySHA256(password, member.EncryptedPassword)) throw new Exception("帳號或密碼錯誤");
                if (member.IsConfirmed == false) throw new Exception("帳號未開通");
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Members");
        }
    }
}