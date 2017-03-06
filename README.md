# AspNetCore.CleanStart

A simple library intended to clean up the start code for .NET Core MVC applications.

Currently this startup/configuration code is messy and includes anti-paterns such as:
* Configuration by convention using reflection (rather than implmentation of interfaces)
* "Fluent" configuration, aka. configuration through side-effects
* Configuration providers needing magic strings as keys

Not to mention it is genrally inconsistent and has few real patterns. This library wraps that code to provide a more consistent and more strongly-typed interface.


## Usage

The primary public api provides two classes: `AspNetCore.CleanStart.Server`, and `AspNetCore.CleanStart.Startup`.

`AspNetCore.CleanStart.Server` creates an object that can begin running the server in numerous ways. It takes a subclass of `AspNetCore.CleanStart.Startup` a a type parameter and the list of urls to bind the server to as a constructor parameter.

`AspNetCore.CleanStart.Startup` is a base class that provides a cleaner startup api. It can be used as is for a simple MVC application provided that application uses attribute routing. Alternatively, it can be used as the base class for a more advanced startup configuration.

Each of these classes provides several virtual methods that can be overridden to apply custom configuration.

## Compromises

Several compromises have been made to work around the basic .NET Core configuration mechanisms.

Primarily the pattern of using overridable `protected virtual void Configure[SOMETHING] (...)` methods, while providing a cleaner more readily understandable interface, still does not allow for true dependency injection from the first line of code.

The configuration code that is being wrapped allows for limited unit testing so there are few tests.

## Version Numbering

Each version will have two numbers: a Windows file version and a SemVer version.
The SemVer version will be derived from the Windows file version.

Versioning will follow this pattern:
* Major Version : Increased when there is a release that breaks a public api
* Minor Version : Increased for each non-api-breaking release
* Build Number : 0 for alpha versions, 1 for beta versions, 2 for release candidates, 3 for releases
* Revision : Always 0 for release versions, always 1+ for alpha, beta, rc versions to indicate the alpha/beta/rc number (alpha/beeta/rc numbers should always be two-digits)

So the following versions are equalivalent
* 1.0.0.1 => 1.0.0-alpha01
* 1.0.1.1 => 1.0.1-beta01
* 1.0.2.1 => 1.0.2-rc01
* 1.0.3.0 => 1.0.3