using BootFX.Common.Web.UI;
using BootFX.Common.Web;
using BootFX.Common;

namespace Mbrit.Vegas.Web.Api
{
    internal class ApiPermissionsShim : IWebPermissionsShim
    {
        // TBD...
        public bool CanLogin(IBfxUser user) => true;

        // TBD...
        public bool CheckComponentPermissions(BfxComponentBase component, IBfxUser user, IBfxSessionSource source) => true;

        // TBD...
        public void CheckContext(Endpoint ep, HttpContext context)
        {
        }
    }
}
