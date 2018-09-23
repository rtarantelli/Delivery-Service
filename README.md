[![Build Status](https://dev.azure.com/rtarantelli/DeliveryService/_apis/build/status/rtarantelli.DeliveryService)](https://dev.azure.com/rtarantelli/DeliveryService/_build/latest?definitionId=3) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

# Introduction

This project can be run using Visual Studio 2017 or later, or VSCode.

Initial data, as described in the exercise, was added in memory using EF Core InMemoryDatabase.

Swagger can be used for testing, discovering models and all available services, or, if you prefer, can use a client for HTTP requests, such as Postman and SoapUI.


# Information

Brief description of the services and their methods developed for this application example.


## Auth
generate authorization token, used to access specific methods

**POST** /api/Auth


## Paths
describes the cost and time from one point to another.

**GET** /api/Paths

**POST** /api/Paths

**GET** /api/Paths/{id}

**PUT** /api/Paths/{id}

**DELETE** /api/Paths/{id}


## Points
information for each storage point.

**GET** /api/Points

**POST** /api/Points

**GET** /api/Points/{id}

**PUT** /api/Points/{id}

**DELETE** /api/Points/{id}


## Routes
describes the set of paths, which constitute a route.

**GET** /api/Routes

**GET** /api/Routes/{origin}/{destiny}/{type}


# Models
```
Point
{
	pointId		integer($int32)
	name*		string maxLength: 1
}

Path
{
	pathId		integer($int32)
	origin*		Point
	originId	integer($int32)
	destiny*	Point
	destinyId	integer($int32)
}

Route
{
	routeId		integer($int32)
	cost*		integer($int32)
	time*		integer($int32)
	pathId*		integer($int32)
	path*		Path
}

Login
{
	username	string
	password	string
	role		string
}
```


# Technologies/patterns used

>* RESTFul API
>* Repository pattern
>* Inversion of control and dependency injection
>* Single responsability principle

>* ASP.NET Core 2.1
>* Entity Framework Core 2.1
>* XUnit Core 2.1

# Third libraries

>* Swashbuckle.AspNetCore 3.0
>* Newtonsoft.Json 11.0
>* FluentAssertions 5.4
>* coverlet.msbuild 3.2

# Issues & Contributions

If you find a bug or have a feature request, please report them at this repository's issues section. Contributions are highly welcome, however except for very small changes, kindly file an issue and let's have a discussion before you open a pull request.

# License

This project is licensed under the MIT license. See the [LICENSE](LICENSE) file for more info.