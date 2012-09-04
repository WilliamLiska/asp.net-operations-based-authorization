using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //If, for some reason, the handler isn't an Mvc handler, give control to the base
            if (httpContext.CurrentHandler.GetType() != typeof(System.Web.Mvc.MvcHandler))
            {
                base.AuthorizeCore(httpContext);
            }

            Dictionary<string, string> routeParameters;
            string operation;
            var requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
            NameValueCollection queryStringParameters;
            
            var routeData = requestContext.RouteData;

            var queryString = requestContext.HttpContext.Request.QueryString;

            //Fill in routeParameters and the operation name
            Utilities.ParseRouteData(((MvcHandler)httpContext.CurrentHandler).RequestContext, out routeParameters, out queryStringParameters, out operation);

            //Get the UserAuthorizations
            var userAuthorizations = OperationBasedAuthorizationSetup.UserAuthorizationRepository.GetAuthorizationsForCurrentUser();

            //If parameters have been supplied from the controller, authorize on those only.
            //If no parameters have been supplied, authorize on all Action parameters
            if (_authorizationParameters == null)
            {
                //No parameters have been specified for the AuthorizeAttribute; check against all
                return Utilities.IsUserAuthorizedForOperation(operation, routeParameters, userAuthorizations);
            }

            //Since parameters were specified in the constructor, match them with values from Route Parameters.
            var parametersToCheck = _authorizationParameters.Select(param => new
                                                                                 {
                                                                                     key = param,
                                                                                     value =
                                                                                 routeParameters.FirstOrDefault(
                                                                                     r => r.Key == param).Value
                                                                                 }).ToDictionary(p => p.key,
                                                                                                 p => p.value);

            var parametersWithValues = parametersToCheck.Where(p => p.Value != null);
            //Match any parameters without values to values from the querystring
            var parametersWithoutValues = parametersToCheck.Where(p => p.Value == null).Select(p => new 
                                                                                                        {
                                                                                                            key = p.Key,
                                                                                                            value = queryStringParameters[p.Key]
                                                                                                        }).ToDictionary(p => p.key,
                                                                                                                        p => p.value);
           
            var concatenatedParameters = parametersWithValues.Concat(parametersWithoutValues).ToDictionary(p => p.Key,
                                                                                                           p => p.Value);

            //Attempt to authorize
            return Utilities.IsUserAuthorizedForOperation(operation, concatenatedParameters, userAuthorizations);

        }
    }


}
