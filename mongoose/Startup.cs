using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mongoose.Startup))]
namespace mongoose
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
