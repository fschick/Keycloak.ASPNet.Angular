# How to: Keycloak - ASP.NET Core - Angular

Code to demonstrate how to use Keycloak with ASP.NET, OpenAPI UI (formerly Swagger) and Angular

**HINT**: Although this repository demonstrates the use of UMA, it is not necessarily the best use case. In many scenarios, [role-based authorization](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/authorization-via-roles) may be perfectly adequate.

**WARNING**: Most security features are disabled for this demo. Before using Keycloak in production, your application must be [secured](https://www.keycloak.org/docs/latest/securing_apps/index.html).

## Repository

The repository contains feature commits as well as branches for the following steps:

[Demo application without authentication](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/01-authentication-none)

[Demo application with authentication](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/02-authentication-only)

[Demo application with authorization via roles](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/03-authorization-via-roles)

[Demo application with authorization via UMA (user managed access protocol), role based](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/04-authorization-via-uma-role-based)

[Demo application with authorization via UMA, policy based](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/05-authorization-via-uma-policy-based)

[Demo application with authorization via UMA, decision API](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/06-authorization-via-uma-decision-api)

## Repository updates

I will use ‘[Rewriting History](https://git-scm.com/book/en/v2/Git-Tools-Rewriting-History)’ for updates. So just get the repository every time you want the latest version. A `git pull` will normally lead to conflicts.

## Project structure

[`slides`](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/main/slides): Slide to the talk about Keycloak with ASP.NET and Angular

[`keycloak`](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/main/keycloak): Export of Keycloak realms according to the steps from above

[`snippets`](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/main/snippets): Code snippets to implement step-by-step

[ `source/backend`](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/main/source/backend): Source code for the back-end server (ASP.NET Core)

[`source/frontend`](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/main/source/frontend): Source code for the front-end (Angular)

## Resources

*An Illustrated Guide to OAuth and OpenID Connect*
https://developer.okta.com/blog/2019/10/21/illustrated-guide-to-oauth-and-oidc

*Background information to OAuth and OpenID Connect*
[OAuth 2.0](https://ldapwiki.com/wiki/Wiki.jsp?page=OAuth 2.0#top)
[OpenID Connect](https://ldapwiki.com/wiki/Wiki.jsp?page=OpenID Connect#top)

*Use Keycloak as Identity Provider in ASP.NET Core 6*
https://nikiforovall.github.io/aspnetcore/dotnet/2022/08/24/dotnet-keycloak-auth.html

*Real-World Application with Keycloak*
https://github.com/fschick/TimeTracking

*.NET / C# REST API client for the Keycloak Admin REST API*
https://github.com/fschick/Keycloak.RestApiClient