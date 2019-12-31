using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Data;

namespace NetBaires.Api
{
    public class ApiExplorerSettingsExtendAttribute : ApiExplorerSettingsAttribute
    {
        public ApiExplorerSettingsExtendAttribute(UserRole role)
        {
            GroupName = role.ToString();
        }
        public ApiExplorerSettingsExtendAttribute(UserAnonymous role)
        {
            GroupName = role.ToString();
        }
        
        public ApiExplorerSettingsExtendAttribute(string role)
        {
            GroupName = role;
        }
        public ApiExplorerSettingsExtendAttribute(List<UserRole> roles)
        {

        }
    }
}