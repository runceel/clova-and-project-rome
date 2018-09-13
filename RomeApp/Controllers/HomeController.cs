using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RomeApp.Models;
using RomeApp.Repositories;

namespace RomeApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserInfoRepository UserInfoRepository { get; }

        public HomeController(UserInfoRepository userInfoRepository)
        {
            UserInfoRepository = userInfoRepository;
        }

        public async Task<IActionResult> Index()
        {
            using (var graph = new GraphRepository(await HttpContext.GetTokenAsync("access_token")))
            {
                var devices = await graph.GetDevicesAsync();
                return View(new DeviceSelectPageViewModel
                {
                    Devices = devices.Select(x => new SelectListItem
                    {
                        Text = $"{x.Name}({x.Platform})",
                        Value = x.Id,
                    }),
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(DeviceSelectPageViewModel viewModel)
        {
            using (var graph = new GraphRepository(await HttpContext.GetTokenAsync("access_token")))
            {
                var user = await graph.GetUserAsync();
                await UserInfoRepository.RegistDeviceIdAsync(user.Id, viewModel.SelectedDeviceId);
            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
