# Quick Fix Guide - BCSQuiz

## Purpose
This guide provides step-by-step instructions to fix the most critical issues in the BCSQuiz application. Follow these in order to get a minimally working contact management application.

## Prerequisites
- .NET 8.0 SDK installed
- Visual Studio 2022 or VS Code with C# extension
- Basic understanding of C# and XAML

## Critical Issues to Fix (Priority Order)

### 🔴 Issue #1: Save Button Does Nothing (30 minutes)

**Problem:** Edit contact page has a Save button that doesn't work.

**Files to modify:**
1. `Views/EditContactPage.xaml` - Line 40
2. `Views/EditContactPage.xaml.cs` - Add new method

**Step 1:** Add click handler to XAML
```xaml
<!-- Change line 40 from: -->
<Button x:Name="saveBtn" Text="Save" />

<!-- To: -->
<Button x:Name="saveBtn" Text="Save" Clicked="saveBtn_Clicked" />
```

**Step 2:** Add method in code-behind

Add this method to `Views/EditContactPage.xaml.cs`:

```csharp
private async void saveBtn_Clicked(object sender, EventArgs e)
{
    if (contact == null)
        return;

    // Validate input
    if (string.IsNullOrWhiteSpace(entName.Text))
    {
        await DisplayAlert("Error", "Name is required", "OK");
        return;
    }

    if (string.IsNullOrWhiteSpace(entEmail.Text))
    {
        await DisplayAlert("Error", "Email is required", "OK");
        return;
    }

    // Update contact
    contact.Name = entName.Text;
    contact.Email = entEmail.Text;
    ContactRepository.UpdateContact(contact);

    // Show success and navigate back
    await DisplayAlert("Success", "Contact saved successfully", "OK");
    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

**Step 3:** Update existing method to be async

Change line 13 from:
```csharp
private void cnlBtn_Clicked(object sender, EventArgs e)
{
    Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

To:
```csharp
private async void cnlBtn_Clicked(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

---

### 🔴 Issue #2: Missing UpdateContact Method (15 minutes)

**Problem:** Repository has no Update method, so even with button fixed, saves won't work.

**File to modify:** `Models/ContactRepository.cs`

**Step 1:** Add the UpdateContact method

Add this method after line 23:

```csharp
public static void UpdateContact(Contact contact)
{
    var existing = _contacts.FirstOrDefault(x => x.ContactId == contact.ContactId);
    if (existing != null)
    {
        existing.Name = contact.Name;
        existing.Email = contact.Email;
    }
}
```

---

### 🔴 Issue #3: Public Mutable Field (5 minutes)

**Problem:** `_contacts` field is public, violating encapsulation.

**File to modify:** `Models/ContactRepository.cs`

**Step 1:** Change line 11 from:
```csharp
public static List<Contact> _contacts = new List<Contact>()
```

To:
```csharp
private static List<Contact> _contacts = new List<Contact>()
```

---

### 🔴 Issue #4: Fix Async/Await Warnings (10 minutes)

**Problem:** GoToAsync calls not awaited properly.

**Files to modify:**
1. `Views/ContactPage.xaml.cs`
2. `Views/HomePage.xaml.cs`

**Step 1:** Update ContactPage.xaml.cs

Change lines 24-27 from:
```csharp
private void cnlBtn_Clicked(object Sender, EventArgs e)
{
    Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

To:
```csharp
private async void cnlBtn_Clicked(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

**Step 2:** Update HomePage.xaml.cs

Change lines 17-19 from:
```csharp
private void strBtn_Clicked(object sender, EventArgs e)
{
    Shell.Current.GoToAsync(nameof(ContactPage));
}
```

To:
```csharp
private async void strBtn_Clicked(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync(nameof(ContactPage));
}
```

---

### ⚠️ Issue #5: Replace Test Data with Safe Data (10 minutes)

**Problem:** Real email addresses in code.

**File to modify:** `Models/ContactRepository.cs`

**Step 1:** Replace lines 13-17 with:

```csharp
new Contact { ContactId = 1, Name = "John Doe", Email = "john.doe@example.com" },
new Contact { ContactId = 2, Name = "Jane Smith", Email = "jane.smith@example.com" },
new Contact { ContactId = 3, Name = "Bob Johnson", Email = "bob.johnson@example.com" }
```

---

### ⚠️ Issue #6: Add Nullable Return Type (5 minutes)

**Problem:** GetContactById can return null but doesn't indicate it.

**File to modify:** `Models/ContactRepository.cs`

**Step 1:** Change line 20 from:
```csharp
public static Contact GetContactById(int contactId)
```

To:
```csharp
public static Contact? GetContactById(int contactId)
```

---

### ⚠️ Issue #7: Remove Commented Code (5 minutes)

**Problem:** Empty method with commented code.

**File to modify:** `Views/HomePage.xaml.cs`

**Option A (Recommended):** Remove the entire method and button reference

1. Delete lines 10-14 from HomePage.xaml.cs
2. Delete line 11 from HomePage.xaml (the ctgBtn button)

**Option B:** Keep it but remove comment

Remove the comment on line 12.

---

### ⚠️ Issue #8: Add AddContact Method (10 minutes)

**Problem:** No way to add new contacts.

**File to modify:** `Models/ContactRepository.cs`

**Step 1:** Add method after UpdateContact:

```csharp
public static void AddContact(Contact contact)
{
    contact.ContactId = _contacts.Any() ? _contacts.Max(c => c.ContactId) + 1 : 1;
    _contacts.Add(contact);
}
```

---

### ⚠️ Issue #9: Add DeleteContact Method (10 minutes)

**Problem:** No way to delete contacts.

**File to modify:** `Models/ContactRepository.cs`

**Step 1:** Add method after AddContact:

```csharp
public static void DeleteContact(int contactId)
{
    var contact = GetContactById(contactId);
    if (contact != null)
    {
        _contacts.Remove(contact);
    }
}
```

---

### ⚠️ Issue #10: Delete Unused Files (2 minutes)

**Problem:** MainPage files are not used in the application.

**Step 1:** Delete these files:
- `MainPage.xaml`
- `MainPage.xaml.cs`

Use Git to delete:
```bash
git rm MainPage.xaml MainPage.xaml.cs
git commit -m "Remove unused MainPage files"
```

---

### 💡 Issue #11: Replace Placeholder Text (15 minutes)

**Problem:** Generic "Welcome to .NET MAUI!" text in multiple places.

**Files to modify:**
1. `Views/HomePage.xaml`
2. `Views/ContactPage.xaml`

**Step 1:** Update HomePage.xaml

Change lines 7-10 from:
```xaml
<Label 
    Text="Welcome to .NET MAUI!"
    VerticalOptions="Center" 
    HorizontalOptions="Center" />
```

To:
```xaml
<Label 
    Text="Contact Manager"
    FontSize="24"
    FontAttributes="Bold"
    VerticalOptions="Center" 
    HorizontalOptions="Center"
    Margin="0,20,0,20" />
```

**Step 2:** Update ContactPage.xaml

Change lines 7-10 from:
```xaml
<Label 
    Text="Welcome to .NET MAUI!"
    VerticalOptions="Center" 
    HorizontalOptions="Center" />
```

To:
```xaml
<Label 
    Text="Contact List"
    FontSize="22"
    FontAttributes="Bold"
    VerticalOptions="Center" 
    HorizontalOptions="Center"
    Margin="0,10,0,10" />
```

---

## Testing Your Changes

### Test #1: Edit and Save Contact
1. Run the application
2. Click "Start" button on home page
3. Select a contact from the list
4. Change the name or email
5. Click "Save" button
6. Verify success message appears
7. Go back to contact list
8. Verify changes are saved

### Test #2: Validation
1. Follow steps 1-3 above
2. Clear the Name field
3. Click "Save"
4. Verify error message appears
5. Repeat for Email field

### Test #3: Navigation
1. Run the application
2. Test all navigation buttons work properly
3. Verify no crashes or hanging

---

## Verification Checklist

After implementing all fixes:

- [ ] Save button works and updates contacts
- [ ] Validation prevents empty name/email
- [ ] Success message appears after save
- [ ] All navigation properly uses async/await
- [ ] No compilation warnings
- [ ] Test data uses example.com emails
- [ ] Unused files deleted
- [ ] Placeholder text replaced
- [ ] Manual testing passes all scenarios

---

## Build and Run

### Command Line
```bash
# Navigate to project directory
cd /path/to/BCSQuiz

# Restore dependencies
dotnet restore

# Build project (Windows)
dotnet build -f net8.0-windows10.0.19041.0

# Or for Android
dotnet build -f net8.0-android
```

### Visual Studio
1. Open `BCSQuiz.sln`
2. Select target platform (Android/Windows/iOS)
3. Press F5 to build and run

---

## Time Estimate

| Issue | Time | Difficulty |
|-------|------|------------|
| #1 Save Button | 30 min | Medium |
| #2 UpdateContact | 15 min | Easy |
| #3 Public Field | 5 min | Easy |
| #4 Async/Await | 10 min | Easy |
| #5 Test Data | 10 min | Easy |
| #6 Nullable Type | 5 min | Easy |
| #7 Commented Code | 5 min | Easy |
| #8 AddContact | 10 min | Easy |
| #9 DeleteContact | 10 min | Easy |
| #10 Delete Files | 2 min | Easy |
| #11 Placeholder Text | 15 min | Easy |
| **Testing** | 30 min | - |
| **TOTAL** | **2-3 hours** | - |

---

## Next Steps After Quick Fixes

Once these critical issues are resolved, consider:

1. **Add Data Persistence** - Implement SQLite database (see ARCHITECTURE_RECOMMENDATIONS.md)
2. **Implement MVVM** - Refactor to proper MVVM pattern
3. **Add Unit Tests** - Write tests for repository and business logic
4. **Enhance Validation** - Add email format validation, duplicate checking
5. **Add Features** - Implement search, sort, add new contact UI

---

## Need Help?

- Review the full analysis: `PROJECT_REVIEW.md`
- Check code quality details: `CODE_QUALITY_REPORT.md`
- Learn about architecture improvements: `ARCHITECTURE_RECOMMENDATIONS.md`

---

**Document Version:** 1.0  
**Last Updated:** December 12, 2025  
**Estimated Time to Complete:** 2-3 hours
