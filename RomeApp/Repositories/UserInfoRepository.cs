using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomeApp.Repositories
{
    public class UserInfoRepository
    {
        private Dictionary<string, string> Cache { get; } = new Dictionary<string, string>();
        public Task<string> GetTargetDeviceIdByUserIdAsync(string userId) => Task.FromResult(Cache.ContainsKey(userId) ? Cache[userId] : null);

        public Task RegistDeviceIdAsync(string userId, string deviceId)
        {
            Cache[userId] = deviceId;
            return Task.CompletedTask;
        }
    }
}
