using CEK.CSharp;
using CEK.CSharp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RomeApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RomeApp.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ClovaController : Controller
    {
        private static Dictionary<string, string> SchemeMap { get; } = new Dictionary<string, string>
        {
            ["ツイッターアプリ"] = "https://twitter.com/okazuki",
            ["twitterアプリ"] = "https://twitter.com/okazuki",
            ["設定アプリ"] = "ms-settings:",
            ["地図アプリ"] = "bingmaps:?cp=40.726966~-74.006076",
        };
        private UserInfoRepository UserInfoRepository { get; }

        public ClovaController(UserInfoRepository userInfoRepository)
        {
            UserInfoRepository = userInfoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var client = new ClovaClient();
            var cekRequest = await client.GetRequest(
                Request.Headers["SignatureCEK"],
                Request.Body);

            var cekResponse = new CEKResponse();

            switch (cekRequest.Request.Type)
            {
                case RequestType.LaunchRequest:
                    {
                        cekResponse.AddText("スキル設定画面で設定したパソコンのアプリケーションを起動します。");
                        cekResponse.ShouldEndSession = false;
                        break;
                    }
                case RequestType.IntentRequest:
                    {
                        switch (cekRequest.Request.Intent.Name)
                        {
                            case "LaunchAppIntent":
                                var name = cekRequest.Request.Intent.Slots["name"];
                                if (SchemeMap.ContainsKey(name.Value))
                                {
                                    var ignore = InvokeAppAsync(cekRequest.Session.User.AccessToken, SchemeMap[name.Value]);
                                    cekResponse.AddText($"{name.Value}を起動しました。");
                                }
                                break;
                        }
                        break;
                    }
                case RequestType.SessionEndedRequest:
                    {
                        break;
                    }
            }

            return new OkObjectResult(cekResponse);
        }

        private async Task InvokeAppAsync(string accessToken, string scheme)
        {
            try
            {
                using (var graph = new GraphRepository(accessToken))
                {
                    var user = await graph.GetUserAsync();
                    var targetDeviceId = await UserInfoRepository.GetTargetDeviceIdByUserIdAsync(user.Id);
                    await graph.AddLaunchAppRequestAsync(targetDeviceId, scheme);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
