using i_am.ViewModels;

namespace i_am.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(UserVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}