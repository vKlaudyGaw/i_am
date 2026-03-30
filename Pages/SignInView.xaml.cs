namespace i_am.Pages;

public partial class SignInView : ContentPage
{
    private readonly SignInViewModel _viewModel;

    public SignInView(SignInViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Najpierw sprawdŸ auto-login
        bool autoLoginSuccessful = await _viewModel.CheckAutoLoginAsync();
        
        // Jeœli auto-login nie nast¹pi³, wyczyœæ pola
        if (!autoLoginSuccessful)
        {
            _viewModel.ClearFields();
        }
    }
}