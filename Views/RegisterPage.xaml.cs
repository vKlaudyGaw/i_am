using i_am.ViewModels;

namespace i_am.Views
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