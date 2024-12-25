using EStore.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStore.Models.ViewModels
{
    public class ResetPasswordVm
    {
        [Display(Name = "密碼")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "{0} 長度需介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(15, ErrorMessage = "{0} 長度不可超過 {1}")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}