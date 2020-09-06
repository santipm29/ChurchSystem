using ChurchSystem.Common.Entities;
using ChurchSystem.Common.Enums;
using ChurchSystem.Web.Data.Entities;
using ChurchSystem.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchSystem.Web.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckFieldsAsync();
            await CheckRolesAsync();
            await CheckProfessionsAsync();
            await CheckUserAsync("1010", "Santiago", "Patiño", "santipmartinez@outlook.com", "320 20 20", "Calle Villa", UserType.Admin);
            await CheckUserAsync("1020", "Isabel", "Martinez", "isabelmarianap@gmail.com", "315 40 20", "Calle Imaginaria", UserType.User);
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
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    Church = _context.Churches.FirstOrDefault(),
                    UserType = userType
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
