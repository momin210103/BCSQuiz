# BCSQuiz API Reference

## Table of Contents

1. [Models](#models)
2. [Repositories](#repositories)
3. [Pages (Views)](#pages-views)
4. [Navigation](#navigation)
5. [App Lifecycle](#app-lifecycle)

---

## Models

### Contact Class

**Namespace**: `BCSQuiz.Models`

**Description**: Represents a contact entity with basic information.

#### Class Definition

```csharp
public class Contact
{
    public int ContactId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}
```

#### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ContactId` | `int` | 0 | Unique identifier for the contact |
| `Name` | `string` | `""` | Contact's display name |
| `Email` | `string` | `""` | Contact's email address |

#### Usage Example

```csharp
// Create a new contact
var contact = new Contact
{
    ContactId = 1,
    Name = "John Doe",
    Email = "john.doe@example.com"
};

// Access properties
Console.WriteLine($"Name: {contact.Name}");
Console.WriteLine($"Email: {contact.Email}");
```

#### Notes

- All properties have public getters and setters
- No validation is performed at the model level
- String properties default to empty string (not null)

---

## Repositories

### ContactRepository Class

**Namespace**: `BCSQuiz.Models`

**Description**: Static repository that manages contact data using an in-memory list.

#### Class Definition

```csharp
public static class ContactRepository
{
    public static List<Contact> _contacts;
    
    public static List<Contact> GetContacts();
    public static Contact GetContactById(int contactId);
}
```

#### Static Fields

##### `_contacts`

```csharp
public static List<Contact> _contacts
```

**Type**: `List<Contact>`

**Description**: Static list containing all contacts. Initialized with sample data.

**Initial Data**:
```csharp
_contacts = new List<Contact>()
{
    new Contact { ContactId = 1, Name = "Momin", Email = "momincse13@gmail.com" },
    new Contact { ContactId = 2, Name = "Sazzad", Email = "Momin17@gmail.com" },
    new Contact { ContactId = 3, Name = "Johab", Email = "momincse13@gmail.com" }
};
```

**Warning**: Being public and static, this list can be modified from anywhere, which is not recommended.

---

#### Methods

##### `GetContacts()`

```csharp
public static List<Contact> GetContacts()
```

**Description**: Returns all contacts in the repository.

**Parameters**: None

**Returns**: `List<Contact>` - A list containing all contacts

**Throws**: None

**Usage Example**:
```csharp
List<Contact> allContacts = ContactRepository.GetContacts();

foreach (var contact in allContacts)
{
    Console.WriteLine($"{contact.Name} - {contact.Email}");
}
```

**Notes**:
- Returns reference to the actual list (not a copy)
- Modifications to returned list will affect the repository
- No pagination support

---

##### `GetContactById(int contactId)`

```csharp
public static Contact GetContactById(int contactId)
```

**Description**: Retrieves a single contact by its ID.

**Parameters**:
| Name | Type | Description |
|------|------|-------------|
| `contactId` | `int` | The unique identifier of the contact to retrieve |

**Returns**: 
- `Contact` - The contact with the specified ID
- `null` - If no contact with the given ID exists

**Throws**: None

**Usage Example**:
```csharp
Contact contact = ContactRepository.GetContactById(1);

if (contact != null)
{
    Console.WriteLine($"Found: {contact.Name}");
}
else
{
    Console.WriteLine("Contact not found");
}
```

**Implementation Details**:
```csharp
return _contacts.FirstOrDefault(x => x.ContactId == contactId);
```

**Notes**:
- Uses LINQ `FirstOrDefault` to find the contact
- Returns `null` if contact doesn't exist (always check for null!)
- O(n) time complexity

---

### Missing Repository Methods

The following methods would be expected in a complete repository but are not implemented:

#### `AddContact(Contact contact)` ❌

**Expected Signature**:
```csharp
public static void AddContact(Contact contact)
{
    // Not implemented
}
```

**Description**: Should add a new contact to the repository.

---

#### `UpdateContact(Contact contact)` ❌

**Expected Signature**:
```csharp
public static void UpdateContact(Contact contact)
{
    // Not implemented
}
```

**Description**: Should update an existing contact in the repository.

---

#### `DeleteContact(int contactId)` ❌

**Expected Signature**:
```csharp
public static bool DeleteContact(int contactId)
{
    // Not implemented
}
```

**Description**: Should remove a contact from the repository.

---

## Pages (Views)

### HomePage

**Namespace**: `BCSQuiz.Views`

**File**: `Views/HomePage.xaml`, `Views/HomePage.xaml.cs`

**Description**: Landing page of the application with navigation buttons.

#### Class Definition

```csharp
public partial class HomePage : ContentPage
{
    public HomePage();
    
    private void ctgBtn_Clicked(object sender, EventArgs e);
    private void strBtn_Clicked(object sender, EventArgs e);
}
```

---

#### Constructor

##### `HomePage()`

```csharp
public HomePage()
```

**Description**: Initializes the home page.

**Usage**:
```csharp
var homePage = new HomePage();
```

---

#### Methods

##### `ctgBtn_Clicked(object sender, EventArgs e)`

```csharp
private void ctgBtn_Clicked(object sender, EventArgs e)
```

**Description**: Event handler for the Category button click.

**Parameters**:
| Name | Type | Description |
|------|------|-------------|
| `sender` | `object` | The button that triggered the event |
| `e` | `EventArgs` | Event arguments |

**Returns**: `void`

**Current Implementation**: Empty (not implemented)

**Expected Behavior**: Should navigate to a category selection page

---

##### `strBtn_Clicked(object sender, EventArgs e)`

```csharp
private void strBtn_Clicked(object sender, EventArgs e)
```

**Description**: Event handler for the Start button click. Navigates to ContactPage.

**Parameters**:
| Name | Type | Description |
|------|------|-------------|
| `sender` | `object` | The button that triggered the event |
| `e` | `EventArgs` | Event arguments |

**Returns**: `void`

**Implementation**:
```csharp
Shell.Current.GoToAsync(nameof(ContactPage));
```

**Navigation**: ContactPage (modal/pushed navigation)

---

#### UI Elements

| Element | Name | Type | Description |
|---------|------|------|-------------|
| Label | - | `Label` | Displays "Welcome to .NET MAUI!" |
| Category Button | `ctgBtn` | `Button` | Not yet functional |
| Start Button | `strBtn` | `Button` | Navigates to contacts |

---

### ContactPage

**Namespace**: `BCSQuiz.Views`

**File**: `Views/ContactPage.xaml`, `Views/ContactPage.xaml.cs`

**Description**: Displays a list of all contacts.

#### Class Definition

```csharp
public partial class ContactPage : ContentPage
{
    public ContactPage();
    
    private async void listContacts_ItemSelected(object Sender, SelectedItemChangedEventArgs e);
    private void cnlBtn_Clicked(object Sender, EventArgs e);
}
```

---

#### Constructor

##### `ContactPage()`

```csharp
public ContactPage()
```

**Description**: Initializes the contact page and loads contact data.

**Implementation**:
```csharp
InitializeComponent();
List<Contact> contacts = ContactRepository.GetContacts();
listContacts.ItemsSource = contacts;
```

**Side Effects**:
- Fetches all contacts from repository
- Binds contacts to ListView

---

#### Methods

##### `listContacts_ItemSelected(object Sender, SelectedItemChangedEventArgs e)`

```csharp
private async void listContacts_ItemSelected(object Sender, SelectedItemChangedEventArgs e)
```

**Description**: Event handler for contact selection in the list.

**Parameters**:
| Name | Type | Description |
|------|------|-------------|
| `Sender` | `object` | The ListView that triggered the event |
| `e` | `SelectedItemChangedEventArgs` | Contains the selected item |

**Returns**: `void` (async)

**Implementation**:
```csharp
if (listContacts.SelectedItem != null)
{
    await Shell.Current.GoToAsync(
        $"{nameof(EditContactPage)}?Id={((Contact)listContacts.SelectedItem).ContactId}"
    );
}
```

**Navigation**: EditContactPage with ContactId parameter

**Notes**:
- Only navigates if an item is actually selected
- Passes ContactId as query parameter
- Uses async navigation

---

##### `cnlBtn_Clicked(object Sender, EventArgs e)`

```csharp
private void cnlBtn_Clicked(object Sender, EventArgs e)
```

**Description**: Event handler for the Cancel button. Returns to home page.

**Parameters**:
| Name | Type | Description |
|------|------|-------------|
| `Sender` | `object` | The button that triggered the event |
| `e` | `EventArgs` | Event arguments |

**Returns**: `void`

**Implementation**:
```csharp
Shell.Current.GoToAsync($"//{nameof(HomePage)}");
```

**Navigation**: HomePage (absolute navigation, clears back stack)

---

#### UI Elements

| Element | Name | Type | Description |
|---------|------|------|-------------|
| Label | - | `Label` | Header text |
| ListView | `listContacts` | `ListView` | Displays contact list |
| Cancel Button | `cnlBtn` | `Button` | Returns to home |

#### ListView Configuration

**Properties**:
- `BackgroundColor`: Transparent
- `SeparatorColor`: Aqua
- `RowHeight`: 100
- `ItemSelected`: `listContacts_ItemSelected`

**ItemTemplate**:
```xml
<DataTemplate>
    <TextCell Text="{Binding Name}"
              Detail="{Binding Email}" />
</DataTemplate>
```

---

### EditContactPage

**Namespace**: `BCSQuiz.Views`

**File**: `Views/EditContactPage.xaml`, `Views/EditContactPage.xaml.cs`

**Description**: Displays and allows editing of contact details (editing not yet functional).

#### Class Definition

```csharp
[QueryProperty(nameof(ContactId), "Id")]
public partial class EditContactPage : ContentPage
{
    private Contact contact;
    
    public EditContactPage();
    public string ContactId { set; }
    
    private void cnlBtn_Clicked(object sender, EventArgs e);
}
```

---

#### Attributes

##### `[QueryProperty(nameof(ContactId), "Id")]`

**Description**: Marks the page to receive a query parameter named "Id" that will be assigned to the `ContactId` property.

**Usage in Navigation**:
```csharp
await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id=1");
```

---

#### Fields

##### `contact`

```csharp
private Contact contact;
```

**Type**: `Contact`

**Description**: Stores the contact being viewed/edited.

**Scope**: Private

---

#### Constructor

##### `EditContactPage()`

```csharp
public EditContactPage()
```

**Description**: Initializes the edit contact page.

**Implementation**:
```csharp
InitializeComponent();
```

**Notes**:
- Contact data is loaded via the ContactId property setter
- Constructor does not load any data

---

#### Properties

##### `ContactId` (Write-only)

```csharp
public string ContactId { set; }
```

**Type**: `string` (write-only property)

**Description**: Receives the contact ID from navigation and loads the contact data.

**Usage**: Automatically set by Shell navigation when page is navigated to with query parameter.

**Implementation**:
```csharp
set
{
    contact = ContactRepository.GetContactById(int.Parse(value));
    lblName.Text = contact.Name;
    entName.Text = contact.Name;
    entEmail.Text = contact.Email;
}
```

**Side Effects**:
- Fetches contact from repository
- Updates UI elements with contact data
- Stores contact in private field

**Error Handling**: 
- ⚠️ No null checking for contact
- ⚠️ No try-catch for int.Parse (will crash if Id is invalid)

**Recommended Enhancement**:
```csharp
set
{
    if (int.TryParse(value, out int id))
    {
        contact = ContactRepository.GetContactById(id);
        if (contact != null)
        {
            lblName.Text = contact.Name;
            entName.Text = contact.Name;
            entEmail.Text = contact.Email;
        }
    }
}
```

---

#### Methods

##### `cnlBtn_Clicked(object sender, EventArgs e)`

```csharp
private void cnlBtn_Clicked(object sender, EventArgs e)
```

**Description**: Event handler for the Cancel button. Returns to home page.

**Parameters**:
| Name | Type | Description |
|------|------|-------------|
| `sender` | `object` | The button that triggered the event |
| `e` | `EventArgs` | Event arguments |

**Returns**: `void`

**Implementation**:
```csharp
Shell.Current.GoToAsync($"//{nameof(HomePage)}");
```

**Navigation**: HomePage (absolute navigation)

---

#### UI Elements

| Element | Name | Type | Description |
|---------|------|------|-------------|
| Label | `lblName` | `Label` | Displays contact name as title |
| Entry | `entName` | `Entry` | Name input field |
| Entry | `entEmail` | `Entry` | Email input field (keyboard: Email) |
| Cancel Button | `cnlBtn` | `Button` | Returns to home |
| Save Button | `saveBtn` | `Button` | ❌ No handler (not functional) |

---

### MainPage

**Namespace**: `BCSQuiz`

**File**: `MainPage.xaml`, `MainPage.xaml.cs`

**Description**: Default MAUI template page (not used in the application).

#### Class Definition

```csharp
public partial class MainPage : ContentPage
{
    int count = 0;
    
    public MainPage();
    private void OnCounterClicked(object sender, EventArgs e);
}
```

**Note**: This page is part of the default MAUI template and is not used in the actual application navigation.

---

## Navigation

### AppShell

**Namespace**: `BCSQuiz`

**File**: `AppShell.xaml`, `AppShell.xaml.cs`

**Description**: Shell navigation container that manages app-wide navigation.

#### Class Definition

```csharp
public partial class AppShell : Shell
{
    public AppShell();
}
```

---

#### Constructor

##### `AppShell()`

```csharp
public AppShell()
```

**Description**: Initializes the app shell and registers navigation routes.

**Implementation**:
```csharp
InitializeComponent();
Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
Routing.RegisterRoute(nameof(EditContactPage), typeof(EditContactPage));
Routing.RegisterRoute(nameof(ContactPage), typeof(ContactPage));
```

**Registered Routes**:
| Route Name | Page Type | Navigation URI |
|------------|-----------|----------------|
| `HomePage` | `HomePage` | `HomePage` |
| `ContactPage` | `ContactPage` | `ContactPage` |
| `EditContactPage` | `EditContactPage` | `EditContactPage?Id={id}` |

---

### Navigation Methods

All navigation is performed using the Shell navigation API.

#### `Shell.Current.GoToAsync(string route)`

**Description**: Navigates to a specified route.

**Signature**:
```csharp
public static Task GoToAsync(string state)
```

**Parameters**:
| Name | Type | Description |
|------|------|-------------|
| `route` | `string` | The navigation route (relative or absolute) |

**Returns**: `Task` - Awaitable task

**Examples**:

##### Relative Navigation
```csharp
await Shell.Current.GoToAsync(nameof(ContactPage));
// Pushes ContactPage onto the navigation stack
```

##### Absolute Navigation
```csharp
await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
// Returns to HomePage and clears the navigation stack
```

##### Navigation with Parameters
```csharp
await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id=1");
// Navigates to EditContactPage passing Id as query parameter
```

---

## App Lifecycle

### App Class

**Namespace**: `BCSQuiz`

**File**: `App.xaml`, `App.xaml.cs`

**Description**: Main application class that initializes the app.

#### Class Definition

```csharp
public partial class App : Application
{
    public App();
}
```

---

#### Constructor

##### `App()`

```csharp
public App()
```

**Description**: Initializes the application and sets the main page.

**Implementation**:
```csharp
InitializeComponent();
MainPage = new AppShell();
```

**Side Effects**:
- Initializes XAML components
- Sets AppShell as the application's main page
- Loads merged resource dictionaries (Colors, Styles)

---

### MauiProgram Class

**Namespace**: `BCSQuiz`

**File**: `MauiProgram.cs`

**Description**: Configures and builds the MAUI application.

#### Class Definition

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp();
}
```

---

#### Methods

##### `CreateMauiApp()`

```csharp
public static MauiApp CreateMauiApp()
```

**Description**: Creates and configures the MAUI application.

**Returns**: `MauiApp` - Configured application instance

**Implementation**:
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

**Configuration**:
- Sets `App` as the application class
- Registers OpenSans fonts
- Adds debug logging in debug builds

---

## Extension Points

### Adding New Pages

To add a new page to the application:

1. **Create XAML and Code-Behind**:
```csharp
// NewPage.xaml.cs
namespace BCSQuiz.Views;

public partial class NewPage : ContentPage
{
    public NewPage()
    {
        InitializeComponent();
    }
}
```

2. **Register Route in AppShell**:
```csharp
Routing.RegisterRoute(nameof(NewPage), typeof(NewPage));
```

3. **Navigate to the Page**:
```csharp
await Shell.Current.GoToAsync(nameof(NewPage));
```

---

### Adding Repository Methods

To extend the repository functionality:

```csharp
// In ContactRepository class
public static void AddContact(Contact contact)
{
    int maxId = _contacts.Any() ? _contacts.Max(c => c.ContactId) : 0;
    contact.ContactId = maxId + 1;
    _contacts.Add(contact);
}

public static bool UpdateContact(Contact contact)
{
    var existing = GetContactById(contact.ContactId);
    if (existing != null)
    {
        existing.Name = contact.Name;
        existing.Email = contact.Email;
        return true;
    }
    return false;
}

public static bool DeleteContact(int contactId)
{
    var contact = GetContactById(contactId);
    if (contact != null)
    {
        _contacts.Remove(contact);
        return true;
    }
    return false;
}
```

---

## Error Handling

### Current State

⚠️ **Limited error handling throughout the application**

**Issues**:
- No try-catch blocks
- No null checking in critical paths
- No validation of user input
- No error messages to users

### Recommended Improvements

```csharp
// Example: Improved navigation with error handling
try
{
    await Shell.Current.GoToAsync(nameof(ContactPage));
}
catch (Exception ex)
{
    await DisplayAlert("Error", "Failed to navigate to contacts", "OK");
    // Log error
}

// Example: Improved data loading with error handling
try
{
    var contacts = ContactRepository.GetContacts();
    if (contacts == null || contacts.Count == 0)
    {
        await DisplayAlert("Info", "No contacts found", "OK");
        return;
    }
    listContacts.ItemsSource = contacts;
}
catch (Exception ex)
{
    await DisplayAlert("Error", "Failed to load contacts", "OK");
    // Log error
}
```

---

## Best Practices

When working with this codebase:

1. **Always check for null** when retrieving contacts
2. **Use async/await** for navigation and data operations
3. **Handle exceptions** appropriately
4. **Validate input** before processing
5. **Use nameof()** for type-safe navigation routes
6. **Consider thread safety** when accessing static data

---

## Common Usage Patterns

### Pattern 1: Navigate to a Page

```csharp
await Shell.Current.GoToAsync(nameof(ContactPage));
```

### Pattern 2: Navigate with Parameters

```csharp
await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={contactId}");
```

### Pattern 3: Return to Root

```csharp
await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
```

### Pattern 4: Load Data on Page Load

```csharp
public MyPage()
{
    InitializeComponent();
    LoadData();
}

private void LoadData()
{
    var data = ContactRepository.GetContacts();
    myListView.ItemsSource = data;
}
```

### Pattern 5: Handle Item Selection

```csharp
private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
{
    if (e.SelectedItem is Contact contact)
    {
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Id={contact.ContactId}");
    }
}
```

---

*Last Updated: December 2024*
