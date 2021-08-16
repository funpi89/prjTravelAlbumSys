using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace prjTravelAlbumSys.Models
{
    public partial class TAlbum
    {
        [Display(Name = "照片編號")]
        public int FAlbumId { get; set; }

        [Display(Name = "帳號")]
        public string FUid { get; set; }

        [Display(Name = "分類")]
        public int? FCid { get; set; }

        [Display(Name = "主題")]
        public string FTitle { get; set; }

        [Display(Name = "描述")]
        [Required(ErrorMessage = "必填")]
        public string FDescription { get; set; }

        [Display(Name = "圖檔")]
        public string FAlbum { get; set; }

        [Display(Name = "發布時間")]
        public DateTime? FReleaseTime { get; set; }

        [Display(Name = "評論數量")]
        public int? FCommentNum { get; set; }
    }
}
