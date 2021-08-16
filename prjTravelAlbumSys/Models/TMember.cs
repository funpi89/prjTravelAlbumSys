using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace prjTravelAlbumSys.Models
{
    public partial class TMember
    {
        [Display(Name="帳號")]
        [Required(ErrorMessage = "必填")]
        public string FUid { get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "必填")]
        public string FPwd { get; set; }
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "必填")]
        public string FName { get; set; }
        [Display(Name = "信箱")]
        [Required(ErrorMessage = "必填")]
        [EmailAddress(ErrorMessage = "必須符合信件格式")]
        public string FMail { get; set; }
        [Display(Name = "角色")]
        public string FRole { get; set; }

    }
}
