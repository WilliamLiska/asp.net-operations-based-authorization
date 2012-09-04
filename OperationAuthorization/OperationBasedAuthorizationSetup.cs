using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAuthorization
{
    public static class OperationBasedAuthorizationSetup
    {
        public static IUserAuthorizationRepository UserAuthorizationRepository;

        public static void UseRepository(IUserAuthorizationRepository userAuthorizationRepository)
        {
            UserAuthorizationRepository = userAuthorizationRepository;
        }
    }
}
