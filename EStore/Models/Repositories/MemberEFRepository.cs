using EStore.Models.DTOs;
using EStore.Models.EFModels;
using EStore.Models.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EStore.Models.Repositories
{
    public class MemberEFRepository
    {
        public void Create(RegisterDto dto)
        {
            using (var db = new AppDbContext())
            {
                var member = new Member
                {
                    Account = dto.Account,
                    EncryptedPassword = dto.EnctyptedPassword,
                    Name = dto.Name,
                    Email = dto.Email,
                    Mobile = dto.Mobile,
                    IsConfirmed = false,
                    ConfirmCode = Guid.NewGuid().ToString("N")
                };

                db.Members.Add(member);
                db.SaveChanges();
            }
        }

        public bool IsExists(string account)
        {
            using (var db = new AppDbContext())
            {
                return db.Members.Any(x => x.Account == account);
            }
        }
    }
}