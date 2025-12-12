# BCSQuiz Project Overview

## Table of Contents
1. [Project Description](#project-description)
2. [Technology Stack](#technology-stack)
3. [Project Structure](#project-structure)
4. [Architecture](#architecture)
5. [Models](#models)
6. [Views and Pages](#views-and-pages)
7. [Navigation](#navigation)
8. [Styling and Resources](#styling-and-resources)
9. [Platform Support](#platform-support)
10. [Build and Configuration](#build-and-configuration)

---

## Project Description

**BCSQuiz** is a cross-platform mobile application built with .NET MAUI (Multi-platform App UI). The application appears to be a contact management system with quiz-related naming conventions. It provides functionality to view, browse, and edit contact information across multiple platforms including Android, iOS, macOS (Catalyst), and Windows.

---

## Technology Stack

- **Framework**: .NET 8.0 with .NET MAUI
- **Language**: C# 
- **UI Framework**: XAML for UI definition
- **Architecture Pattern**: MVVM-like structure with code-behind
- **Target Platforms**:
  - Android (API 21+)
  - iOS (11.0+)
  - macOS Catalyst (13.1+)
  - Windows (10.0.17763.0+)
  - Tizen (6.5+) - optional

---

## Project Structure

```
BCSQuiz/
├── .git/                          # Git repository files
├── .gitattributes                 # Git attributes configuration
├── .gitignore                     # Git ignore rules
├── App.xaml                       # Application-level XAML resources
├── App.xaml.cs                    # Application entry point
├── AppShell.xaml                  # Shell navigation structure
├── AppShell.xaml.cs               # Shell code-behind and routing
├── BCSQuiz.csproj                 # Project file
├── BCSQuiz.sln                    # Solution file
├── MainPage.xaml                  # Default MAUI template page (unused)
├── MainPage.xaml.cs               # Default page code-behind (unused)
├── MauiProgram.cs                 # Application configuration
├── README.md                      # Basic project readme
├── Models/                        # Data models
│   ├── Contact.cs                 # Contact entity model
│   └── ContactRepository.cs       # Static data repository
├── Views/                         # UI pages
│   ├── HomePage.xaml              # Home/landing page
│   ├── HomePage.xaml.cs           # Home page logic
│   ├── ContactPage.xaml           # Contact list page
│   ├── ContactPage.xaml.cs        # Contact list logic
│   ├── EditContactPage.xaml       # Edit contact page
│   └── EditContactPage.xaml.cs    # Edit contact logic
├── Platforms/                     # Platform-specific code
│   ├── Android/                   # Android-specific files
│   ├── iOS/                       # iOS-specific files
│   ├── MacCatalyst/              # macOS-specific files
│   ├── Windows/                   # Windows-specific files
│   └── Tizen/                     # Tizen-specific files (optional)
├── Resources/                     # Application resources
│   ├── AppIcon/                   # Application icon files
│   │   ├── appicon.svg
│   │   └── appiconfg.svg
│   ├── Fonts/                     # Custom fonts
│   │   ├── OpenSans-Regular.ttf
│   │   └── OpenSans-Semibold.ttf
│   ├── Images/                    # Image resources
│   │   └── dotnet_bot.png
│   ├── Raw/                       # Raw assets
│   │   └── AboutAssets.txt
│   ├── Splash/                    # Splash screen
│   │   └── splash.svg
│   └── Styles/                    # XAML style definitions
│       ├── Colors.xaml
│       └── Styles.xaml
└── Properties/                    # Project properties
    └── launchSettings.json
```

---

## Architecture

### Application Flow

1. **Entry Point**: `MauiProgram.cs` configures the application
2. **Application Class**: `App.xaml.cs` sets `AppShell` as the main page
3. **Shell Navigation**: `AppShell.xaml` defines the navigation structure with `HomePage` as the initial route
4. **Pages**: Three main pages handle different functionalities

### Design Pattern

The application uses a simplified MVVM-like pattern:
- **Models**: Data classes (`Contact`, `ContactRepository`)
- **Views**: XAML files define the UI
- **Code-Behind**: `.xaml.cs` files contain UI logic (instead of ViewModels)

---

## Models

### Contact.cs
```csharp
namespace BCSQuiz.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
```

**Properties:**
- `ContactId`: Unique identifier for the contact
- `Name`: Contact's name
- `Email`: Contact's email address

### ContactRepository.cs
```csharp
namespace BCSQuiz.Models
{
    public static class ContactRepository
    {
        public static List<Contact> _contacts = new List<Contact>()
        {
            new Contact { ContactId = 1, Name = "Momin", Email = "momincse13@gmail.com" },
            new Contact { ContactId = 2, Name = "Sazzad", Email = "Momin17@gmail.com" },
            new Contact { ContactId = 3, Name = "Johab", Email = "momincse13@gmail.com" }
        };
        
        public static List<Contact> GetContacts() => _contacts;
        
        public static Contact GetContactById(int contactId)
        {
            return _contacts.FirstOrDefault(x => x.ContactId == contactId);
        }
    }
}
```

**Purpose**: Static in-memory repository for contact data
**Methods:**
- `GetContacts()`: Returns all contacts
- `GetContactById(int contactId)`: Returns a specific contact by ID

**Note**: Uses a static list for demo purposes; production apps would use a database.

---

## Views and Pages

### 1. HomePage (Views/HomePage.xaml)

**Purpose**: Landing page with navigation buttons

**UI Elements:**
- Welcome label
- "Category" button (ctgBtn) - currently not implemented
- "Start" button (strBtn) - navigates to ContactPage

**Code-Behind Logic:**
```csharp
private void ctgBtn_Clicked(object sender, EventArgs e)
{
    // Currently commented out/not implemented
}

private void strBtn_Clicked(object sender, EventArgs e)
{
    Shell.Current.GoToAsync(nameof(ContactPage));
}
```

### 2. ContactPage (Views/ContactPage.xaml)

**Purpose**: Displays a list of all contacts

**UI Elements:**
- Welcome label
- ListView displaying contacts with name and email
- Cancel button to return to HomePage

**Features:**
- Uses `ListView` with `DataTemplate` for contact items
- Each item shows contact name and email
- Selecting an item navigates to EditContactPage with the contact ID

**Code-Behind Logic:**
```csharp
public ContactPage()
{
    InitializeComponent();
    List<Contact> contacts = ContactRepository.GetContacts();
    listContacts.ItemsSource = contacts;
}

private async void listContacts_ItemSelected(object Sender, SelectedItemChangedEventArgs e)
{
    if (listContacts.SelectedItem != null)
    {
        await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={((Contact)listContacts.SelectedItem).ContactId}");
    }
}

private void cnlBtn_Clicked(object Sender, EventArgs e)
{
    Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

### 3. EditContactPage (Views/EditContactPage.xaml)

**Purpose**: Display and edit contact details (currently view-only)

**UI Elements:**
- Title label showing contact name
- Framed form with:
  - Name entry field
  - Email entry field
- Cancel button (returns to HomePage)
- Save button (not yet implemented)

**Query Property:**
- Uses `[QueryProperty(nameof(ContactId), "Id")]` to receive contact ID from navigation

**Code-Behind Logic:**
```csharp
[QueryProperty(nameof(ContactId), "Id")]
public partial class EditContactPage : ContentPage
{
    private Contact contact;
    
    public string ContactId
    {
        set
        {
            contact = ContactRepository.GetContactById(int.Parse(value));
            lblName.Text = contact.Name;
            entName.Text = contact.Name;
            entEmail.Text = contact.Email;
        }
    }
    
    private void cnlBtn_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}
```

**Note**: The Save button has no handler, so editing is currently not functional.

### 4. MainPage (MainPage.xaml)

**Purpose**: Default MAUI template page (not used in the app)

This is the default counter page that comes with new MAUI projects. The app uses AppShell navigation instead, starting with HomePage.

---

## Navigation

### Shell Navigation Structure

The application uses .NET MAUI Shell for navigation:

**AppShell.xaml**:
```xml
<Shell>
    <ShellContent
        Title="Home"
        ContentTemplate="{DataTemplate views:HomePage}"
        Route="HomePage" />
</Shell>
```

**Registered Routes** (in AppShell.xaml.cs):
```csharp
Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
Routing.RegisterRoute(nameof(EditContactPage), typeof(EditContactPage));
Routing.RegisterRoute(nameof(ContactPage), typeof(ContactPage));
```

### Navigation Flow

```
HomePage
    ├─> ContactPage (via "Start" button)
    │   └─> EditContactPage (via contact selection)
    │       └─> Back to HomePage (via "Cancel")
    └─> (Category - not implemented)
```

### Navigation Methods

1. **Forward Navigation**: 
   ```csharp
   await Shell.Current.GoToAsync(nameof(ContactPage));
   ```

2. **Navigation with Parameters**:
   ```csharp
   await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={contactId}");
   ```

3. **Absolute Navigation** (back to root):
   ```csharp
   Shell.Current.GoToAsync($"//{nameof(HomePage)}");
   ```

---

## Styling and Resources

### Colors (Resources/Styles/Colors.xaml)

**Primary Colors:**
- Primary: `#512BD4` (Purple)
- PrimaryDark: `#ac99ea`
- Secondary: `#DFD8F7`
- Tertiary: `#2B0B98`

**Neutral Colors:**
- White, Black, OffBlack
- Gray scale: Gray100 through Gray950
- Special: Magenta (`#D600AA`), MidnightBlue (`#190649`)

**Theme Support:**
- Light and Dark theme bindings using `AppThemeBinding`

### Styles (Resources/Styles/Styles.xaml)

Comprehensive style definitions for all MAUI controls:
- **Buttons**: Purple background, white text, rounded corners
- **Entry/Editor**: Transparent background, themed text colors
- **Labels**: Multiple styles including Headline and SubHeadline
- **ListView**: Custom separator colors
- **Frame**: Shadow effects, rounded corners
- **And many more...**

All styles support:
- Light/Dark theme switching
- Disabled states
- Visual state management
- Accessibility features

### Fonts

Two OpenSans font variants:
- OpenSans-Regular.ttf
- OpenSans-Semibold.ttf

### Images and Icons

- App icon: `Resources/AppIcon/appicon.svg`
- Splash screen: `Resources/Splash/splash.svg`
- Sample image: `Resources/Images/dotnet_bot.png`

---

## Platform Support

### Android (Platforms/Android/)

**Files:**
- `MainActivity.cs`: Main Android activity
- `MainApplication.cs`: Application class
- `AndroidManifest.xml`: Android manifest configuration
- `Resources/values/colors.xml`: Android-specific color resources

**Minimum SDK**: Android 5.0 (API 21)

### iOS (Platforms/iOS/)

**Files:**
- `AppDelegate.cs`: iOS application delegate
- `Program.cs`: Entry point
- `Info.plist`: iOS configuration (implied)

**Minimum Version**: iOS 11.0

### macOS Catalyst (Platforms/MacCatalyst/)

**Files:**
- `AppDelegate.cs`: macOS app delegate
- `Program.cs`: Entry point
- `Info.plist`: macOS configuration (implied)

**Minimum Version**: macOS 13.1 (Catalyst)

### Windows (Platforms/Windows/)

**Files:**
- `App.xaml.cs`: Windows-specific app initialization
- `app.manifest`: Windows manifest (implied)
- `Package.appxmanifest`: UWP packaging manifest (implied)

**Minimum Version**: Windows 10 version 1809 (Build 10.0.17763.0)

### Tizen (Platforms/Tizen/)

**Files:**
- `Main.cs`: Tizen entry point
- `tizen-manifest.xml`: Tizen configuration

**Minimum Version**: Tizen 6.5
**Note**: Optional platform, commented out in project file

---

## Build and Configuration

### Project File (BCSQuiz.csproj)

**Target Frameworks:**
```xml
<TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">
    $(TargetFrameworks);net8.0-windows10.0.19041.0
</TargetFrameworks>
```

**Key Settings:**
- OutputType: Exe
- UseMaui: true
- SingleProject: true
- ImplicitUsings: enable
- Nullable: enable

**Application Metadata:**
- ApplicationTitle: BCSQuiz
- ApplicationId: com.companyname.bcsquiz
- ApplicationDisplayVersion: 1.0
- ApplicationVersion: 1

**Dependencies:**
- Microsoft.Maui.Controls
- Microsoft.Maui.Controls.Compatibility
- Microsoft.Extensions.Logging.Debug (8.0.1)

### MauiProgram.cs

**Configuration:**
```csharp
var builder = MauiApp.CreateBuilder();
builder
    .UseMauiApp<App>()
    .ConfigureFonts(fonts =>
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    });

#if DEBUG
    builder.Logging.AddDebug();
#endif

return builder.Build();
```

### Launch Settings (Properties/launchSettings.json)

**Profile:**
- Windows Machine: MsixPackage deployment

---

## Key Features Summary

### Implemented Features
✅ Cross-platform UI with .NET MAUI  
✅ Shell-based navigation  
✅ Contact listing with ListView  
✅ Contact detail viewing  
✅ Navigation with parameters  
✅ Light/Dark theme support  
✅ Responsive layouts  
✅ Platform-specific code structure  

### Features Not Yet Implemented
⚠️ Category functionality (button exists but not connected)  
⚠️ Contact editing (Save button has no handler)  
⚠️ Data persistence (uses in-memory static list)  
⚠️ Contact addition  
⚠️ Contact deletion  
⚠️ Quiz functionality (despite project name)  

---

## Development Notes

### Code Quality Observations

**Strengths:**
- Clean separation of concerns with Models and Views
- Proper use of MAUI Shell navigation
- Comprehensive styling with theme support
- Multi-platform support configured

**Areas for Improvement:**
- No actual quiz functionality (project is misnamed or incomplete)
- Missing data persistence layer
- Edit functionality incomplete (no save handler)
- Static data repository should be replaced with database
- Missing validation for form inputs
- No error handling for navigation or data operations
- Category feature stubbed out

### Suggested Enhancements

1. **Data Layer**: Implement proper data persistence using SQLite or similar
2. **MVVM**: Add ViewModels for better separation of concerns
3. **Validation**: Add input validation for contact forms
4. **Complete Edit**: Implement save functionality in EditContactPage
5. **Add/Delete**: Add ability to create and remove contacts
6. **Quiz Features**: Implement actual quiz functionality if that's the intent
7. **Categories**: Complete the category feature or remove it
8. **Error Handling**: Add try-catch blocks and user-friendly error messages
9. **Loading States**: Add loading indicators for async operations
10. **Unit Tests**: Add test project for business logic

---

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 (17.8+) or Visual Studio Code with .NET MAUI extensions
- Platform-specific SDKs:
  - Android: Android SDK (API 21+)
  - iOS: Xcode (macOS only)
  - Windows: Windows 10 SDK (17763+)

### Building the Project

```bash
# Restore dependencies
dotnet restore

# Build for all platforms
dotnet build

# Build for specific platform
dotnet build -f net8.0-android
dotnet build -f net8.0-ios
dotnet build -f net8.0-windows10.0.19041.0

# Run the application
dotnet run
```

### Running on Different Platforms

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

---

## License

Not specified in the repository.

---

## Contact

Repository: https://github.com/momin210103/BCSQuiz

---

*Last Updated: December 2024*
