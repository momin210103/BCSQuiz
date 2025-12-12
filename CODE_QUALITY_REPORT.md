# Code Quality Report - BCSQuiz

## Overview
This document provides specific code-level recommendations for improving the BCSQuiz application.

## File-by-File Analysis

### 1. Models/ContactRepository.cs

#### Issue 1: Public Mutable Field
**Severity:** 🔴 Critical  
**Location:** Line 11  

**Current Code:**
```csharp
public static List<Contact> _contacts = new List<Contact>()
```

**Problem:**
- Violates encapsulation principle
- Any code can modify the list directly: `ContactRepository._contacts.Clear()`
- Field naming convention (_contacts) indicates it should be private

**Fix:**
```csharp
private static List<Contact> _contacts = new List<Contact>()
```

#### Issue 2: Hardcoded Test Data with Real Email
**Severity:** ⚠️ Medium  
**Location:** Lines 13-17  

**Current Code:**
```csharp
new Contact { ContactId = 1, Name = "Momin",Email="[personal-email]" },
new Contact { ContactId = 2, Name = "Sazzad",Email="[personal-email]" },
new Contact { ContactId = 3, Name = "Johab", Email = "[personal-email]" }
```

**Problems:**
- Real/personal email addresses in code (privacy concern)
- Duplicate email addresses
- Inconsistent spacing around `=`

**Fix:**
```csharp
new Contact { ContactId = 1, Name = "John Doe", Email = "john.doe@example.com" },
new Contact { ContactId = 2, Name = "Jane Smith", Email = "jane.smith@example.com" },
new Contact { ContactId = 3, Name = "Bob Johnson", Email = "bob.johnson@example.com" }
```

#### Issue 3: Missing CRUD Methods
**Severity:** 🔴 Critical  
**Location:** Entire file  

**Problem:** Only Read operations exist, no Create, Update, or Delete

**Add these methods:**
```csharp
public static void AddContact(Contact contact)
{
    contact.ContactId = _contacts.Any() ? _contacts.Max(c => c.ContactId) + 1 : 1;
    _contacts.Add(contact);
}

public static void UpdateContact(Contact contact)
{
    var existing = _contacts.FirstOrDefault(x => x.ContactId == contact.ContactId);
    if (existing != null)
    {
        existing.Name = contact.Name;
        existing.Email = contact.Email;
    }
}

public static void DeleteContact(int contactId)
{
    var contact = GetContactById(contactId);
    if (contact != null)
    {
        _contacts.Remove(contact);
    }
}
```

#### Issue 4: No Null Safety Warning
**Severity:** ⚠️ Medium  
**Location:** Line 22  

**Current Code:**
```csharp
public static Contact GetContactById(int contactId)
{
    return _contacts.FirstOrDefault(x => x.ContactId == contactId);
}
```

**Problem:** Returns null without indicating it (FirstOrDefault returns null if not found)

**Fix:**
```csharp
public static Contact? GetContactById(int contactId)
{
    return _contacts.FirstOrDefault(x => x.ContactId == contactId);
}
```

#### Issue 5: GetContacts Returns Direct Reference
**Severity:** ⚠️ Medium  
**Location:** Line 19  

**Current Code:**
```csharp
public static List<Contact> GetContacts() => _contacts;
```

**Problem:** Returns direct reference to internal list, allowing external modification

**Fix:**
```csharp
public static List<Contact> GetContacts() => new List<Contact>(_contacts);
```

---

### 2. Views/HomePage.xaml.cs

#### Issue 1: Commented Code
**Severity:** ⚠️ Medium  
**Location:** Line 12  

**Current Code:**
```csharp
private void ctgBtn_Clicked(object sender, EventArgs e)
{
    //Shell.Current.GoToAsync(nameof(EditContactPage));
}
```

**Problem:** Commented code should be removed or implemented

**Fix:** Either implement or remove the method entirely

#### Issue 2: Unclear Variable Names
**Severity:** 💡 Low  
**Location:** Lines 10-17  

**Current Code:**
```csharp
private void ctgBtn_Clicked(object sender, EventArgs e) // ctgBtn = Category button?
private void strBtn_Clicked(object sender, EventArgs e) // strBtn = Start button?
```

**Problem:** Abbreviations make code unclear

**Fix:**
```csharp
private void categoryButton_Clicked(object sender, EventArgs e)
private void startButton_Clicked(object sender, EventArgs e)
```

And update XAML:
```xaml
<Button x:Name="categoryButton" Clicked="categoryButton_Clicked" Text="Category"/>
<Button x:Name="startButton" Text="Start" Clicked="startButton_Clicked"/>
```

---

### 3. Views/HomePage.xaml

#### Issue 1: Placeholder Text
**Severity:** ⚠️ Medium  
**Location:** Line 8  

**Current Code:**
```xaml
<Label 
    Text="Welcome to .NET MAUI!"
    VerticalOptions="Center" 
    HorizontalOptions="Center" />
```

**Problem:** Generic template text

**Fix:**
```xaml
<Label 
    Text="BCS Contact Manager"
    FontSize="24"
    FontAttributes="Bold"
    VerticalOptions="Center" 
    HorizontalOptions="Center" />
```

---

### 4. Views/EditContactPage.xaml

#### Issue: Save Button Has No Click Handler
**Severity:** 🔴 Critical  
**Location:** Line 40  

**Current Code:**
```xaml
<Button x:Name="saveBtn" Text="Save" />
```

**Problem:** Button exists but does nothing when clicked

**Fix:**
```xaml
<Button x:Name="saveBtn" Text="Save" Clicked="saveBtn_Clicked" />
```

---

### 5. Views/EditContactPage.xaml.cs

#### Issue 1: Missing Save Button Click Handler
**Severity:** 🔴 Critical  
**Location:** Missing from file  

**Problem:** Save button has no functionality

**Add this method:**
```csharp
private void saveBtn_Clicked(object sender, EventArgs e)
{
    if (contact != null)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(entName.Text))
        {
            DisplayAlert("Error", "Name is required", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(entEmail.Text))
        {
            DisplayAlert("Error", "Email is required", "OK");
            return;
        }

        // Update contact
        contact.Name = entName.Text;
        contact.Email = entEmail.Text;
        ContactRepository.UpdateContact(contact);

        // Show success and navigate back
        DisplayAlert("Success", "Contact saved successfully", "OK");
        Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}
```

#### Issue 2: Label Misuse
**Severity:** ⚠️ Medium  
**Location:** Line 24  

**Current Code:**
```csharp
lblName.Text = contact.Name;
```

**Problem:** Using label to display contact name when it should be a static page title

**Fix:** Either:
1. Remove this line and keep label as static "Edit Contact"
2. Rename label to better indicate its purpose
```csharp
lblTitle.Text = $"Editing: {contact.Name}";
```

#### Issue 3: Inconsistent Spacing
**Severity:** 💡 Low  
**Location:** Line 13  

**Current Code:**
```csharp
private  void  cnlBtn_Clicked(object sender, EventArgs e)
```

**Problem:** Extra spaces between `private` `void`

**Fix:**
```csharp
private void cnlBtn_Clicked(object sender, EventArgs e)
```

#### Issue 4: Async Method Not Awaited
**Severity:** ⚠️ Medium  
**Location:** Line 16  

**Current Code:**
```csharp
Shell.Current.GoToAsync($"//{nameof(HomePage)}");
```

**Problem:** GoToAsync returns Task but not awaited

**Fix:**
```csharp
private async void cnlBtn_Clicked(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

---

### 6. Views/ContactPage.xaml.cs

#### Issue 1: Inconsistent Parameter Naming
**Severity:** 💡 Low  
**Location:** Lines 16, 24  

**Current Code:**
```csharp
private async void listContacts_ItemSelected(object Sender, SelectedItemChangedEventArgs e)
private void cnlBtn_Clicked(object Sender, EventArgs e)
```

**Problem:** `Sender` should be lowercase `sender` per C# conventions

**Fix:**
```csharp
private async void listContacts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
private void cnlBtn_Clicked(object sender, EventArgs e)
```

#### Issue 2: Async Method Not Awaited
**Severity:** ⚠️ Medium  
**Location:** Line 26  

**Current Code:**
```csharp
private void cnlBtn_Clicked(object Sender, EventArgs e)
{
    Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

**Problem:** GoToAsync returns Task but not awaited

**Fix:**
```csharp
private async void cnlBtn_Clicked(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
}
```

---

### 7. Views/ContactPage.xaml

#### Issue: Placeholder Text
**Severity:** ⚠️ Medium  
**Location:** Lines 7-10  

**Current Code:**
```xaml
<Label 
    Text="Welcome to .NET MAUI!"
    VerticalOptions="Center" 
    HorizontalOptions="Center" />
```

**Problem:** Generic template text

**Fix:**
```xaml
<Label 
    Text="Contact List"
    FontSize="22"
    FontAttributes="Bold"
    VerticalOptions="Center" 
    HorizontalOptions="Center" />
```

---

### 8. Models/Contact.cs

#### Issue: No Validation
**Severity:** ⚠️ Medium  
**Location:** Lines 11-13  

**Current Code:**
```csharp
public int ContactId { get; set; }
public string Name { get; set; } = "";
public string Email { get; set; } = "";
```

**Recommendation:** Add data annotations for validation

**Enhanced Version:**
```csharp
using System.ComponentModel.DataAnnotations;

public class Contact
{
    public int ContactId { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
    public string Name { get; set; } = "";
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = "";
}
```

---

### 9. MainPage.xaml and MainPage.xaml.cs

#### Issue: Unused Files
**Severity:** ⚠️ Medium  
**Location:** Both files  

**Problem:** These are template files not used in the application

**Recommendation:** Delete both files as they're not referenced anywhere

---

## Summary of Required Changes

### Critical (Must Fix Immediately)
1. ✅ Make `_contacts` field private in ContactRepository
2. ✅ Add Save button click handler in EditContactPage
3. ✅ Implement UpdateContact method in ContactRepository
4. ✅ Add input validation in EditContactPage

### High Priority (Fix Soon)
5. ✅ Add AddContact and DeleteContact methods
6. ✅ Remove commented code from HomePage
7. ✅ Fix all async/await warnings
8. ✅ Replace placeholder text in views

### Medium Priority (Improve Code Quality)
9. ✅ Use descriptive variable names instead of abbreviations
10. ✅ Fix parameter naming (Sender → sender)
11. ✅ Return defensive copies from GetContacts
12. ✅ Add nullable return type for GetContactById
13. ✅ Replace real email addresses with example.com

### Low Priority (Polish)
14. ✅ Delete unused MainPage files
15. ✅ Fix spacing inconsistencies
16. ✅ Add XML documentation comments
17. ✅ Add data annotations for validation

## Estimated Time to Fix

- Critical issues: 2-4 hours
- High priority: 4-6 hours  
- Medium priority: 6-8 hours
- Low priority: 2-3 hours

**Total:** 14-21 hours of development time

---

**Report Generated:** December 12, 2025  
**Tool:** GitHub Copilot Code Review Agent
