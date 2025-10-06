# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- **String-Based Navigation**: Navigate using page names/keys similar to Prism
  - New `NavigateToAsync(string pageKey, INavigationParameters? parameters)` overload
  - `PageRegistry` class for managing page type registrations
  - `RegisterPage<TPage>()` extension method for registering pages
  - Support for both DI-resolved and manually-instantiated pages
  - Type-safe navigation using `nameof()` operator
- Initial release of Plugin.Maui.NavigationAware
- `INavigationAware` interface for navigation lifecycle awareness
- `INavigationParameters` interface for passing parameters during navigation
- `NavigationParameters` class for creating and managing navigation parameters
- `NavigationAwarePage` base class for easy implementation
- `INavigationService` interface for navigation operations
- `NavigationService` implementation for handling navigation with parameters
- Extension methods for easy integration (`GetNavigationService`, `AddNavigationAware`)
- Cross-platform support (iOS, Android, macOS, Windows, .NET)
- Comprehensive documentation and examples
- HTML documentation website
- Sample application demonstrating all features

### Enhanced
- Updated documentation to include string-based navigation examples
- Updated migration guide with Prism-compatible navigation patterns
- Enhanced API reference with complete PageRegistry documentation

### Features
- ✅ Navigation Awareness (OnNavigatedTo/OnNavigatedFrom)
- ✅ Strongly-typed parameter passing
- ✅ String-based navigation (like Prism)
- ✅ Instance-based navigation
- ✅ Prism-like API for familiar developer experience
- ✅ Easy integration with existing projects
- ✅ Optional dependency injection support
- ✅ Zero external dependencies

## [1.0.0] - TBD

### First Public Release
- Initial stable release
- Full documentation
- Sample application
- Cross-platform support

---

## Contributing

If you have suggestions for how this project could be improved, or want to report a bug, please open an issue! We'd love all and any contributions.

## License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
