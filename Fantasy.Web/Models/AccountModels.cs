using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Fantasy.Web.Properties;

namespace Fantasy.Web.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "LogInModel_UserName", ResourceType = typeof(Resources))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "LogInModel_Password", ResourceType = typeof(Resources))]
        public string Password { get; set; }

        [Display(Name = "LogInModel_RememberMe", ResourceType = typeof(Resources))]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    
}
