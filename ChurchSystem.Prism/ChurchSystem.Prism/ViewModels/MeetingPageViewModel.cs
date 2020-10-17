using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurchSystem.Prism.ViewModels
{
    public class MeetingPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public MeetingPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Meetings";

        }
    }
}
