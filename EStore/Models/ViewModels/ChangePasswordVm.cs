using EStore.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStore.Models.ViewModels
{
    public class ChangePasswordVm
    {
        [Display(Name = "原密碼")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "{0} 長度需介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        public string OriginalPassword { get; set; }

        [Display(Name = "新密碼")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "{0} 長度需介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = DAHelper.Required)]
        [StringLength(15, ErrorMessage = "{0} 長度不可超過 {1}")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}