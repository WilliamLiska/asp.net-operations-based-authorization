using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAuthorization
{
    class TestUserAuthorizationRepository : IUserAuthorizationRepository
    {
        class UserAuthorization : IUserAuthorization
        {
            public string Operation { get; set; }
            public Dictionary<string, string> AuthorizationParameters { get; set; }

            public UserAuthorization(string operation, Dictionary<string, string> parameters)
            {
                Operation = operation;
                AuthorizationParameters = parameters;
            }
        }

        public IEnumerable<IUserAuthorization> GetAuthorizationsForCurrentUser()
        {
            List<UserAuthorization> testAuthorizations = new List<UserAuthorization>();
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "id", "1" } }));
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "fakeParam", "1" }, { "id", "1" } }));
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "id", "12" } }));
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "id", "*" } }));

            return testAuthorizations;
        }
    }
}
