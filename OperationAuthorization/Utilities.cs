using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace OperationAuthorization
{
   public static class Utilities
    {
       /// <summary>
       /// Determines whether a user is authorized for an operation.
       /// </summary>
       /// <param name="operation">A string representing the operation name.</param>
       /// <param name="operationParameters">A dictionary of parameters to match against the userAuthorizations</param>
       /// <param name="userAuthorizations">The user's authorizations</param>
       /// <returns>True if at least one userAuthorization matches the operation and operationParameters. False otherwise. </returns>
       public static bool IsUserAuthorizedForOperation(string operation, Dictionary<string, string> operationParameters, IEnumerable<IUserAuthorization> userAuthorizations)
       {
           //Get all authorizations for the specified operation
           var authDictionaries = from a in userAuthorizations
                                  where a.Operation == operation
                                  select a.AuthorizationParameters;

           return operationParameters.Select(parameter => (from a in authDictionaries
                                                            where a.ContainsKey(parameter.Key) || a.ContainsKey("*")
                                                            where a.ContainsValue(parameter.Value) || a.ContainsValue("*")
                                                            select a)).All(filteredAuths => filteredAuths.Any());
       }

       /// <summary>
       /// Pareses the Route Parameters and the Operation from the RouteData
       /// </summary>
       /// <param name="routeData">A RequestContext.RouteData instance.</param>
       /// <param name="requestContext"> </param>
       /// <param name="routeParameters">A dictionary that will be populated with the Route Parameters.</param>
       /// <param name="operation">A string that will be populated with the operation name. Operation is Controller/Action or Area/Controller/Action.</param>
       public static void ParseRouteData(RequestContext requestContext, out Dictionary<string, string> routeParameters, out NameValueCollection queryStringParameters, out string operation)
       {
           var routeData = requestContext.RouteData;

           //Get any querystring parameters
           queryStringParameters = requestContext.HttpContext.Request.QueryString;

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

    }
}
