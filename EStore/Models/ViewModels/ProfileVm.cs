using EStore.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStore.Models.ViewModels
{
    public class ProfileVm
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(30, ErrorMessage = "{0} 長度不可超過 {1}")]
        public string Account { get; set; }

        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(256, ErrorMessage = "{0} 長度不可超過 {1}")]
        [EmailAddress(ErrorMessage = DAHelper.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(30, ErrorMessage = "{0} 長度不可超過 {1}")]
        public string Name { get; set; }

        [Display(Name = "手機")]
        [StringLength(10, ErrorMessage = "{0} 長度不可超過 {1}")]
        public string Mobile { get; set; }
    }
}