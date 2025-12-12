using BCSQuiz.Models;
using Contact = BCSQuiz.Models.Contact;

namespace BCSQuiz.Views;

public partial class ContactPage : ContentPage
{
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
}