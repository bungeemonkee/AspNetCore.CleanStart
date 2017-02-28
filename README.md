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