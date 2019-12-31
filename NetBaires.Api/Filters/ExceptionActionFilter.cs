using System;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetBaires.Api.Filters
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly TelemetryClient _telemetryClient;

        public ExceptionActionFilter(
            IHostingEnvironment hostingEnvironment,
            TelemetryClient telemetryClient)
        {
            _hostingEnvironment = hostingEnvironment;
            _telemetryClient = telemetryClient;
        }

        #region Overrides of ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            Type controllerType = actionDescriptor.ControllerTypeInfo;

            var controllerBase = typeof(ControllerBase);
            var controller = typeof(Controller);

            // Api's implements ControllerBase but not Controller
            if (controllerType.IsSubclassOf(controllerBase) && !controllerType.IsSubclassOf(controller))
            {
                // Handle web api exception
            }

            // Pages implements ControllerBase and Controller
            if (controllerType.IsSubclassOf(controllerBase) && controllerType.IsSubclassOf(controller))
            {
                // Handle page exception
            }

            if (!_hostingEnvironment.IsDevelopment())
            {
                // Report exception to insights
                _telemetryClient.TrackException(context.Exception);
                _telemetryClient.Flush();
            }

            base.OnException(context);
        }

        #endregion
    }
}