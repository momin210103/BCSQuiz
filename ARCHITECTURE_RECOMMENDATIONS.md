# Architecture Recommendations - BCSQuiz

## Current Architecture

### Application Type
- **Framework:** .NET MAUI (Multi-platform App UI)
- **Pattern:** Code-behind (No MVVM)
- **Data Storage:** In-memory static list
- **Navigation:** Shell-based navigation

### Current Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                        Application                           │
│                         (App.xaml)                          │
└────────────────────┬────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────────┐
│                      AppShell (Shell)                        │
│                  - Navigation Framework                      │
│                  - Route Registration                        │
└───┬──────────────────┬─────────────────────┬────────────────┘
    │                  │                     │
    ▼                  ▼                     ▼
┌──────────┐     ┌────────────┐     ┌──────────────┐
│ HomePage │────▶│ContactPage │────▶│EditContact   │
│  (XAML)  │     │   (XAML)   │     │Page (XAML)   │
│   .cs    │     │    .cs     │     │    .cs       │
└────┬─────┘     └─────┬──────┘     └──────┬───────┘
     │                 │                    │
     └─────────────────┴────────────────────┘
                       │
                       ▼
              ┌─────────────────┐
              │ ContactRepository│  (Static)
              │  - GetContacts() │
              │  - GetById()     │
              └────────┬─────────┘
                       │
                       ▼
                 ┌──────────┐
                 │ Contact  │  (Model)
                 │ - Id     │
                 │ - Name   │
                 │ - Email  │
                 └──────────┘
```

## Problems with Current Architecture

### 1. No Separation of Concerns
- **Issue:** Business logic mixed with UI code
- **Impact:** Hard to test, maintain, and reuse
- **Example:** Data loading in code-behind constructors

### 2. Static Data Repository
- **Issue:** Global mutable state
- **Impact:** Thread safety issues, testing difficulties
- **Example:** `ContactRepository._contacts` is public static

### 3. No Data Persistence
- **Issue:** Data lost on app restart
- **Impact:** Not suitable for real-world use
- **Current:** In-memory List<Contact>

### 4. Direct View-to-Model Coupling
- **Issue:** Views directly access repository
- **Impact:** Cannot change data source without modifying views
- **Example:** `ContactRepository.GetContacts()` called in view code-behind

### 5. No Dependency Injection
- **Issue:** Hard dependencies throughout
- **Impact:** Cannot mock services for testing
- **Current:** Static classes everywhere

## Recommended Architecture: MVVM with Clean Architecture

### Proposed Architecture Diagram

```
┌───────────────────────────────────────────────────────────────┐
│                     Presentation Layer                         │
├───────────────────────────────────────────────────────────────┤
│                                                                │
│  ┌────────┐         ┌────────────┐         ┌──────────────┐  │
│  │HomePage│◄───────▶│HomeViewModel│         │ Converters   │  │
│  │ (XAML) │         │             │         │ & Behaviors  │  │
│  └────────┘         └──────┬──────┘         └──────────────┘  │
│                            │                                   │
│  ┌────────────┐      ┌─────────────────┐                      │
│  │ContactPage │◄────▶│ContactViewModel │                      │
│  │   (XAML)   │      │                 │                      │
│  └────────────┘      └────────┬────────┘                      │
│                               │                                │
│  ┌──────────────┐    ┌────────────────────┐                   │
│  │EditContact   │◄──▶│EditContactViewModel│                   │
│  │Page (XAML)   │    │                    │                   │
│  └──────────────┘    └──────────┬─────────┘                   │
│                                 │                              │
└─────────────────────────────────┼──────────────────────────────┘
                                  │
                    ┌─────────────▼──────────────┐
                    │   Application Services      │
                    ├────────────────────────────┤
                    │  - Navigation Service      │
                    │  - Dialog Service          │
                    │  - Validation Service      │
                    └──────────┬──────────────────┘
                               │
┌──────────────────────────────┼──────────────────────────────────┐
│                    Business Logic Layer                          │
├──────────────────────────────┼──────────────────────────────────┤
│                              │                                   │
│                    ┌─────────▼─────────┐                         │
│                    │  IContactService  │ (Interface)             │
│                    └─────────┬─────────┘                         │
│                              │                                   │
│                    ┌─────────▼─────────┐                         │
│                    │  ContactService   │ (Implementation)        │
│                    │  - GetAll()       │                         │
│                    │  - GetById()      │                         │
│                    │  - Add()          │                         │
│                    │  - Update()       │                         │
│                    │  - Delete()       │                         │
│                    └─────────┬─────────┘                         │
│                              │                                   │
└──────────────────────────────┼──────────────────────────────────┘
                               │
┌──────────────────────────────┼──────────────────────────────────┐
│                       Data Access Layer                          │
├──────────────────────────────┼──────────────────────────────────┤
│                              │                                   │
│                    ┌─────────▼─────────────┐                    │
│                    │ IContactRepository    │ (Interface)        │
│                    └─────────┬─────────────┘                    │
│                              │                                   │
│          ┌───────────────────┴───────────────────┐              │
│          │                                       │              │
│  ┌───────▼────────────┐             ┌───────────▼──────────┐   │
│  │ SqliteRepository   │             │ InMemoryRepository   │   │
│  │ (Production)       │             │ (Testing)            │   │
│  └───────┬────────────┘             └──────────────────────┘   │
│          │                                                      │
└──────────┼──────────────────────────────────────────────────────┘
           │
┌──────────▼──────────────────────────────────────────────────────┐
│                        Data Layer                                │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────┐         ┌──────────────┐                      │
│  │   Contact    │         │  SQLite DB   │                      │
│  │   (Entity)   │◄───────▶│  contacts.db │                      │
│  └──────────────┘         └──────────────┘                      │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘
```

## Implementation Roadmap

### Phase 1: Refactor to MVVM (1-2 weeks)

#### Step 1.1: Add ViewModels
Create base ViewModel class:
```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
```

Create ViewModels for each page:
```csharp
// ViewModels/HomeViewModel.cs
public class HomeViewModel : BaseViewModel
{
    public ICommand NavigateToContactsCommand { get; }
    public ICommand NavigateToCategoriesCommand { get; }
}

// ViewModels/ContactListViewModel.cs
public class ContactListViewModel : BaseViewModel
{
    private readonly IContactService _contactService;
    public ObservableCollection<Contact> Contacts { get; }
    public ICommand SelectContactCommand { get; }
}

// ViewModels/EditContactViewModel.cs
public class EditContactViewModel : BaseViewModel
{
    private readonly IContactService _contactService;
    private Contact _contact;
    
    public string Name { get; set; }
    public string Email { get; set; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
}
```

#### Step 1.2: Move Logic to ViewModels
- Remove business logic from code-behind
- Implement INotifyPropertyChanged
- Use Commands instead of event handlers

### Phase 2: Add Service Layer (Week 2)

#### Step 2.1: Define Interfaces
```csharp
public interface IContactService
{
    Task<List<Contact>> GetAllContactsAsync();
    Task<Contact> GetContactByIdAsync(int id);
    Task AddContactAsync(Contact contact);
    Task UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(int id);
}
```

#### Step 2.2: Implement Services
```csharp
public class ContactService : IContactService
{
    private readonly IContactRepository _repository;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Contact>> GetAllContactsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        // Add validation logic here
        if (string.IsNullOrWhiteSpace(contact.Name))
            throw new ArgumentException("Name is required");

        if (string.IsNullOrWhiteSpace(contact.Email))
            throw new ArgumentException("Email is required");

        if (!IsValidEmail(contact.Email))
            throw new ArgumentException("Invalid email format");

        await _repository.UpdateAsync(contact);
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
```

### Phase 3: Add Data Persistence (Week 3)

#### Step 3.1: Add SQLite
Add package reference to BCSQuiz.csproj:
```xml
<PackageReference Include="sqlite-net-pcl" Version="1.9.172" />
<PackageReference Include="SQLitePCLRaw.bundle_green" Version="2.1.8" />
```

#### Step 3.2: Create Repository Interface
```csharp
public interface IContactRepository
{
    Task<List<Contact>> GetAllAsync();
    Task<Contact> GetByIdAsync(int id);
    Task<int> AddAsync(Contact contact);
    Task<int> UpdateAsync(Contact contact);
    Task<int> DeleteAsync(int id);
}
```

#### Step 3.3: Implement SQLite Repository
```csharp
public class SqliteContactRepository : IContactRepository
{
    private readonly SQLiteAsyncConnection _database;

    public SqliteContactRepository(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Contact>().Wait();
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        return await _database.Table<Contact>().ToListAsync();
    }

    public async Task<Contact> GetByIdAsync(int id)
    {
        return await _database.Table<Contact>()
            .Where(c => c.ContactId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(Contact contact)
    {
        return await _database.InsertAsync(contact);
    }

    public async Task<int> UpdateAsync(Contact contact)
    {
        return await _database.UpdateAsync(contact);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _database.DeleteAsync<Contact>(id);
    }
}
```

### Phase 4: Add Dependency Injection (Week 3)

#### Step 4.1: Configure DI in MauiProgram.cs
```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        string dbPath = Path.Combine(
            FileSystem.AppDataDirectory, 
            "contacts.db3");

        builder.Services.AddSingleton<IContactRepository>(
            new SqliteContactRepository(dbPath));
        builder.Services.AddSingleton<IContactService, ContactService>();
        
        // Register ViewModels
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<ContactListViewModel>();
        builder.Services.AddTransient<EditContactViewModel>();
        
        // Register Views
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<ContactPage>();
        builder.Services.AddTransient<EditContactPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
```

#### Step 4.2: Use DI in Views
```csharp
public partial class ContactPage : ContentPage
{
    private readonly ContactListViewModel _viewModel;

    public ContactPage(ContactListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadContactsAsync();
    }
}
```

### Phase 5: Add Testing (Week 4)

#### Step 5.1: Create Test Project
```bash
dotnet new xunit -n BCSQuiz.Tests
dotnet sln add BCSQuiz.Tests/BCSQuiz.Tests.csproj
```

#### Step 5.2: Add Unit Tests
```csharp
public class ContactServiceTests
{
    [Fact]
    public async Task UpdateContact_WithValidData_ShouldSucceed()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = new ContactService(repository);
        var contact = new Contact 
        { 
            ContactId = 1, 
            Name = "Test", 
            Email = "test@example.com" 
        };

        // Act
        await service.UpdateContactAsync(contact);

        // Assert
        var updated = await repository.GetByIdAsync(1);
        Assert.Equal("Test", updated.Name);
        Assert.Equal("test@example.com", updated.Email);
    }

    [Fact]
    public async Task UpdateContact_WithInvalidEmail_ShouldThrow()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = new ContactService(repository);
        var contact = new Contact 
        { 
            ContactId = 1, 
            Name = "Test", 
            Email = "invalid-email" 
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.UpdateContactAsync(contact));
    }
}
```

## Folder Structure (Recommended)

```
BCSQuiz/
├── App.xaml / App.xaml.cs
├── AppShell.xaml / AppShell.xaml.cs
├── MauiProgram.cs
│
├── Models/
│   └── Contact.cs
│
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── HomeViewModel.cs
│   ├── ContactListViewModel.cs
│   └── EditContactViewModel.cs
│
├── Views/
│   ├── HomePage.xaml / .xaml.cs
│   ├── ContactPage.xaml / .xaml.cs
│   └── EditContactPage.xaml / .xaml.cs
│
├── Services/
│   ├── Interfaces/
│   │   ├── IContactService.cs
│   │   └── INavigationService.cs
│   └── Implementations/
│       ├── ContactService.cs
│       └── NavigationService.cs
│
├── Repositories/
│   ├── Interfaces/
│   │   └── IContactRepository.cs
│   └── Implementations/
│       ├── SqliteContactRepository.cs
│       └── InMemoryContactRepository.cs (for testing)
│
├── Converters/
│   └── BoolToColorConverter.cs
│
├── Behaviors/
│   └── EmailValidationBehavior.cs
│
└── Resources/
    ├── Images/
    ├── Fonts/
    ├── Styles/
    └── AppIcon/
```

## Benefits of Proposed Architecture

### 1. Testability
- ✅ Unit test ViewModels independently
- ✅ Mock services and repositories
- ✅ Test business logic without UI

### 2. Maintainability
- ✅ Clear separation of concerns
- ✅ Easy to locate and fix bugs
- ✅ Changes isolated to specific layers

### 3. Scalability
- ✅ Easy to add new features
- ✅ Can switch data sources without changing UI
- ✅ Multiple developers can work independently

### 4. Reusability
- ✅ ViewModels can be reused across platforms
- ✅ Services can be shared between features
- ✅ Business logic independent of UI

### 5. Data Persistence
- ✅ Data survives app restarts
- ✅ Can sync with cloud services later
- ✅ Offline-first capability

## Estimated Migration Effort

| Phase | Description | Time | Complexity |
|-------|-------------|------|------------|
| Phase 1 | MVVM Pattern | 1-2 weeks | Medium |
| Phase 2 | Service Layer | 3-5 days | Low |
| Phase 3 | Data Persistence | 1 week | Medium |
| Phase 4 | Dependency Injection | 2-3 days | Low |
| Phase 5 | Testing | 1 week | Medium |

**Total Estimated Time:** 4-6 weeks

## Alternative: Quick Fixes (If Time Constrained)

If full refactoring is not feasible, implement these minimal changes:

1. **Make repository private** (1 hour)
2. **Add UpdateContact method** (2 hours)
3. **Implement Save button** (2 hours)
4. **Add SQLite with direct access** (1 day)
5. **Basic validation** (4 hours)

**Minimum Fix Time:** 2-3 days

This gets you a working app without full architectural refactor.

## Conclusion

The recommended MVVM + Clean Architecture approach provides:
- Professional code structure
- Long-term maintainability
- Testability
- Scalability

However, it requires 4-6 weeks of development time. For immediate functionality, consider the quick fixes approach first, then gradually refactor to the full architecture.

---

**Document Version:** 1.0  
**Last Updated:** December 12, 2025
