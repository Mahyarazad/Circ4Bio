using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Nace
{
    public class CreateModel : PageModel
    {
        public CreateNace Command;
        public GetDetailList CreateCommand;

        public string Name;
        public string Value;

        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INaceApplication _naceApplication;


        public CreateModel(IAuthenticateHelper authenticateHelper, INaceApplication naceApplication)
        {
            _authenticateHelper = authenticateHelper;
            _naceApplication = naceApplication;
        }

        public IActionResult OnGet()
        {
            Command = new CreateNace();
            CreateCommand = new GetDetailList();
            return null;
        }

        [RequirePermission(UserPermission.RegisterNace)]
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
