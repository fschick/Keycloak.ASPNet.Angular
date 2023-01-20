# How to: Keycloak - ASP.NET Core - Angular

Code to demonstrate how to use Keycloak with ASP.NET (including Swagger / OpenAPI UI) and Angular

## Repository

The repository contains feature commits as well as branches for the following steps:

`demo_without_authentication`: Demo application without authentication

`demo_with_authentication`: Demo application with authentication

`demo_with_authorization_roles`: Demo application with authorization via roles

`demo_with_authorization_uma`: Demo application with authorization via UMA (user managed access protocol)

## Project structure

`keycloak`: Export of Keycloak realms according to the steps from above

 `slides`: Slide to the talk about Keycloak with ASP.NET and Angular

 `source/api`: Source code for the back-end server (ASP.NET Core)

`source/frontend`: Source code for the front-end (Angular)

## Keycloak Admin REST API client

.NET / C# REST API client for the Keycloak Admin REST API

https://github.com/fschick/Keycloak.RestApiClient

## Resources

*An Illustrated Guide to OAuth and OpenID Connect*
https://developer.okta.com/blog/2019/10/21/illustrated-guide-to-oauth-and-oidc

*Background information to OAuth and OpenID Connect*
[https://ldapwiki.com/wiki/OAuth 2.0 ](https://ldapwiki.com/wiki/OAuth 2.0)
[https://ldapwiki.com/wiki/OpenID Connect](https://ldapwiki.com/wiki/OpenID Connect)

*Use Keycloak as Identity Provider in ASP.NET Core 6*
https://nikiforovall.github.io/aspnetcore/dotnet/2022/08/24/dotnet-keycloak-auth.html

*Deep Dive in ASP.NET Core Authorization mit Keycloak*
https://www.thinktecture.com/webinare/deep-dive-keycloak-authorization/



