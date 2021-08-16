using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using prjTravelAlbumSys.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace prjTravelAlbumSys.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {
        private TravelAlbumDbContext _context;
        private string _path;

        public MemberController(TravelAlbumDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
        }

        public IActionResult Index()
        {
            var hotAlbums = _context.TAlbums
                .OrderByDescending(m => m.FCommentNum)
                .ThenByDescending(m => m.FReleaseTime)
                .Take(9)
                .ToList();
            return View(hotAlbums);
        }

        public IActionResult CommentList(int AlbumId)
        {
            ViewBag.Album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);
            var comments = _context.TComments.Where(m => m.FAlbumId == AlbumId).OrderByDescending(m => m.FReleaseTime).ToList();
            return View(comments);
        }

        [HttpPost]
        public IActionResult CommentCreate(string Comment, int AlbumId)
        {
            TComment comment = new TComment();
            comment.FAlbumId = AlbumId;
            comment.FUid = User.Identity.Name;
            comment.FName = _context.TMembers.FirstOrDefault(m => m.FUid == User.Identity.Name).FName;
            comment.FComment = Comment;
            comment.FReleaseTime = DateTime.Now;
            _context.TComments.Add(comment);

            var album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);
            album.FCommentNum += 1;
            _context.SaveChanges();
            return RedirectToAction("CommentList", new { AlbumId = AlbumId });
        }

        public IActionResult AlbumCategory(int Cid = 1)
        {
            ViewBag.CategoryName = _context.TCategories.FirstOrDefault(m => m.FCid == Cid).FCname;
            var albums = _context.TAlbums.Where(m => m.FCid == Cid).OrderByDescending(m => m.FCommentNum).ThenByDescending(m => m.FReleaseTime).ToList();
            return View(albums);
        }

        public IActionResult MemberEdit()
        {
            string uid = User.Identity.Name;
            var member = _context.TMembers.FirstOrDefault(m => m.FUid == uid);
            return View(member);
        }

        [HttpPost]
        public IActionResult MemberEdit(TMember member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string uid = User.Identity.Name;
                    var temp = _context.TMembers.FirstOrDefault(m => m.FUid == uid);
                    temp.FName = member.FName;
                    temp.FPwd = member.FPwd;
                    temp.FMail = member.FMail;
                    _context.SaveChanges();
                    ViewBag.Msg = "會員資料編輯完成";
                }
                catch (Exception ex)
                {
                    ViewBag.Msg = "會員資料無法修改,請重新檢視修改資料";
                }
            }
            return View(member);
        }

        public IActionResult AlbumUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AlbumUpload(TAlbum album, IFormFile formFile)
        {
            ViewBag.Msg = "資料無法上傳,請記得上傳照片並檢視資料";
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {

                    if (formFile.Length > 0)
                    {
                        string filename = $"{Guid.NewGuid().ToString()}.jpg";
                        string savePath = $"{_path}\\{filename}";
                        using(var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        album.FUid = User.Identity.Name;
                        album.FAlbum = filename;
                        album.FCommentNum = 0;
                        album.FReleaseTime = DateTime.Now;
                        _context.TAlbums.Add(album);
                        _context.SaveChanges();

                        return RedirectToAction("MemberAlbum");
                    }
                }
            }
            return View(album);
        }

        public IActionResult MemberAlbum()
        {
            string uid = User.Identity.Name;
            var albums = _context.TAlbums.Where(m => m.FUid == uid).OrderByDescending(m => m.FReleaseTime).ToList();
            return View(albums);
        }

        public IActionResult AlbumDelete(int AlbumId)
        {
            var album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);
            var comments = _context.TComments.Where(m => m.FAlbumId == AlbumId);

            System.IO.File.Delete($"{_path}\\{album.FAlbum}");

            _context.TAlbums.Remove(album);
            _context.TComments.RemoveRange(comments);
            _context.SaveChanges();

            return RedirectToAction("MemberAlbum");
        }

        public IActionResult CommentManager(int AlbumId)
        {
            ViewBag.Album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);
            var comments = _context.TComments.Where(m => m.FAlbumId == AlbumId).OrderByDescending(m => m.FReleaseTime).ToList();

            return View(comments);
        }

        public IActionResult CommentDelete(int CommentId)
        {
            var comment = _context.TComments.FirstOrDefault(m => m.FCommentId == CommentId);
            int? AlbumId = comment.FAlbumId;
            var album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);
            album.FCommentNum -= 1;
            _context.TComments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("CommentManager", new { AlbumId = AlbumId });
        }
    }
}
