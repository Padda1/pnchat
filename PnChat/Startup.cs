using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PnChat.Startup))]
namespace PnChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
