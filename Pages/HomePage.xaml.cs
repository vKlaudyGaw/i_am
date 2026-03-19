using i_am.ViewModels;

namespace i_am.Pages
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