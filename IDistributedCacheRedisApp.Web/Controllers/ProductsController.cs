using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            Product product = new() { Id = 2, Name = "Kalem2", Price = 200 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:2", byteProduct);

           // await _distributedCache.SetStringAsync("product:2", jsonProduct, cacheEntryOptions);
            
            
            return View();
        }

        public IActionResult Show()
        {
            byte[] byteProduct = _distributedCache.Get("product:2");


            string jsonProduct = Encoding.UTF8.GetString(byteProduct);
            
            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.product = product;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("product:2");

            return View();
        }


        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/primex.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image",imageByte);


            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("image");



            return File(imageByte,"image/jpg");

        }
    }
}