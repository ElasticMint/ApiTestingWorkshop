using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ElasticMint.Api.Test.Workshop.Api.Infrastructure.Routing
{
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly string _routePrefix;

        public RouteConvention(string routePrefix)
        {
            _routePrefix = routePrefix;
        }

        public void Apply(ApplicationModel application)
        {
            var centralPrefix = new AttributeRouteModel(new RouteAttribute(_routePrefix));
            foreach (var controller in application.Controllers)
            {
                var routeSelector = controller.Selectors.FirstOrDefault(x => x.AttributeRouteModel != null);

                if (routeSelector != null)
                {
                    routeSelector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                        centralPrefix,
                        routeSelector.AttributeRouteModel);
                }
                else
                {
                    controller.Selectors.Add(new SelectorModel { AttributeRouteModel = centralPrefix });
                }
            }
        }
    }
}