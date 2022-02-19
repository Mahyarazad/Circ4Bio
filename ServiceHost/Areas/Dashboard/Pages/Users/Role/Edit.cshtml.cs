using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Users.Role
{
    public class EditModel : PageModel
    {
        private readonly IRoleApplication _roleApplication;
        private readonly IEnumerable<IPermissionExposer> _exposers;
        public EditRole Role;
        public List<SelectListItem> PermissionItems = new List<SelectListItem>();
        public EditModel(IRoleApplication roleApplication,
            IEnumerable<IPermissionExposer> permissionExposer)
        {
            _roleApplication = roleApplication;
            _exposers = permissionExposer;
        }

        public async void OnGet(int id)
        {
            Role = await _roleApplication.GetRole(id);
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
                        if (Role.MappedPermissions.Any(x => x.Code == permissionDetail.Code))
                            item.Selected = true;
                        PermissionItems.Add(item);
                    }
                }
            }
        }

        public IActionResult OnPost(EditRole Role)
        {
            if (!ModelState.IsValid)
            {

            }
            var result = _roleApplication.Edit(Role);
            return RedirectToPage("Index");
        }
    }
}
