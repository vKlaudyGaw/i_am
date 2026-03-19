using i_am.ViewModels;

namespace i_am.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(UserVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}