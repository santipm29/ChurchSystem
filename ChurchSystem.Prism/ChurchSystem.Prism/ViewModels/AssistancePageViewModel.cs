using ChurchSystem.Common.Entities;
using ChurchSystem.Common.Helpers;
using ChurchSystem.Common.Request;
using ChurchSystem.Common.Responses;
using ChurchSystem.Common.Services;
using ChurchSystem.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace ChurchSystem.Prism.ViewModels
{
    public class AssistancePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private UserResponse _user;
        private ObservableCollection<Assistance> _assistancies;
        private DelegateCommand _getAssistanciesCommand;
        private DelegateCommand _updateAssistanciesCommand;
        public AssistancePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            IsEnabled = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            User = token.User;
            MiniumDate = DateTime.Now;
            Title = Languages.TitleAssistancies;
        }
        public DelegateCommand GetAssistanciesCommand => _getAssistanciesCommand ?? (_getAssistanciesCommand = new DelegateCommand(GetAssistanciesAsync));

        public DelegateCommand UpdateAssistanciesCommand => _updateAssistanciesCommand ?? (_updateAssistanciesCommand = new DelegateCommand(UpdateAssistanciesAsync));


        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ObservableCollection<Assistance> Asistancies
        {
            get => _assistancies;
            set => SetProperty(ref _assistancies, value);
        }

        public DateTime Date { get; set; }
        public DateTime MiniumDate { get; }

        private async void GetAssistanciesAsync()
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
            string url = App.Current.Resources["UrlAPI"].ToString();
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var meetingRequest = new MeetingRequest
            {
                Date = Date
            };
            Response response = await _apiService.PostListAsync<Assistance>(url, "/api", "/Meetings/GetAssistances", meetingRequest, token.Token);

            IsRunning = false;
            IsEnabled = true;

            
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, "There are no assistances on that date", Languages.Accept);
                return;
            }
            
            List<Assistance> assistance = (List<Assistance>)response.Result;
            Asistancies = new ObservableCollection<Assistance>(assistance);
        }

        private void UpdateAssistanciesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
