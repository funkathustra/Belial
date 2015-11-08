using System;
using Windows.UI.Xaml;
using Belial.Services.MediaCenterServices;

namespace Belial.Services.SettingsServices
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
    public partial class SettingsService : ISettingsService
    {
        public static SettingsService Instance { get; }
        static SettingsService()
        {
            // implement singleton pattern
            Instance = Instance ?? new SettingsService();
        }

        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        // Server stuff that needs to persist
        public string ServerAccessKey
        {
            get { return _helper.Read<string>(nameof(ServerAccessKey), "", Template10.Services.SettingsService.SettingsStrategies.Roam); }
            set
            {
                _helper.Write(nameof(ServerAccessKey), value, Template10.Services.SettingsService.SettingsStrategies.Roam);
                McwsService.Instance.AccessKey = value;
            }
        }

        public string ServerUserName
        {
            get { return _helper.Read<string>(nameof(ServerUserName), "", Template10.Services.SettingsService.SettingsStrategies.Roam); }
            set
            {
                _helper.Write(nameof(ServerUserName), value, Template10.Services.SettingsService.SettingsStrategies.Roam);
                McwsService.Instance.UserName = value;
            }
        }

        public string ServerPassword
        {
            get { return _helper.Read<string>(nameof(ServerPassword), "", Template10.Services.SettingsService.SettingsStrategies.Roam); }
            set
            {
                _helper.Write(nameof(ServerPassword), value, Template10.Services.SettingsService.SettingsStrategies.Roam);
                McwsService.Instance.Password = value;
            }
        }


        public bool UseShellBackButton
        {
            get {
                return _helper.Read<bool>(nameof(UseShellBackButton), true);
            }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                ApplyUseShellBackButton(value);
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Dark;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                ApplyAppTheme(value);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                ApplyCacheMaxDuration(value);
            }
        }
    }
}

