# SakeExtensions
A generic set of useful commands (MSBuild/DNX) for the [Sake](https://github.com/sakeproject/sake) build system.

# Why?
With the creation of ASP.NET 5 and the new DNX runtime, there is now a need for a cross platform approach of building these projects. The ASP.NET has developed what they call "KoreBuild" that solves this, but it is very opinionated and specific to the libraries of the .NET stack (EntityFramework, MVC, etc). Instead, SakeExtensions makes no assumptions about your build workflow, and gives you full access to every command needed to build and deploy DNX projects.

My intentions for this project is to support more than just DNX-specific commands. This repository will server as a base for other common stuff (gulp/node/bower/etc) that a .NET developer would commonly need to build/deploy their applications.

# Getting Started
It is super easy to get started. Copy the contents of the [build-template](https://github.com/theonlylawislove/SakeExtensions/tree/master/build-template) folder to the root of your project. These files should be at your root.

* ```Nuget.Config``` and ```Nuget.master.config``` - Used to fetch Sake/SakeExtensions from NuGet.
* ```build.cmd``` - Build entry point for Windows.
* ```build.sh``` - Build entry point for non-Windows (Mac/Linux/etc).
* ```makefile.shade``` - The cross platform build script for your project.

Thats it! Your ready to start creating your build/ci scripts!
