using ChurchSystem.Common.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchSystem.Web.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;
        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckFieldsAsync();
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
