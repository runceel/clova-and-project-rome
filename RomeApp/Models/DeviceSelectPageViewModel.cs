using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomeApp.Models
{
    public class DeviceSelectPageViewModel
    {
        public string SelectedDeviceId { get; set; }
        public IEnumerable<SelectListItem> Devices { get; set; }
    }
}
