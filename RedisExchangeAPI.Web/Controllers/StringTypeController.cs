using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            var db = _redisService.GetDb(0);
            db.StringSet("name", "ugur cengiz");
            db.StringSet("ziyaretci", 100);


            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");
            var value2 = db.StringGetRange("name", 0, 3);
            var value3 = db.StringLength("name");

            Byte[] resimByte = default(byte[]);
            db.StringSet("resim", resimByte);

            db.StringIncrement("ziyaretci", 1);
            var count = db.StringDecrementAsync("ziyaretci", 1).Result;



            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
