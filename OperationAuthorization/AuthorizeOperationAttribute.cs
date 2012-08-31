using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;

namespace OperationAuthorization
{
    /// <summary>
    /// Provides operations-based authorization for ASP.NET applications.
    /// </summary>
    public class AuthorizeOperationAttribute : AuthorizeAttribute
    {
        #region Properties

        private readonly IList<string> _authorizationParameters;
        private readonly IUserAuthorizationRepository _userAuthorizationRepository; 

        #endregion

        #region Constructors

        /// <summary>
        /// When no paramaters are supplied, users will be validatad against all Route Parameters
        /// </summary>
        public AuthorizeOperationAttribute()
        {
            
        }

        /// <summary>
        /// When parameters are supplied, users will be validated against them (and only them)
        /// </summary>
        /// <param name="authorizationParameters"></param>
        public AuthorizeOperationAttribute(params string[] authorizationParameters)
        {
            _authorizationParameters = authorizationParameters;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Pareses the Route Parameters and the Operation from the RouteData
        /// </summary>
        /// <param name="routeData">A RequestContext.RouteData instance.</param>
        /// <param name="routeParameters">A dictionary that will be populated with the Route Parameters.</param>
        /// <param name="operation">A string that will be populated with the operation name. Operation is Controller/Action or Area/Controller/Action.</param>
        public static void ParseRouteData(RouteData routeData, out Dictionary<string, string> routeParameters, out string operation)
        {
            //Get only the parameters from the RouteData, excluding the controller and action
            routeParameters = routeData.Values.Where(routeParameter => routeParameter.Key != "controller" && routeParameter.Key != "action").ToDictionary(routeParameter => routeParameter.Key, routeParameter => routeParameter.Value.ToString());

            var requestedArea = routeData.DataTokens["area"];
            var requestedController = routeData.Values["controller"];
            var requestedAction = routeData.Values["action"];

            if (requestedArea != null)
            {
                operation = requestedArea + @"/" + requestedController + @"/" + requestedAction;
            }
            else
            {
                operation = requestedController + @"/" + requestedAction;
            }
        }

        #endregion

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //If, for some reason, the handler isn't an Mvc handler, give control to the base
            if (httpContext.CurrentHandler.GetType() != typeof(System.Web.Mvc.MvcHandler))
            {
                base.AuthorizeCore(httpContext);
            }

            Dictionary<string, string> routeParameters;
            string operation;

            ParseRouteData(((MvcHandler)httpContext.CurrentHandler).RequestContext.RouteData, out routeParameters, out operation);

        }
    }


}
