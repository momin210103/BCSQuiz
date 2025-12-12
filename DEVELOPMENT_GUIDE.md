# BCSQuiz Development Guide

## Table of Contents

1. [Getting Started](#getting-started)
2. [Development Environment Setup](#development-environment-setup)
3. [Project Structure](#project-structure)
4. [Development Workflow](#development-workflow)
5. [Coding Standards](#coding-standards)
6. [Building and Running](#building-and-running)
7. [Debugging](#debugging)
8. [Testing](#testing)
9. [Common Tasks](#common-tasks)
10. [Troubleshooting](#troubleshooting)
11. [Contributing](#contributing)

---

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

#### Required

- **Visual Studio 2022** (version 17.8 or later) or **Visual Studio Code**
- **.NET 8.0 SDK** or later
- **Git** for version control

#### Platform-Specific Requirements

**For Android Development:**
- Android SDK (API level 21 or higher)
- Android Emulator or physical device
- Java Development Kit (JDK) 11 or later

**For iOS Development (macOS only):**
- Xcode 14 or later
- iOS Simulator or physical device
- Apple Developer account (for device deployment)

**For Windows Development:**
- Windows 10 SDK (10.0.17763.0 or later)
- Windows 11 recommended

**For macOS Development:**
- macOS 13.1 or later
- Xcode command-line tools

---

## Development Environment Setup

### Visual Studio 2022 Setup

1. **Install Visual Studio 2022**
   - Download from: https://visualstudio.microsoft.com/
   - Select the ".NET Multi-platform App UI development" workload

2. **Configure Android Emulator** (for Android development)
   - Tools → Android → Android Device Manager
   - Create a new virtual device or use an existing one

3. **Configure iOS Simulator** (macOS only)
   - Xcode → Preferences → Components
   - Download required iOS simulators

### Visual Studio Code Setup

1. **Install Visual Studio Code**
   - Download from: https://code.visualstudio.com/

2. **Install Required Extensions**
   ```
   - C# for Visual Studio Code
   - .NET MAUI Extension Pack
   - C# Dev Kit
   ```

3. **Install .NET MAUI Workload**
   ```bash
   dotnet workload install maui
   ```

### Verify Installation

```bash
# Check .NET version
dotnet --version

# Check installed workloads
dotnet workload list

# Expected output should include: maui, maui-android, maui-ios, etc.
```

---

## Project Structure

### Directory Layout

```
BCSQuiz/
├── Models/              # Data models
├── Views/               # XAML pages and code-behind
├── Resources/           # App resources
│   ├── AppIcon/        # App icons
│   ├── Fonts/          # Custom fonts
│   ├── Images/         # Image assets
│   ├── Splash/         # Splash screen
│   └── Styles/         # XAML styles
├── Platforms/          # Platform-specific code
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   └── Windows/
└── Properties/         # Launch settings
```

### Key Files

| File | Purpose |
|------|---------|
| `App.xaml` / `App.xaml.cs` | Application entry point |
| `AppShell.xaml` / `AppShell.xaml.cs` | Navigation shell |
| `MauiProgram.cs` | App configuration |
| `BCSQuiz.csproj` | Project configuration |
| `BCSQuiz.sln` | Solution file |

---

## Development Workflow

### 1. Clone the Repository

```bash
git clone https://github.com/momin210103/BCSQuiz.git
cd BCSQuiz
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

### 4. Run the Application

**Android:**
```bash
dotnet build -t:Run -f net8.0-android
```

**iOS (macOS only):**
```bash
dotnet build -t:Run -f net8.0-ios
```

**Windows:**
```bash
dotnet build -t:Run -f net8.0-windows10.0.19041.0
```

**macOS Catalyst:**
```bash
dotnet build -t:Run -f net8.0-maccatalyst
```

---

## Coding Standards

### C# Conventions

#### Naming Conventions

```csharp
// Classes: PascalCase
public class ContactRepository { }

// Methods: PascalCase
public void GetContacts() { }

// Private fields: camelCase with underscore prefix
private List<Contact> _contacts;

// Properties: PascalCase
public string Name { get; set; }

// Local variables: camelCase
var contactList = new List<Contact>();

// Constants: PascalCase
public const int MaxContacts = 100;
```

#### Code Organization

```csharp
// Order of class members:
public class ExamplePage : ContentPage
{
    // 1. Private fields
    private Contact _contact;
    
    // 2. Constructor
    public ExamplePage()
    {
        InitializeComponent();
    }
    
    // 3. Public properties
    public string ContactId { get; set; }
    
    // 4. Public methods
    public void LoadData() { }
    
    // 5. Private methods
    private void OnButtonClicked(object sender, EventArgs e) { }
}
```

### XAML Conventions

```xml
<!-- Use meaningful names for controls -->
<Button x:Name="saveContactButton" 
        Text="Save" 
        Clicked="OnSaveClicked" />

<!-- Group related properties -->
<Entry x:Name="nameEntry"
       Placeholder="Enter name"
       Keyboard="Text"
       HorizontalOptions="FillAndExpand" />

<!-- Use data binding where appropriate -->
<Label Text="{Binding Name}" 
       FontSize="18"
       TextColor="{StaticResource Primary}" />
```

### File Organization

- One class per file
- File name matches class name
- Use folders to organize related files
- Group XAML and code-behind files together

---

## Building and Running

### Build Configurations

#### Debug Build

```bash
dotnet build --configuration Debug
```

**Characteristics:**
- Includes debug symbols
- No code optimization
- Logging enabled

#### Release Build

```bash
dotnet build --configuration Release
```

**Characteristics:**
- Optimized code
- No debug symbols
- Reduced logging

### Clean Build

```bash
# Clean build artifacts
dotnet clean

# Clean and rebuild
dotnet clean && dotnet build
```

### Build for Specific Platform

```bash
# Android only
dotnet build -f net8.0-android

# iOS only (macOS)
dotnet build -f net8.0-ios

# Windows only
dotnet build -f net8.0-windows10.0.19041.0

# macOS Catalyst only (macOS)
dotnet build -f net8.0-maccatalyst
```

---

## Debugging

### Visual Studio Debugging

1. **Set Breakpoints**
   - Click in the left margin of code editor
   - Or press F9 on the line

2. **Start Debugging**
   - Press F5 or click "Start Debugging"
   - Select target platform (Android, iOS, Windows)

3. **Debugging Tools**
   - Locals window: View local variables
   - Watch window: Monitor specific variables
   - Call Stack: View execution path
   - Output window: See debug logs

### Debug Output

```csharp
// Add debug output
System.Diagnostics.Debug.WriteLine($"Contact loaded: {contact.Name}");

// Or use the logger (in MauiProgram.cs)
#if DEBUG
builder.Logging.AddDebug();
#endif

// In your code
var logger = LoggerFactory.Create(builder => 
    builder.AddDebug()).CreateLogger<ContactPage>();
logger.LogInformation("Page loaded");
```

### Common Debugging Scenarios

#### Debugging Navigation Issues

```csharp
try
{
    await Shell.Current.GoToAsync(nameof(ContactPage));
}
catch (Exception ex)
{
    Debug.WriteLine($"Navigation failed: {ex.Message}");
}
```

#### Debugging Data Loading

```csharp
var contacts = ContactRepository.GetContacts();
Debug.WriteLine($"Loaded {contacts.Count} contacts");
foreach (var contact in contacts)
{
    Debug.WriteLine($"  - {contact.Name}");
}
```

#### Debugging UI Updates

```csharp
protected override void OnAppearing()
{
    base.OnAppearing();
    Debug.WriteLine("Page appearing");
    LoadData();
}
```

---

## Testing

### Unit Testing (Not Yet Implemented)

To add unit testing:

1. **Create Test Project**

```bash
dotnet new xunit -n BCSQuiz.Tests
cd BCSQuiz.Tests
dotnet add reference ../BCSQuiz.csproj
```

2. **Add Test Packages**

```bash
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
```

3. **Write Tests**

```csharp
// ContactRepositoryTests.cs
using Xunit;
using BCSQuiz.Models;

public class ContactRepositoryTests
{
    [Fact]
    public void GetContacts_ReturnsAllContacts()
    {
        // Arrange & Act
        var contacts = ContactRepository.GetContacts();
        
        // Assert
        Assert.NotNull(contacts);
        Assert.True(contacts.Count > 0);
    }
    
    [Fact]
    public void GetContactById_ValidId_ReturnsContact()
    {
        // Arrange
        int validId = 1;
        
        // Act
        var contact = ContactRepository.GetContactById(validId);
        
        // Assert
        Assert.NotNull(contact);
        Assert.Equal(validId, contact.ContactId);
    }
    
    [Fact]
    public void GetContactById_InvalidId_ReturnsNull()
    {
        // Arrange
        int invalidId = 999;
        
        // Act
        var contact = ContactRepository.GetContactById(invalidId);
        
        // Assert
        Assert.Null(contact);
    }
}
```

4. **Run Tests**

```bash
dotnet test
```

---

## Common Tasks

### Adding a New Page

#### 1. Create XAML Files

**NewPage.xaml**:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="BCSQuiz.Views.NewPage"
             Title="New Page">
    <VerticalStackLayout Padding="16" Spacing="12">
        <Label Text="Welcome to the new page!" />
    </VerticalStackLayout>
</ContentPage>
```

**NewPage.xaml.cs**:
```csharp
namespace BCSQuiz.Views;

public partial class NewPage : ContentPage
{
    public NewPage()
    {
        InitializeComponent();
    }
}
```

#### 2. Register Route

In `AppShell.xaml.cs`:
```csharp
Routing.RegisterRoute(nameof(NewPage), typeof(NewPage));
```

#### 3. Navigate to Page

```csharp
await Shell.Current.GoToAsync(nameof(NewPage));
```

---

### Adding a New Model

```csharp
// Models/YourModel.cs
namespace BCSQuiz.Models
{
    public class YourModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        
        // Add validation
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
```

---

### Modifying Styles

#### Global Styles

Edit `Resources/Styles/Styles.xaml`:

```xml
<Style TargetType="Button" x:Key="PrimaryButton">
    <Setter Property="BackgroundColor" Value="{StaticResource Primary}" />
    <Setter Property="TextColor" Value="White" />
    <Setter Property="CornerRadius" Value="8" />
    <Setter Property="Padding" Value="16,12" />
</Style>
```

Use in XAML:
```xml
<Button Text="Click Me" Style="{StaticResource PrimaryButton}" />
```

#### Colors

Edit `Resources/Styles/Colors.xaml`:

```xml
<Color x:Key="MyCustomColor">#FF5733</Color>
```

---

### Adding Images

1. **Add Image File**
   - Place image in `Resources/Images/`
   - Supported formats: PNG, JPG, SVG

2. **Use in XAML**
   ```xml
   <Image Source="my_image.png" 
          WidthRequest="200"
          HeightRequest="200" />
   ```

3. **Use in Code**
   ```csharp
   var image = new Image
   {
       Source = "my_image.png"
   };
   ```

---

### Working with ListView

```xml
<!-- XAML -->
<ListView x:Name="myListView"
          ItemsSource="{Binding Items}"
          ItemSelected="OnItemSelected">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <VerticalStackLayout Padding="10">
                    <Label Text="{Binding Title}" FontSize="18" />
                    <Label Text="{Binding Description}" FontSize="14" />
                </VerticalStackLayout>
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

```csharp
// Code-behind
private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
{
    if (e.SelectedItem is YourModel item)
    {
        // Handle selection
    }
}
```

---

## Troubleshooting

### Common Issues and Solutions

#### 1. Build Failures

**Issue**: "The name 'InitializeComponent' does not exist"

**Solution**:
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

---

#### 2. Navigation Not Working

**Issue**: Navigation fails silently

**Solution**:
```csharp
// Ensure route is registered
Routing.RegisterRoute(nameof(YourPage), typeof(YourPage));

// Check for typos in route names
await Shell.Current.GoToAsync(nameof(YourPage)); // Use nameof()
```

---

#### 3. UI Not Updating

**Issue**: Changes in code-behind don't reflect in UI

**Solution**:
```csharp
// Use INotifyPropertyChanged or reload the page
public class MyViewModel : INotifyPropertyChanged
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

---

#### 4. Android Emulator Issues

**Issue**: Emulator not starting or running slowly

**Solutions**:
- Enable hardware acceleration (Intel HAXM or AMD Hypervisor)
- Increase emulator RAM in AVD Manager
- Use x86_64 system image instead of ARM
- Enable "Use Host GPU" in emulator settings

---

#### 5. iOS Simulator Issues (macOS)

**Issue**: Simulator not appearing in Visual Studio

**Solution**:
```bash
# Reset simulator
xcrun simctl erase all

# List available simulators
xcrun simctl list devices
```

---

#### 6. Hot Reload Not Working

**Solution**:
- Enable XAML Hot Reload in Visual Studio settings
- Ensure app is running in Debug mode
- Restart the debug session

---

### Getting Help

**Resources**:
- [.NET MAUI Documentation](https://docs.microsoft.com/en-us/dotnet/maui/)
- [.NET MAUI Community Toolkit](https://github.com/CommunityToolkit/Maui)
- [Stack Overflow - MAUI Tag](https://stackoverflow.com/questions/tagged/maui)
- [GitHub Issues](https://github.com/dotnet/maui/issues)

---

## Contributing

### Workflow

1. **Create a Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make Changes**
   - Follow coding standards
   - Add comments for complex logic
   - Update documentation

3. **Test Your Changes**
   - Build for all target platforms
   - Test on emulators/simulators
   - Add unit tests if applicable

4. **Commit Changes**
   ```bash
   git add .
   git commit -m "Add: Brief description of changes"
   ```

5. **Push to Repository**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Create Pull Request**
   - Provide clear description
   - Reference related issues
   - Wait for code review

### Commit Message Guidelines

```
Format: <type>: <description>

Types:
- Add: New feature
- Fix: Bug fix
- Update: Modify existing feature
- Refactor: Code refactoring
- Docs: Documentation changes
- Style: Code style changes
- Test: Add or update tests

Examples:
- Add: Implement contact deletion feature
- Fix: Resolve navigation crash on EditContactPage
- Update: Improve ListView performance
- Refactor: Extract validation logic to separate class
- Docs: Update API reference for ContactRepository
```

---

## Performance Tips

### Optimize ListView

```csharp
// Use CachingStrategy
<ListView CachingStrategy="RecycleElement">
    <!-- ItemTemplate -->
</ListView>
```

### Async Operations

```csharp
// Always use async/await for I/O operations
public async Task LoadContactsAsync()
{
    var contacts = await Task.Run(() => 
        ContactRepository.GetContacts());
    listView.ItemsSource = contacts;
}
```

### Memory Management

```csharp
// Dispose of resources
protected override void OnDisappearing()
{
    base.OnDisappearing();
    // Clean up resources
}
```

---

## Useful Commands Reference

```bash
# Build commands
dotnet build                          # Build all platforms
dotnet build -c Release              # Release build
dotnet clean                         # Clean build artifacts

# Run commands
dotnet build -t:Run -f net8.0-android    # Run on Android
dotnet build -t:Run -f net8.0-ios        # Run on iOS
dotnet build -t:Run -f net8.0-windows10.0.19041.0  # Run on Windows

# Package commands
dotnet publish -f net8.0-android -c Release    # Android APK
dotnet publish -f net8.0-ios -c Release        # iOS IPA

# Workload commands
dotnet workload list                 # List installed workloads
dotnet workload install maui         # Install MAUI workload
dotnet workload update              # Update workloads

# NuGet commands
dotnet restore                       # Restore packages
dotnet add package PackageName       # Add package
dotnet list package                 # List packages
```

---

*Last Updated: December 2024*
