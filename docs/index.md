# Plugin.Maui.NavigationAware Documentation

Welcome to the Plugin.Maui.NavigationAware documentation!

## Table of Contents

1. [Getting Started](getting-started.md)
2. [API Reference](api-reference.md)
3. [Examples](examples.md)
4. [Migration Guide](migration-guide.md)

## Quick Links

- [GitHub Repository](https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware)
- [NuGet Package](https://www.nuget.org/packages/Plugin.Maui.NavigationAware/)
- [Report Issues](https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/issues)

## What is Plugin.Maui.NavigationAware?

Plugin.Maui.NavigationAware is a lightweight plugin for .NET MAUI that provides navigation awareness capabilities similar to Prism. It allows your pages to be notified when navigation events occur, making it easy to handle page lifecycle events and pass parameters between pages.

## Key Features

- **Navigation Lifecycle Events**: Receive callbacks when navigating to or from a page
- **Parameter Passing**: Pass strongly-typed parameters during navigation
- **Simple Integration**: Easy-to-use base class or interface
- **Cross-Platform**: Works on all .NET MAUI platforms
- **Zero Dependencies**: No external dependencies beyond .NET MAUI

## Installation

```bash
dotnet add package Plugin.Maui.NavigationAware
```

## Quick Start

```csharp
public partial class MyPage : NavigationAwarePage
{
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        // Handle incoming navigation
    }

    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
        // Handle outgoing navigation
    }
}
```

For detailed instructions, see the [Getting Started Guide](getting-started.md).
