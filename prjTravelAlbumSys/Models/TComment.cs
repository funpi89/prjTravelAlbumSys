using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace prjTravelAlbumSys.Models
{
    public partial class TComment
    {
        [Display(Name = "評論編號")]
        public int FCommentId { get; set; }
        [Display(Name = "照片編號")]
        public int? FAlbumId { get; set; }
        [Display(Name = "帳號")]
        public string FUid { get; set; }
        [Display(Name = "評論者")]
        public string FName { get; set; }
        [Display(Name = "評論訊息")]
        [Required(ErrorMessage = "必填")]
        public string FComment { get; set; }
        [Display(Name = "發布時間")]
        public DateTime? FReleaseTime { get; set; }
    }
}
