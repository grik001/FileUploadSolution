using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FileUpload.Front.Startup))]
namespace FileUpload.Front
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
