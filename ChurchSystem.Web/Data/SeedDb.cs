
using ChurchSystem.Common.Enums;
using ChurchSystem.Common.Models;
using ChurchSystem.Common.Services;
using ChurchSystem.Web.Data.Entities;
using ChurchSystem.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchSystem.Web.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IApiService _apiService;
        private readonly Random _random;
        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper, IApiService apiService)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _apiService = apiService;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckFieldsAsync();
            await CheckRolesAsync();
            await CheckProfessionsAsync();
            await CheckUsersAsync();
        }

        private async Task CheckProfessionsAsync()
        {
            if (!_context.Professions.Any())
            {
                _context.Professions.Add(new Profession
                {
                    Name = "Ingeniero"
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Contador"
                });

                _context.Professions.Add(new Profession
                {
                    Name = "Arquitecto"
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Actor"
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Médico"
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
            await _userHelper.CheckRoleAsync(UserType.Teacher.ToString());
        }

        private async Task CheckUsersAsync()
        {
            if (!_context.Users.Any())
            {
                await CheckAdminsAsync();
                await CheckTeachersAsync();
                await CheckMembersAsync();
            }
        }

        private async Task CheckMembersAsync()
        {
            for (int i = 1; i <= 50; i++)
            {
                await CheckUserAsync($"200{i}", $"member{i}@yopmail.com", UserType.User);
            }
        }
        private async Task CheckTeachersAsync()
        {
            for (int i = 1; i <= 15; i++)
            {
                await CheckUserAsync($"200{i}", $"teacher{i}@yopmail.com", UserType.Teacher);
            }
        }

        private async Task CheckAdminsAsync()
        {
            await CheckUserAsync("1001", "admin1@yopmail.com", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(
            string document,
            string email,
            UserType userType)
        {
            RandomUsers randomUsers;

            do
            {
                randomUsers = await _apiService.GetRandomUser("https://randomuser.me", "api");
            } while (randomUsers == null);

            Guid imageId = Guid.Empty;
            RandomUser randomUser = randomUsers.Results.FirstOrDefault();
            string imageUrl = randomUser.Picture.Large.ToString().Substring(22);
            Stream stream = await _apiService.GetPictureAsync("https://randomuser.me", imageUrl);
            if (stream != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(stream, "users");
            }

            int churchId = _random.Next(1, _context.Churches.Count());
            int profesionId = _random.Next(1, _context.Professions.Count());
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = randomUser.Name.First,
                    LastName = randomUser.Name.Last,
                    Email = email,
                    UserName = email,
                    PhoneNumber = randomUser.Cell,
                    Address = $"{randomUser.Location.Street.Number}, {randomUser.Location.Street.Name}",
                    Document = document,
                    UserType = userType,
                    Profession = await _context.Professions.FindAsync(profesionId),
                    Church = await _context.Churches.FindAsync(churchId),
                    ImageId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }


        private async Task CheckFieldsAsync()
        {
            if (!_context.Fields.Any())
            {
                _context.Fields.Add(new Field
                {
                    Name = "Comuna ocho",
                    Districts = new List<District> { 
                        new District
                        {
                            Name = "San Miguel",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Basilica Menor" },
                                new Church { Name = "La Candelaria" },
                                new Church { Name = "Veracruz" },
                                new Church { Name = "San Ignacio" }
                            }
                        },
                        new District
                        {
                            Name = "La Mansión",
                            Churches = new List<Church>
                            {
                                new Church { Name = "San Jose" },
                                new Church { Name = "Jesus Nazareno" },
                                new Church { Name = "Santa Gertrudis" }
                            }
                        }
                    }
                });
                _context.Fields.Add(new Field
                {
                    Name = "Comuna siete",
                    Districts = new List<District> {
                        new District
                        {
                            Name = "Robledo",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Basilica Metropolitana" },
                                new Church { Name = "San Ignacio de Loyola" },
                                new Church { Name = "Señor de las Misericordias" }
                            }
                        },
                        new District
                        {
                            Name = "Bello Horizonte",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Santa Gema" },
                                new Church { Name = "Nuestra señora del Rosario" },
                                new Church { Name = "San Benito" }
                            }
                        }
                    }
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
