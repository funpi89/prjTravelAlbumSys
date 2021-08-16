using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using prjTravelAlbumSys.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace prjTravelAlbumSys.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private TravelAlbumDbContext _context;
        private string _path;

        public AdminController(TravelAlbumDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
        }
        public IActionResult Index()
        {
            var members = _context.TMembers.Where(m => m.FRole == "Member").ToList();
            return View(members);
        }

        public IActionResult MemberDelete(string Uid)
        {
            var member = _context.TMembers.FirstOrDefault(m => m.FUid == Uid);
            var albums = _context.TAlbums.Where(m => m.FUid == Uid);
            var comments = _context.TComments.Where(m => m.FUid == Uid);

            foreach(var item in albums)
            {
                System.IO.File.Delete($"{_path}\\{item.FAlbum}");
            }

            _context.TAlbums.RemoveRange(albums);
            _context.TComments.RemoveRange(comments);
            _context.TMembers.Remove(member);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult MemberAlbum(string Uid)
        {
            var member = _context.TMembers.FirstOrDefault(m => m.FUid == Uid);
            ViewBag.MemberInfo = $"{member.FUid}({member.FName})";

            var albums = _context.TAlbums.Where(m => m.FUid == Uid).OrderByDescending(m => m.FCommentNum).ThenByDescending(m => m.FReleaseTime).ToList();
            return View(albums);
        }

        public IActionResult AlbumDeleteFromMember(int AlbumId)
        {
            var album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);
            var comments = _context.TComments.Where(m => m.FAlbumId == AlbumId);

            System.IO.File.Delete($"{_path}\\{album.FAlbum}");
            var Uid = album.FUid;
            _context.TAlbums.Remove(album);
            _context.TComments.RemoveRange(comments);
            _context.SaveChanges();

            return RedirectToAction("MemberAlbum", new { Uid = Uid });

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

        public IActionResult AlbumCategory(int Cid=1)
        {
            ViewBag.CategoryName = _context.TCategories.FirstOrDefault(m => m.FCid == Cid).FCname;
            var albums = _context.TAlbums.Where(m => m.FCid == Cid).OrderByDescending(m => m.FCommentNum).ThenByDescending(m => m.FReleaseTime).ToList();
            return View(albums);
        }

        public IActionResult AlbumDelete(int AlbumId)
        {
            var album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);
            var comments = _context.TComments.Where(m => m.FAlbumId == AlbumId);

            System.IO.File.Delete($"{_path}\\{album.FAlbum}");
            var Cid = album.FCid;
            _context.TAlbums.Remove(album);
            _context.TComments.RemoveRange(comments);
            _context.SaveChanges();

            return RedirectToAction("AlbumCategory", new { Cid = Cid });
        }
    }
}
