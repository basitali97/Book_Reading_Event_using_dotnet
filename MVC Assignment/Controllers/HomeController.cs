using common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_Assignment.Helper;
using MVC_Assignment.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        static EventAPI _api = new EventAPI();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Models.Event> eventdata = new List<Models.Event>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/event/Get");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                eventdata = JsonConvert.DeserializeObject<List<Models.Event>>(result);
            }
            return View(eventdata);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyLogin(RegisterDTO LoginData)
        {
            HttpClient client = _api.Initial();
            var postTask = client.PostAsJsonAsync<RegisterDTO>("VerifyLogin", LoginData);
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                if(LoginData.Email== "myadmin@bookevents.com")
                {
                    List<Models.Event> eventdataofAll = new List<Models.Event>();
                    HttpResponseMessage res1 = await client.GetAsync("api/event/Get");
                    if (res1.IsSuccessStatusCode)
                    {
                        var result1 = res1.Content.ReadAsStringAsync().Result;
                        eventdataofAll = JsonConvert.DeserializeObject<List<Models.Event>>(result1);
                    }
                    return View("Admin",eventdataofAll);
                }
                var userid = result.Content.ReadAsAsync<int>().Result;
                HttpContext.Session.SetInt32("UserId", userid);
                List<Models.Event> eventdata = new List<Models.Event>();
                HttpResponseMessage res = await client.GetAsync($"api/event/GetByUserId/{ userid}");
                if (res.IsSuccessStatusCode)
                {
                    var x = res.Content.ReadAsStringAsync().Result;
                    eventdata = JsonConvert.DeserializeObject<List<Models.Event>>(x);
                    return View(eventdata);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> MyEvent()
        {
            HttpClient client = _api.Initial();
            var userId = HttpContext.Session.GetInt32("UserId");
            List<Models.Event> eventdata = new List<Models.Event>();
            HttpResponseMessage res = await client.GetAsync($"api/event/GetByUserId/{ userId}");
            if (res.IsSuccessStatusCode)
            {
                var x = res.Content.ReadAsStringAsync().Result;
                eventdata = JsonConvert.DeserializeObject<List<Models.Event>>(x);
                return View(eventdata);
            }
            else
            {
                return NotFound();
            }
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO UserModel)
        {
            HttpClient client = _api.Initial();
            var postTask = client.PostAsJsonAsync<RegisterDTO>("api/event/UserData", UserModel);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> EventDetails(int id)
        {
            EventDTO eventdata = new EventDTO();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/event/EventDetails/{id}");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsAsync<EventDTO>();
                eventdata = result.Result;
                return View("EventDetails",eventdata);
            }
            else
            {
                return NotFound();
            }
            
        }

        public IActionResult CreateEvent()
        {
            return View();
        }

        public async Task<IActionResult> RegisterEvent(Event EventData)
        {
            List<Event> registerData = new List<Event>();
            HttpClient client = _api.Initial();
            EventData.UserId = HttpContext.Session.GetInt32("UserId").Value;
            var postTask = client.PostAsJsonAsync<Event>("api/event/RegisterEvent", EventData);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var res = result.Content.ReadAsStringAsync().Result;
                registerData = JsonConvert.DeserializeObject<List<Event>>(res);
                return View("VerifyLogin", registerData);
            }
            return View();
        }

        public IActionResult UpdateEvent(Event EventData)
        {
            List<Event> registerData = new List<Event>();
            HttpClient client = _api.Initial();
            EventData.UserId = HttpContext.Session.GetInt32("UserId").Value;
            var postTask = client.PostAsJsonAsync<Event>("api/event/UpdateEvent", EventData);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var res = result.Content.ReadAsStringAsync().Result;
                registerData = JsonConvert.DeserializeObject<List<Event>>(res);
                return View("VerifyLogin", registerData);
            }
            return View("VerifyLogin");
        }
        [HttpGet("EditEventDetails/{id}")]
        public async Task<IActionResult> EditEventDetails(int id)
        {
            EventDTO eventdata = new EventDTO();
            //var eventid = HttpContext.Session.GetInt32("UserId").Value;
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/event/EventDetails/{id}");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsAsync<EventDTO>();
                eventdata = result.Result;
                return View("Edit", eventdata);
            }
            else
            {
                return NotFound();
            }
        }


        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
