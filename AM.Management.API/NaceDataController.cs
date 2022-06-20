using System;
using _0_Framework.Application;
using AM.Application.Contracts.NaceData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class NaceDataController : ControllerBase
    {
        private readonly INaceDataApplication _naceDataApplication;

        public NaceDataController(INaceDataApplication naceDataApplication)
        {
            _naceDataApplication = naceDataApplication;
        }

        [Route("[action]")]
        [HttpPost]
        public OperationResult DeleteSingleNaceData(dynamic Id)
        {
            JObject jsonObject = JObject.Parse(Id.ToString());
            var result =
                _naceDataApplication.DeleteNaceData(Convert.ToInt32(jsonObject.First.First)).Result;
            return result;
        }
    }
}