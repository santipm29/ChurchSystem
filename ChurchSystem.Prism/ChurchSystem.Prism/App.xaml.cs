using Prism;
using Prism.Ioc;
using ChurchSystem.Prism.ViewModels;
using ChurchSystem.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using ChurchSystem.Common.Services;
using ChurchSystem.Prism.Helpers;
using Newtonsoft.Json;
using ChurchSystem.Common.Responses;
using ChurchSystem.Common.Helpers;
using System;
using Syncfusion.Licensing;

namespace ChurchSystem.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzM3MDM1QDMxMzgyZTMzMmUzMExWRWVPeXg1UnNLSkJvdFFwSVB5QlRsNVBva1VWK0NHb2h6QzhUcHFWd0E9");
            InitializeComponent();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            if (Settings.IsLogin && token?.Expiration > DateTime.Now)
            {
                await NavigationService.NavigateAsync("/ChurchMasterDetailPage/NavigationPage/MeetingPage");
            }
            else
            {
                await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RecoverPasswordPage, RecoverPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ChurchMasterDetailPage, ChurchMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<MembersPage, MembersPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<MeetingPage, MeetingPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
        }
    }
}
