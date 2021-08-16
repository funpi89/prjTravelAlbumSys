using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using prjTravelAlbumSys.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace prjTravelAlbumSys.Controllers
{
    public class HomeController : Controller
    {
        private TravelAlbumDbContext _context;
        public HomeController(TravelAlbumDbContext context)
        {
            _context = context;
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

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult CommentList(int AlbumId)
        {
            ViewBag.Album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);
            var comments = _context.TComments.Where(m => m.FAlbumId == AlbumId).OrderByDescending(m => m.FReleaseTime).ToList();
            return View(comments);
        }

        public IActionResult AlbumCategory(int Cid = 1)
        {
            ViewBag.CategoryName = _context.TCategories.FirstOrDefault(m => m.FCid == Cid).FCname;
            var albums = _context.TAlbums.Where(m => m.FCid == Cid).OrderByDescending(m => m.FCommentNum).ThenByDescending(m => m.FReleaseTime).ToList();
            return View(albums);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(TMember member)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Msg = "";
                try
                {
                    member.FRole = "Member";
                    _context.TMembers.Add(member);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ViewBag.Msg = "會員註冊失敗, 帳號可能重複";
                }
            }
            return View(member);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string uid, string pwd)
        {
            var member = _context.TMembers.FirstOrDefault(m => m.FUid == uid && m.FPwd == pwd);

            if(member != null)
            {
                IList<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, member.FUid),
                    new Claim(ClaimTypes.Role, member.FRole)
                };
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), authProperties);
                return RedirectToAction("Index", member.FRole);
            }
            ViewBag.Msg = "帳密錯誤,請重新檢查";
            return View();
        }

        public IActionResult NoAuthorization()
        {
            return View();
        }
    }
}
