namespace BCSQuiz.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private void ctgBtn_Clicked(object sender, EventArgs e)
    {
		//Shell.Current.GoToAsync(nameof(EditContactPage));

    }

    private void strBtn_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(ContactPage));

    }
}