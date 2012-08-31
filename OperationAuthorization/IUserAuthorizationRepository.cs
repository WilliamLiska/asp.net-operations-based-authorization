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
    /// <typeparam name="T">Represents a user. Can be of whatever type is required by the implementation of this interface.</typeparam>
    public interface IUserAuthorizationRepository<in T>
    {
        IEnumerable<IUserAuthorization> GetAuthorizationsFor(T user);
    }
}
