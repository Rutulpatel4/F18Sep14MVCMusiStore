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

        //GET: STORE/AddRToCart
        public ActionResult AddToCart(int AlbumId)
        {
            //determine cardId to group this user's item together
            string CurrentCartId;
            GetCartId();
            CurrentCartId = Session["CartId"].ToString();
            //create and save the new cart item
            Cart cart = new Cart
            {
                CartId = CurrentCartId,
                AlbumId = AlbumId,
                Count = 1,
                DateCreated = DateTime.Now
            };
            db.Carts.Add(cart);
            db.SaveChanges();

            //load the shopping cart page
            return RedirectToAction("ShoppingCart");
        }

        private void GetCartId()
        {
            //if there is no CartID session veriable(this is first item.)
            if (Session["CartId"] == null)
            {
                //is user loged in?
                if(User.Identity.IsAuthenticated)
                {
                    Session["CartId"] = User.Identity.Name;
                }
                else
                {
                    // so generate random number
                    Session["CartId"] = Guid.NewGuid().ToString();
                }
            }

        }
        //GET Storw/ShoppingCart
        public ActionResult ShoppingCart()
        {
            //check or generate cart id
            GetCartId();

            //select all the item in the current user's cart
            string CurrentCartId = Session["CartId"].ToString();
            var CartItems = db.Carts.Where(c => c.CartId == CurrentCartId).ToList();

            return View(CartItems);
        }

        //GET /Store/RemoveFromCart
        public ActionResult RemoveFromCart(int id)
        {
            //find and delete this record from user's cart
            Cart CartItem = db.Carts.SingleOrDefault(c => c.RecordId == id);
            db.Carts.Remove(CartItem);
            db.SaveChanges();

            //reload the updated cart page
            return RedirectToAction("ShoppingCart");
        }
    }
}