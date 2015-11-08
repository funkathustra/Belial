using Belial.Services.MediaCenterServices;
using System;
using Windows.UI.Xaml;

namespace Belial.ViewModels
{
    public class SettingsPageViewModel : Belial.Mvvm.ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : Mvvm.ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = Services.SettingsServices.SettingsService.Instance;
        }

        public bool UseShellBackButton
        {
            get { return _settings.UseShellBackButton; }
            set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }

        private string _BusyText = "Please wait...";
        public string BusyText
        {
            get { return _BusyText; }
            set { Set(ref _BusyText, value); }
        }

        public string ServerAccessKey
        {
            get { return _settings.ServerAccessKey; }
            set { _settings.ServerAccessKey = value; McwsService.Instance.Reconnect(); base.RaisePropertyChanged(); }
        }

        public string ServerUserName
        {
            get { return _settings.ServerUserName; }
            set { _settings.ServerUserName = value; McwsService.Instance.Reconnect(); base.RaisePropertyChanged(); }
        }

        public string ServerPassword
        {
            get { return _settings.ServerPassword; }
            set { _settings.ServerPassword = value; McwsService.Instance.Reconnect(); base.RaisePropertyChanged(); }
        }

        public string ServerIp { get { return McwsService.Instance.ServerIp; } }

        public string ServerPort { get { return McwsService.Instance.ServerPort; } }

        public void ShowBusy()
        {
            Views.Shell.SetBusyVisibility(Visibility.Visible, _BusyText);
        }

        public void HideBusy()
        {
            Views.Shell.SetBusyVisibility(Visibility.Collapsed);
        }
    }

    public class AboutPartViewModel : Mvvm.ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var ver = Windows.ApplicationModel.Package.Current.Id.Version;
                return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
            }
        }

        public Uri RateMe => new Uri("http://bing.com");
    }
}

