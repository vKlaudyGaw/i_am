using i_am.ViewModels;

namespace i_am.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(UserVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}