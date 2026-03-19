namespace i_am.Pages;

public partial class SignUpView : ContentPage
{
	public SignUpView(SignUpViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}