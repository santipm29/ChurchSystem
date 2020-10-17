using ChurchSystem.Common.Helpers;
using ChurchSystem.Common.Models;
using ChurchSystem.Prism.Helpers;
using ChurchSystem.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchSystem.Prism.ItemViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {

            if (PageName.Equals("LoginPage"))
            {
                Settings.IsLogin = false;
                Settings.Token = null;
                await _navigationService.NavigateAsync("/NavigationPage/LoginPage");
                return;
            }

            await _navigationService.NavigateAsync($"/ChurchMasterDetailPage/NavigationPage/{PageName}");
        }
    }

}
