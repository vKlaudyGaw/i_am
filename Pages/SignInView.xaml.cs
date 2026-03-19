namespace i_am.Pages;

public partial class SignInView : ContentPage
{
	public SignInView(SignInViewModel viewModel)
    {
		InitializeComponent();

		BindingContext = viewModel;
    }
}