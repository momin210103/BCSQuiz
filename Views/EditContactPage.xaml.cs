using BCSQuiz.Models;
using Contact = BCSQuiz.Models.Contact;

namespace BCSQuiz.Views;
[QueryProperty(nameof(ContactId),"Id")]
public partial class EditContactPage : ContentPage
{
    private Contact contact;
	public EditContactPage()
	{
		InitializeComponent();
	}
    private  void  cnlBtn_Clicked(object sender, EventArgs e)
    {

        Shell.Current.GoToAsync($"//{nameof(HomePage)}");

    }
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

}