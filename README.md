# BCSQuiz

A cross-platform contact management application built with .NET MAUI (Multi-platform App UI) for Android, iOS, macOS, and Windows.

## 📋 Overview

BCSQuiz is a modern, cross-platform mobile application that demonstrates the capabilities of .NET MAUI. Despite its name, the current implementation focuses on contact management, providing a clean interface to view, browse, and manage contact information across multiple platforms.

### Key Features

- ✅ Cross-platform support (Android, iOS, Windows, macOS)
- ✅ Modern UI with light/dark theme support
- ✅ Contact listing and viewing
- ✅ Shell-based navigation
- ✅ Responsive layouts
- ✅ Platform-specific optimizations

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 (17.8+) or Visual Studio Code
- Platform-specific SDKs (Android SDK, Xcode for iOS/macOS, Windows SDK)

### Installation

```bash
# Clone the repository
git clone https://github.com/momin210103/BCSQuiz.git
cd BCSQuiz

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run on your preferred platform
dotnet build -t:Run -f net8.0-android    # Android
dotnet build -t:Run -f net8.0-ios        # iOS (macOS only)
dotnet build -t:Run -f net8.0-windows10.0.19041.0  # Windows
```

## 📚 Documentation

Comprehensive documentation is available in the following files:

- **[PROJECT_OVERVIEW.md](PROJECT_OVERVIEW.md)** - Complete project structure, architecture, and features
- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Technical architecture, design patterns, and component interactions
- **[API_REFERENCE.md](API_REFERENCE.md)** - Detailed API documentation for all classes, methods, and properties
- **[DEVELOPMENT_GUIDE.md](DEVELOPMENT_GUIDE.md)** - Development setup, workflow, coding standards, and troubleshooting

## 🏗️ Project Structure

```
BCSQuiz/
├── Models/              # Data models (Contact, ContactRepository)
├── Views/               # XAML pages (HomePage, ContactPage, EditContactPage)
├── Resources/           # App resources (fonts, images, styles)
├── Platforms/           # Platform-specific code (Android, iOS, Windows, macOS)
└── Properties/          # Launch settings
```

## 🎯 Current Functionality

### Implemented Features

- **Home Page**: Landing page with navigation options
- **Contact List**: Browse all contacts with name and email
- **Contact Details**: View individual contact information
- **Navigation**: Shell-based navigation between pages
- **Theming**: Full light/dark theme support

### In Development

- Contact editing (UI exists, save functionality pending)
- Category functionality
- Data persistence (currently uses in-memory storage)
- Quiz features (if intended based on project name)

## 🛠️ Technology Stack

- **Framework**: .NET 8.0
- **UI Framework**: .NET MAUI
- **Language**: C#
- **UI Definition**: XAML
- **Architecture**: Code-behind pattern (MVVM recommended for future)

## 📱 Supported Platforms

| Platform | Minimum Version | Status |
|----------|----------------|---------|
| Android | API 21 (Android 5.0) | ✅ Supported |
| iOS | iOS 11.0 | ✅ Supported |
| Windows | Windows 10 (Build 17763) | ✅ Supported |
| macOS | macOS 13.1 (Catalyst) | ✅ Supported |
| Tizen | Tizen 6.5 | ⚠️ Optional |

## 🔧 Development

### Building for Specific Platforms

```bash
# Android
dotnet build -f net8.0-android

# iOS (requires macOS)
dotnet build -f net8.0-ios

# Windows
dotnet build -f net8.0-windows10.0.19041.0

# macOS Catalyst (requires macOS)
dotnet build -f net8.0-maccatalyst
```

### Running Tests

```bash
# Tests not yet implemented
# See DEVELOPMENT_GUIDE.md for testing setup instructions
```

## 📖 Usage

1. **Launch the app** on your preferred platform
2. **Home Page**: Click "Start" to view contacts
3. **Contact List**: Tap any contact to view details
4. **Contact Details**: View contact information (editing coming soon)
5. **Navigate Back**: Use Cancel buttons or system back navigation

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m 'Add: YourFeature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

See [DEVELOPMENT_GUIDE.md](DEVELOPMENT_GUIDE.md) for detailed contribution guidelines and coding standards.

## 📝 License

Not specified - please add a LICENSE file if you want to define one.

## 👥 Authors

- **Momin** - Initial work - [momin210103](https://github.com/momin210103)

## 🙏 Acknowledgments

- Built with [.NET MAUI](https://github.com/dotnet/maui)
- Uses [Microsoft.Maui.Controls](https://www.nuget.org/packages/Microsoft.Maui.Controls/)
- Inspired by modern cross-platform development practices

## 📞 Contact

- **Repository**: [https://github.com/momin210103/BCSQuiz](https://github.com/momin210103/BCSQuiz)
- **Issues**: [GitHub Issues](https://github.com/momin210103/BCSQuiz/issues)

## 🗺️ Roadmap

### Short-term Goals

- [ ] Implement contact editing (save functionality)
- [ ] Add contact creation
- [ ] Add contact deletion
- [ ] Implement data persistence with SQLite
- [ ] Add input validation

### Long-term Goals

- [ ] Implement MVVM architecture
- [ ] Add unit and integration tests
- [ ] Backend API integration
- [ ] Cloud synchronization
- [ ] Quiz functionality (if intended)
- [ ] Advanced search and filtering
- [ ] Export/Import contacts

## ⚠️ Known Issues

- Save button in EditContactPage has no handler (editing not functional)
- Category button functionality not implemented
- No data persistence (uses in-memory storage)
- Limited error handling
- No input validation

See [Issues](https://github.com/momin210103/BCSQuiz/issues) for the complete list.

---

**Last Updated**: December 2024

For more detailed information, please refer to the comprehensive documentation files listed above.