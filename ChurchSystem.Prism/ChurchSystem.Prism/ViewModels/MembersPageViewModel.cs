using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurchSystem.Prism.ViewModels
{
    public class MembersPageViewModel : ViewModelBase
    {
        public MembersPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Members";
        }
    }
}
