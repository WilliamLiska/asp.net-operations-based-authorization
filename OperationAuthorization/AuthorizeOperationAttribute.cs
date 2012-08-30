using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
    }
}
