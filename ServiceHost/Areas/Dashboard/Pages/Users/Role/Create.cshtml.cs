using System.Collections.Generic;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Users.Role
{
    public class CreateModel : PageModel
    {
        public CreateRole Command;
        public List<SelectListItem> PermissionItems = new List<SelectListItem>();
        private readonly IRoleApplication _roleApplication;
        private readonly IEnumerable<IPermissionExposer> _exposers;
        public CreateModel(IRoleApplication roleApplication,
            IEnumerable<IPermissionExposer> permissionExposer)
        {
            _roleApplication = roleApplication;
            _exposers = permissionExposer;
        }

        public void OnGet()
        {
            var permissions = new List<PermissionDTO>();
            foreach (var exposer in _exposers)
            {
                var exposedPermission = exposer.Exposer();
                foreach (var eachPermission in exposedPermission)
                {
                    var graoupName = new SelectListGroup
                    {
                        Name = eachPermission.Key
                    };
                    foreach (var permissionDetail in eachPermission.Value)
                    {
                        var item = new SelectListItem(permissionDetail.Name, permissionDetail.Code.ToString())
                        {
                            Group = graoupName
                        };
                        PermissionItems.Add(item);
                    }
                }
            }
        }

        public IActionResult OnPost(CreateRole command)
        {

            var result = _roleApplication.Create(command);
            return RedirectToPage("Index");

        }
    }
}
