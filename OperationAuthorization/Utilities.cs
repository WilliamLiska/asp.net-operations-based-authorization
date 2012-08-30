using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
