﻿using FileUpload.Service.Hubs.ChatWebApplication.Service.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var fileHub = new FileUploadHub();

            GlobalHost.DependencyResolver.Register(typeof(FileUploadHub), () => fileHub);

            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };

                hubConfiguration.EnableDetailedErrors = true;
                map.RunSignalR(hubConfiguration);
            });

        }
    }
}
