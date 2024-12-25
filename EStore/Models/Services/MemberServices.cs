using EStore.Models.DTOs;
using EStore.Models.EFModels;
using EStore.Models.Infra;
using EStore.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EStore.Models.Services
{
    public class MemberServices
    {
        private MemberEFRepository _repo;
        public MemberServices()
        {
            _repo = new MemberEFRepository();
        }
        public void Register(RegisterDto dto)
        {
            using (var db = new AppDbContext())
            {
                if (_repo.IsExists(dto.Account))
                {
                    throw new Exception("帳號已存在");
                }

                dto.EnctyptedPassword = HashUtility.ToSHA256(dto.Password, HashUtility.GetSalt());
                
                _repo.Create(dto);
            }
        }
    }
}