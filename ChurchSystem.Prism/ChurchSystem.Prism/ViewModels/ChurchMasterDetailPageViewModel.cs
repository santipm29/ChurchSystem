using ChurchSystem.Common.Enums;
using ChurchSystem.Common.Helpers;
using ChurchSystem.Common.Models;
using ChurchSystem.Common.Responses;
using ChurchSystem.Prism.Helpers;
using ChurchSystem.Prism.ItemViewModels;
using ChurchSystem.Prism.Views;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChurchSystem.Prism.ViewModels
{
    public class ChurchMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private static ChurchMasterDetailPageViewModel _instance;

        public ChurchMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            LoadUser();
            LoadMenus();
        }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public static ChurchMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }
        public void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.User;
            }
        }
        private void LoadMenus()
        {
            
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_person",
                    PageName = $"{nameof(ModifyUserPage)}",
                    Title = Languages.TitleProfile
                },
                 new Menu
                {
                    Icon = "ic_event_note",
                    PageName = $"{nameof(MeetingPage)}",
                    Title = Languages.TitleMeetings
                },
                 new Menu
                {
                    Icon = "ic_meeting",
                    PageName = $"{nameof(AssistancePage)}",
                    Title = Languages.TitleAssistancies
                },
                new Menu
                {
                    Icon = "ic_members",
                    PageName = $"{nameof(MembersPage)}",
                    Title = Languages.TitleMembers
                },
                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = $"{nameof(LoginPage)}",
                    Title = Settings.IsLogin ? Languages.TitleLogout : Languages.TitleLogin
                }
            };

            if (User.UserType == UserType.Teacher)
            {
                menus.Add(new Menu
                {
                    Icon = "ic_person_plus",
                    PageName = $"{nameof(RegisterPage)}",
                    Title = Languages.TitleRegisterUser
                });

                menus.Add(new Menu
                {
                    Icon = "ic_calendar",
                    PageName = $"{nameof(CreateMeeting)}",
                    Title = Languages.TitleCreateMeating
                });
            }

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                    IsLoginRequired = m.IsLoginRequired
                }).ToList());
        }
    }
}
