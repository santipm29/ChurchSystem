using ChurchSystem.Common.Helpers;
using ChurchSystem.Common.Request;
using ChurchSystem.Common.Responses;
using ChurchSystem.Common.Services;
using ChurchSystem.Prism.Helpers;
using ChurchSystem.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Essentials;

namespace ChurchSystem.Prism.ViewModels
{
    public class CreateMeetingViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _createMeetingCommand;
        public CreateMeetingViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            IsEnabled = true;
            Title = Languages.TitleCreateMeating;
            MiniumDate = DateTime.Now;
        }

        public DelegateCommand CreateMeetingCommand => _createMeetingCommand ?? (_createMeetingCommand = new DelegateCommand(CreateMeetingAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public DateTime Date { get; set; }
        public DateTime MiniumDate { get; }

        private async void CreateMeetingAsync()
        {
            if (string.IsNullOrEmpty(Date.ToString()))
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    "you must enter a date",
                    Languages.Accept);
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            MeetingRequest meetingRequest = new MeetingRequest
            {
                Date = Date
            };
            
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.PostAsync(url, "/api", "/Meetings/Create", meetingRequest, token.Token);
            /*
            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }
            */
            IsRunning = false;
            IsEnabled = true;
            MeetingPageViewModel.GetInstance().LoadMeetingsAsync();
            await _navigationService.NavigateAsync($"/{nameof(ChurchMasterDetailPage)}/NavigationPage/{nameof(MeetingPage)}");
        }
    }
}
