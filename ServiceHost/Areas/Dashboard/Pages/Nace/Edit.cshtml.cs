using System;
using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Nace
{
    public class EditModel : PageModel
    {
        public EditNace Command;
        public GetDetailList CreateCommand;
        public EditIndexDetailTDO IndexDetail;
        public EditIndexDetailItemsTDO IndexItemDetail;
        public string Name;
        public string Value;

        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INaceApplication _naceApplication;


        public EditModel(IAuthenticateHelper authenticateHelper, INaceApplication naceApplication)
        {
            _authenticateHelper = authenticateHelper;
            _naceApplication = naceApplication;
        }

        public IActionResult OnGet(long Id)
        {

            if (_authenticateHelper.CurrentAccountRole().RoleId == "1")
            {
                Command = _naceApplication.EditSingleNace(Id).Result;
                CreateCommand = new GetDetailList();
                IndexDetail = new EditIndexDetailTDO();
                IndexItemDetail = new EditIndexDetailItemsTDO();
                return null;
            }
            return RedirectToPage("/AccessDenied", new { area = "" });
        }

        public JsonResult OnPost(EditNace Command, EditIndexDetailTDO IndexDetail,
            EditIndexDetailItemsTDO IndexItemDetail)
        {
            Command.EditIndices = new List<EditIndexDetail>();
            if (IndexDetail.DetailBody != null)
                for (var item = 0; item < IndexDetail.DetailBody.Count; item++)
                {
                    Command.EditIndices.Add(new EditIndexDetail
                    {
                        IsDeleted = IndexDetail.IsDeleted[item],
                        DetailBody = IndexDetail.DetailBody[item],
                        IndexId = IndexDetail.IndexId[item],
                        ItemDetailList = new List<EditIndexDetailItems>()
                    });
                }

            if (IndexDetail.DetailBody != null)
                for (var index = 0; index < IndexDetail.DetailBody.Count; index++)
                {
                    if (IndexItemDetail.DetailString != null)
                        for (var item = 0; item < IndexItemDetail.DetailString.Count; item++)
                        {
                            if (Command.EditIndices[index].IndexId == IndexItemDetail.RefId[item])
                            {
                                var editIndexDetailItemsList = Command.EditIndices[index].ItemDetailList;
                                if (editIndexDetailItemsList != null)
                                    editIndexDetailItemsList.Add(new EditIndexDetailItems
                                    {
                                        IsDeleted = IndexItemDetail.IsDeleted[item],
                                        DetailString = IndexItemDetail.DetailString[item],
                                        IndexDetailId = IndexItemDetail.IndexDetailId[item],
                                        RefId = IndexItemDetail.RefId[item]
                                    });
                            }
                        }
                }

            return new JsonResult(_naceApplication.EditNace(Command));
        }

    }
}
