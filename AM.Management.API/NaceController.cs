using System;
using System.Linq;
using AM.Application.Contracts.Nace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class NaceController : ControllerBase
    {
        private readonly INaceApplication _naceApplication;

        public NaceController(INaceApplication naceApplication)
        {
            _naceApplication = naceApplication;
        }
        [Route("[action]")]
        [HttpPost]
        public NaceViewModel GetSingleNace(dynamic Id)
        {
            JObject jsonObject = JObject.Parse(Id.ToString());
            var result =
                _naceApplication.GetSingleNace(Convert.ToInt32(jsonObject.First.First)).Result;
            result.Items = result.Items.Where(x => !x.IsDeleted).ToList();
            foreach (var item in result.Items)
            {
                item.ListItems = item.ListItems.Where(x => !x.IsDeleted).ToList();
            }
            return result;
        }
    }
}