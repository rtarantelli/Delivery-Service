# Introduction

This project can be run using Visual Studio 2017 or later, or VSCode.

Initial data, as described in the exercise, was added in memory using EF Core InMemoryDatabase.

Swagger can be used for testing, discovering models and all available services, or, if you prefer, can use a client for HTTP requests, such as Postman and SoapUI.
***
## Infos

Services have been implemented for the following objects:

>- Point - information for each point
>- Path - describes the cost and time from one point to another
>- Route - describes a set paths

## Technologies/patterns

>- RESTFul API
>- Repository pattern
>- Inversion of control and dependency injection
>- Single responsability principle

>- ASP.NET Core 2.1
>- Entity Framework Core 2.1
>- XUnit Core 2.1

## Third libraries

>- Swashbuckle.AspNetCore 3.0
>- Newtonsoft.Json 11.0
>- FluentAssertions 5.4