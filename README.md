# How to: Keycloak - ASP.NET Core - Angular

Code to demonstrate how to use Keycloak with ASP.NET, OpenAPI UI (formerly Swagger) and Angular

## Repository

The repository contains feature commits as well as branches for the following steps:

[Demo application without authentication](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/without-authentication)

[Demo application with authentication](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/authentication-only)

[Demo application with authorization via roles](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/authorization-via-roles)

[Demo application with authorization via UMA (user managed access protocol), role based](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/authorization-via-uma-role-based)

[Demo application with authorization via UMA, policy based](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/authorization-via-uma-policy-based)

[Demo application with authorization via UMA, decision API](https://github.com/fschick/Keycloak.ASPNet.Angular/tree/authorization-via-uma-decision-api)

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