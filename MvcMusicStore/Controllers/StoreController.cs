using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        private MusicStoreModel db = new MusicStoreModel();
        // GET: Store
        public ActionResult Index()
        {

            var genres = db.Genres.OrderBy(g => g.Name).ToList();
            return View(genres);
        }
        //GET STORE/ALBUMS?GENRE
        public ActionResult Albums(string genre)
        {
            var albums = db.Albums.Where(a => a.Genre.Name == genre).ToList();
            return View(albums);
        }
        //GET: STORE/PRODUCT
        public ActionResult Product(String ProductName)
        {
            ViewBag.ProductName = ProductName;
            return View();
        }
    }
}