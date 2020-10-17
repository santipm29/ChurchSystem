using ChurchSystem.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurchSystem.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        public ModifyUserPageViewModel(
            INavigationService navigationService,
            IApiService apiService
        ) : base(navigationService)
        {
            Title = "Modify User";
            _apiService = apiService;
            _navigationService = navigationService;
        }
    }
}
