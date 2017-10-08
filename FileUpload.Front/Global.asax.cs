using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Common;
using Common.Helpers;
using Common.Helpers.IHelpers;
using Data.DataModels;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace FileUpload.Front
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IContainer Container;

        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            SetupDependancyInjection();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void SetupDependancyInjection()
        {
            var config = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<FileDataModel>().As<IFileDataModel>().SingleInstance();
            builder.RegisterType<RabbitMQHelper>().As<IMessageQueueHelper>().SingleInstance();
            builder.RegisterType<RedisHelper>().As<ICacheHelper>().SingleInstance();
            builder.RegisterType<Log4NetHelper>().As<ILogger>().SingleInstance();
            builder.RegisterType<ApplicationConfig>().As<IApplicationConfig>().SingleInstance();
            builder.RegisterType<AzureBlobStorageHelper>().As<IFileUploadHelper>().SingleInstance();
            builder.RegisterType<AspNetUserDataModel>().As<IAspNetUserDataModel>().SingleInstance();
            builder.RegisterType<GenericHelpers>().As<IGenericHelper>().SingleInstance();
            
            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }
    }
}
