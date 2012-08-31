using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAuthorization
{
    /// <summary>
    /// Implementations of this interface will be responsible for returning a collection of IUserAuthorizations for a given user.
    /// </summary>
    public interface IUserAuthorizationRepository
    {
        /// <summary>
        /// Returns an IEnumerable of IUserAuthorizations for the current user.
        /// </summary>
        /// <returns>An IEnumerable of IUserAuthorizations for the current user.</returns>
        IEnumerable<IUserAuthorization> GetAuthorizationsForCurrentUser();
    }
}
