# BCSQuiz Quick Reference

A quick reference guide for common tasks and code patterns in the BCSQuiz project.

---

## 🚀 Quick Commands

### Build & Run

```bash
# Build all platforms
dotnet build

# Run on Android
dotnet build -t:Run -f net8.0-android

# Run on iOS (macOS only)
dotnet build -t:Run -f net8.0-ios

# Run on Windows
dotnet build -t:Run -f net8.0-windows10.0.19041.0

# Clean build
dotnet clean && dotnet build
```

### Package

```bash
# Create Android APK
dotnet publish -f net8.0-android -c Release

# Create iOS package
dotnet publish -f net8.0-ios -c Release
```

---

## 📂 File Locations

| What | Where |
|------|-------|
| Pages | `/Views/` |
| Models | `/Models/` |
| Styles | `/Resources/Styles/` |
| Images | `/Resources/Images/` |
| Fonts | `/Resources/Fonts/` |
| Platform code | `/Platforms/{Platform}/` |

---

## 🧭 Navigation Patterns

### Navigate Forward

```csharp
// Simple navigation
await Shell.Current.GoToAsync(nameof(ContactPage));

// With parameters
await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={contactId}");
```

### Navigate Back

```csharp
// Go back one page
await Shell.Current.GoToAsync("..");

// Go to root (clears navigation stack)
await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
```

### Register Routes

```csharp
// In AppShell.xaml.cs constructor
Routing.RegisterRoute(nameof(YourPage), typeof(YourPage));
```

### Query Parameters

```csharp
// Receiving page
[QueryProperty(nameof(ContactId), "Id")]
public partial class EditContactPage : ContentPage
{
    public string ContactId 
    { 
        set 
        { 
            // Use the parameter
            var id = int.Parse(value);
        } 
    }
}

// Sending page
await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id=123");
```

---

## 📊 Data Access

### Get All Contacts

```csharp
List<Contact> contacts = ContactRepository.GetContacts();
```

### Get Single Contact

```csharp
Contact contact = ContactRepository.GetContactById(1);

// Always check for null!
if (contact != null)
{
    // Use contact
}
```

### Create Contact (Not implemented)

```csharp
// Would need to add to repository:
var newContact = new Contact
{
    Name = "John Doe",
    Email = "john@example.com"
};
// ContactRepository.AddContact(newContact); // Not available yet
```

---

## 🎨 Styling

### Use Static Resources

```xml
<!-- In XAML -->
<Button BackgroundColor="{StaticResource Primary}" 
        TextColor="{StaticResource White}" />
```

### Theme Binding

```xml
<!-- Light/Dark theme support -->
<Label TextColor="{AppThemeBinding Light={StaticResource Black}, 
                                   Dark={StaticResource White}}" />
```

### Available Colors

| Color | Value |
|-------|-------|
| Primary | #512BD4 (Purple) |
| Secondary | #DFD8F7 |
| Tertiary | #2B0B98 |
| White | White |
| Black | Black |
| Gray100-Gray950 | Various shades |

### Apply Custom Style

```xml
<!-- Define style in Styles.xaml -->
<Style x:Key="MyButton" TargetType="Button">
    <Setter Property="BackgroundColor" Value="{StaticResource Primary}" />
</Style>

<!-- Use in XAML -->
<Button Style="{StaticResource MyButton}" Text="Click" />
```

---

## 📱 UI Controls

### Button

```xml
<Button x:Name="myButton"
        Text="Click Me"
        Clicked="OnButtonClicked" />
```

```csharp
private void OnButtonClicked(object sender, EventArgs e)
{
    // Handle click
}
```

### Entry (Text Input)

```xml
<Entry x:Name="nameEntry"
       Placeholder="Enter name"
       Text="{Binding Name}"
       Keyboard="Text" />
```

### Label

```xml
<Label Text="Hello World"
       FontSize="18"
       TextColor="{StaticResource Primary}" />
```

### ListView

```xml
<ListView x:Name="myListView"
          ItemsSource="{Binding Items}"
          ItemSelected="OnItemSelected">
    <ListView.ItemTemplate>
        <DataTemplate>
            <TextCell Text="{Binding Name}"
                      Detail="{Binding Email}" />
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

```csharp
private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
{
    if (e.SelectedItem is Contact contact)
    {
        // Handle selection
    }
}
```

---

## 🔧 Common Tasks

### Add New Page

1. **Create XAML file**: `Views/NewPage.xaml`
2. **Create code-behind**: `Views/NewPage.xaml.cs`
3. **Register route** in `AppShell.xaml.cs`:
   ```csharp
   Routing.RegisterRoute(nameof(NewPage), typeof(NewPage));
   ```
4. **Navigate to it**:
   ```csharp
   await Shell.Current.GoToAsync(nameof(NewPage));
   ```

### Add New Model

```csharp
// In Models folder
namespace BCSQuiz.Models
{
    public class YourModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
```

### Add Image

1. Add image to `Resources/Images/`
2. Use in XAML:
   ```xml
   <Image Source="myimage.png" 
          WidthRequest="100"
          HeightRequest="100" />
   ```

### Add Font

1. Add font to `Resources/Fonts/`
2. Register in `MauiProgram.cs`:
   ```csharp
   fonts.AddFont("MyFont.ttf", "MyFontAlias");
   ```
3. Use in XAML:
   ```xml
   <Label FontFamily="MyFontAlias" />
   ```

---

## 🐛 Debug Helpers

### Debug Output

```csharp
using System.Diagnostics;

Debug.WriteLine("Debug message");
Debug.WriteLine($"Value: {variable}");
```

### Try-Catch Navigation

```csharp
try
{
    await Shell.Current.GoToAsync(nameof(ContactPage));
}
catch (Exception ex)
{
    Debug.WriteLine($"Navigation error: {ex.Message}");
    await DisplayAlert("Error", "Navigation failed", "OK");
}
```

### Async Page Loading

```csharp
public ContactPage()
{
    InitializeComponent();
}

protected override async void OnAppearing()
{
    base.OnAppearing();
    await LoadDataAsync();
}

private async Task LoadDataAsync()
{
    try
    {
        var contacts = await Task.Run(() => 
            ContactRepository.GetContacts());
        listView.ItemsSource = contacts;
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Load error: {ex.Message}");
    }
}
```

---

## 📦 Page Lifecycle

```csharp
public class MyPage : ContentPage
{
    // Constructor - called once
    public MyPage()
    {
        InitializeComponent();
    }
    
    // Page is about to appear
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Reload data, start animations
    }
    
    // Page is about to disappear
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Clean up resources, stop animations
    }
}
```

---

## 🎯 Project Structure Reference

```
BCSQuiz/
│
├── App.xaml(.cs)              # App initialization
├── AppShell.xaml(.cs)         # Navigation shell & routes
├── MauiProgram.cs             # App configuration
│
├── Models/
│   ├── Contact.cs             # Contact model
│   └── ContactRepository.cs   # Data access
│
├── Views/
│   ├── HomePage.xaml(.cs)           # Landing page
│   ├── ContactPage.xaml(.cs)        # Contact list
│   └── EditContactPage.xaml(.cs)    # Contact details
│
├── Resources/
│   ├── Styles/
│   │   ├── Colors.xaml        # Color definitions
│   │   └── Styles.xaml        # Control styles
│   ├── Fonts/                 # Custom fonts
│   ├── Images/                # Image assets
│   └── Splash/                # Splash screen
│
└── Platforms/                 # Platform-specific code
    ├── Android/
    ├── iOS/
    ├── Windows/
    └── MacCatalyst/
```

---

## 🔍 Finding Things

### Find Usage of a Class

```bash
# Using grep
grep -r "ContactRepository" --include="*.cs"

# In Visual Studio
Right-click class name → Find All References (Shift+F12)
```

### Find Files

```bash
# Find XAML files
find . -name "*.xaml"

# Find C# files
find . -name "*.cs"
```

---

## ⚡ Performance Tips

### ListView Optimization

```xml
<ListView CachingStrategy="RecycleElement">
    <!-- Better performance for long lists -->
</ListView>
```

### Async Loading

```csharp
// Don't block UI thread
public async Task LoadDataAsync()
{
    var data = await Task.Run(() => GetDataFromRepository());
    myListView.ItemsSource = data;
}
```

### Image Optimization

```xml
<!-- Set explicit sizes to avoid reflows -->
<Image Source="photo.jpg"
       WidthRequest="100"
       HeightRequest="100"
       Aspect="AspectFill" />
```

---

## 🚨 Common Pitfalls

### ❌ Don't: Forget to register routes

```csharp
// Will fail - route not registered
await Shell.Current.GoToAsync(nameof(MyPage));
```

### ✅ Do: Register in AppShell constructor

```csharp
Routing.RegisterRoute(nameof(MyPage), typeof(MyPage));
```

---

### ❌ Don't: Forget null checks

```csharp
var contact = ContactRepository.GetContactById(1);
lblName.Text = contact.Name; // Might crash if null!
```

### ✅ Do: Check for null

```csharp
var contact = ContactRepository.GetContactById(1);
if (contact != null)
{
    lblName.Text = contact.Name;
}
```

---

### ❌ Don't: Block UI thread

```csharp
public ContactPage()
{
    InitializeComponent();
    // This blocks the UI!
    Thread.Sleep(5000);
}
```

### ✅ Do: Use async/await

```csharp
public ContactPage()
{
    InitializeComponent();
}

protected override async void OnAppearing()
{
    base.OnAppearing();
    await Task.Delay(5000); // Doesn't block UI
}
```

---

## 📚 Quick Links

- **Full Documentation**: [PROJECT_OVERVIEW.md](PROJECT_OVERVIEW.md)
- **Architecture Details**: [ARCHITECTURE.md](ARCHITECTURE.md)
- **API Reference**: [API_REFERENCE.md](API_REFERENCE.md)
- **Development Guide**: [DEVELOPMENT_GUIDE.md](DEVELOPMENT_GUIDE.md)

---

## 💡 Pro Tips

1. **Use `nameof()`** for type-safe navigation and property names
2. **Always await** async navigation calls
3. **Check for null** when accessing repository data
4. **Use StaticResource** for theme colors
5. **Register routes** before navigating
6. **Use debug output** liberally during development
7. **Test on multiple platforms** early and often
8. **Keep pages lightweight** - move logic to ViewModels (future)
9. **Use async/await** for all I/O operations
10. **Clean up resources** in OnDisappearing

---

## 🆘 Need Help?

1. Check the full documentation in this repository
2. Search [.NET MAUI docs](https://docs.microsoft.com/en-us/dotnet/maui/)
3. Ask on [Stack Overflow](https://stackoverflow.com/questions/tagged/maui)
4. Open an issue on [GitHub](https://github.com/momin210103/BCSQuiz/issues)

---

*Last Updated: December 2024*
