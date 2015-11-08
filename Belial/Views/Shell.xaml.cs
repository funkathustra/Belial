using System;
using System.ComponentModel;
using System.Linq;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Belial.Services.SettingsServices;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Data;

namespace Belial.Views
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu { get { return Instance.MyHamburgerMenu; } }

        public Shell(NavigationService navigationService)
        {
            Instance = this;
            InitializeComponent();
            MyHamburgerMenu.NavigationService = navigationService;
            VisualStateManager.GoToState(Instance, Instance.Narrow.Name, true);

            BindingOperations.SetBinding(this, RightSideIsOpenProperty, new Binding() { Source = RightSplitView, Path = new PropertyPath("IsPaneOpen"), Mode = BindingMode.TwoWay });

        }

        private static readonly DependencyProperty RightSideIsOpenProperty = DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(Shell), new PropertyMetadata(null, ValuePropertyChanged));

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var watcher = d as Shell;
            if (watcher == null)
                return;

            if ((bool)e.NewValue)
                VisualStateManager.GoToState(Instance, Instance.RightPanelOpen.Name, true);
            else
                VisualStateManager.GoToState(Instance, Instance.RightPanelClosed.Name, true);
        }

        public static void SetBusyVisibility(Visibility visible, string text = null)
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                switch (visible)
                {
                    case Visibility.Visible:
                        Instance.FindName(nameof(BusyScreen));
                        Instance.BusyText.Text = text ?? string.Empty;
                        if (VisualStateManager.GoToState(Instance, Instance.BusyVisualState.Name, true))
                        {
                            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                                AppViewBackButtonVisibility.Collapsed;
                        }
                        break;
                    case Visibility.Collapsed:
                        if (VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true))
                        {
                            BootStrapper.Current.UpdateShellBackButton();
                        }
                        break;
                }
            });
        }

        private void StackPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // open the pane if it's not open
            if(RightSplitView.IsPaneOpen == false)
            {
                RightSplitView.IsPaneOpen = true;
            }
            else
            {
                // collapse the window
                if (RightSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
                {
                    RightSplitView.IsPaneOpen = false;
                }
            }
        }
    }
}

