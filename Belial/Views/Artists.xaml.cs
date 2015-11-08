using Belial.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Belial.Views
{
    public sealed partial class Artists : Page
    {
        public Artists()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        // strongly-typed view models enable x:bind
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
    }
}
