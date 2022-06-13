using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;

namespace ServiceHost.Areas.Dashboard.Pages.Nace
{
    public class CreateModel : PageModel
    {
        public CreateNace Command;
        public GetDetailList CreateCommand;

        public string Name;
        public string Value;
        public string Name2;
        public string Value2;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INaceApplication _naceApplication;


        public CreateModel(IAuthenticateHelper authenticateHelper, INaceApplication naceApplication)
        {
            _authenticateHelper = authenticateHelper;
            _naceApplication = naceApplication;
        }

        public void OnGet()
        {
            Command = new CreateNace();
            CreateCommand = new GetDetailList();
        }

        public JsonResult OnPost(CreateNace Command, GetDetailList createCommand)
        {

            Command = new CreateNace
            {
                Title = Command.Title,
                Items = new List<CreateDetail>(),
            };
            var startIndex = 0;

            for (var item = 0; item < createCommand.GroupSize.Count(); item++)
            {

                var Detail = new CreateDetail
                {
                    DetailBody = createCommand.DetailBody[item],
                    ItemDetailList = new List<string>(),

                };

                for (var itemNumber = startIndex
                    ; itemNumber < startIndex + createCommand.GroupSize[item];
                    itemNumber++)
                {

                    Detail.ItemDetailList.Add(createCommand.ItemDetailList[itemNumber]);

                }

                startIndex += createCommand.GroupSize[item];
                Console.WriteLine(startIndex);
                Command.Items.Add(Detail);
            };
            return new JsonResult(_naceApplication.CreateNace(Command));
        }

    }
}
