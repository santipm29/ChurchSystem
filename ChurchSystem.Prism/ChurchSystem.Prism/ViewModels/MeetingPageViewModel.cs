using ChurchSystem.Common.Entities;
using ChurchSystem.Common.Helpers;
using ChurchSystem.Common.Responses;
using ChurchSystem.Common.Services;
using ChurchSystem.Prism.Helpers;
using ChurchSystem.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace ChurchSystem.Prism.ViewModels
{
    public class MeetingPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private bool _isRunning;
        private bool _isEnabled;
        private static MeetingPageViewModel _instance;
        private ObservableCollection<Meeting> _meetings;
        private DelegateCommand _selectMeetingCommand;
        public MeetingPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _navigationService = navigationService;
            _apiService = apiService;
            IsEnabled = true;
            _instance = this;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            User = token.User;
            Title = Languages.TitleMeetings;
            LoadMeetingsAsync();
        }

        public DelegateCommand SelectMeetingCommand => _selectMeetingCommand ?? (_selectMeetingCommand = new DelegateCommand(ShowAssistancesAsync));

        public static MeetingPageViewModel GetInstance()
        {
            return _instance;
        }
        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

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

        public ObservableCollection<Meeting> Meetings
        {
            get => _meetings;
            set => SetProperty(ref _meetings, value);
        }
        public async void LoadMeetingsAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            string url = App.Current.Resources["UrlAPI"].ToString();
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            Response response = await _apiService.GetListTokenAsync<Meeting>(url, "/api", "/Meetings", token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            List<Meeting> meetings = (List<Meeting>)response.Result;
            Meetings = new ObservableCollection<Meeting>(meetings);
        }
        private async void ShowAssistancesAsync()
        {
            await _navigationService.NavigateAsync(nameof(AssistancePage));
        }
    }
}
