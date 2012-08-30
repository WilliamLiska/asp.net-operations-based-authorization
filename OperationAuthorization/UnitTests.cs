using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OperationAuthorization
{
    public class UnitTests
    {
        class UserAuthorization : IUserAuthorization
        {
            public string Operation { get; set; }
            public Dictionary<string, string> AuthorizationParameters { get; set; }

            public UserAuthorization(string operation, Dictionary<string, string> parameters )
            {
                Operation = operation;
                AuthorizationParameters = parameters;
            }
        }


        [Fact]
        public void IsAuthorized_Should_Authorize_When_User_Has_Permissions()
        {
            List<UserAuthorization> testAuthorizations = new List<UserAuthorization>();
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "id", "1" } }));
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "fakeParam", "1" }, { "id", "1" } }));
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "id", "12" } }));
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "id", "*" } }));

            var authAttribute = new AuthorizeOperationAttribute();
            var routeParameters = new Dictionary<string, string>();
            routeParameters.Add("id", "1");

            Assert.True(authAttribute.IsAuthorized("Home/POCResult", routeParameters, testAuthorizations));
        }

        [Fact]
        public void IsAuthorized_Should_Not_Authorize_When_User_Does_Not_Have_Permissions()
        {
            List<UserAuthorization> testAuthorizations = new List<UserAuthorization>();
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "id", "1" } }));
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "fakeParam", "1" }, { "id", "1" } }));
            testAuthorizations.Add(new UserAuthorization("Home/POCResult", new Dictionary<string, string>() { { "id", "12" } }));

            var authAttribute = new AuthorizeOperationAttribute();
            var routeParameters = new Dictionary<string, string>();
            routeParameters.Add("id", "13");

            Assert.False(authAttribute.IsAuthorized("Home/POCResult", routeParameters, testAuthorizations));
        }
    }
}
