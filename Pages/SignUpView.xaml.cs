namespace i_am.Pages;

public partial class SignUpView : ContentPage
{
    private readonly SignUpViewModel _viewModel;

    public SignUpView(SignUpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.ClearFieldsIfNeeded();
    }
}