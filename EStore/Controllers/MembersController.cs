using EStore.Models.EFModels;
using EStore.Models.Infra;
using EStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStore.Controllers
{
    public class MembersController : Controller
    {
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
    }
}