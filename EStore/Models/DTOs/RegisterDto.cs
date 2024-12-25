using EStore.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStore.Models.DTOs
{
    public class RegisterDto
    {
        public string Account { get; set; }

        public string Password { get; set; }

        public string EnctyptedPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }
    }
}