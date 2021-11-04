using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssignmentTask.Startup))]
namespace AssignmentTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
