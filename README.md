asp.net-operations-based-authorization
======================================

An operations-based authorization plugin for ASP.NET that is data-source and ORM agnostic.  You decide how to store the permissions; Operations-Based Authorization does the rest.

- To set up operations-based authorizations, create a class that implements IUserAuthorizationRepository.  The only method required is GetAuthorizationsForCurrentUser(), which returns an IEnumerable<IUserAuthorization>.  Register your class with OperationBasedAuthorizationSetup.UseRepository() (in Application_Start should work fine).

- To require authorization on an Action, use [AuthorizeOperation]. By default, authorization is done against all Route Parameters.  You can optionally specify which parameters to check against like this: [AuthorizeOperation("firstParameterToCheck","secondParameterToCheck")].

- Wildcards are allowed in the AuthorizationParameters (i.e. { "id", "*" }), but not yet in Actions (i.e. "Home/*"). This is coming soon.