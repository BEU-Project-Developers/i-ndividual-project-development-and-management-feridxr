using Scheduler.Model.DBEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Services
{
    public class SampleData
    {
        public async Task PopulateSampleData()
        {
            DBContext context = new DBContext();
            var random = new Random();

            // Add Countries
            List<string> countries = new List<string> {
                "US",
                "Canada"
            };
            foreach (string country in countries)
            {
                await context.AddAsync(
                    new Country
                    {
                        Country1 = country,
                        CreateDate = DateTime.Now,
                        CreatedBy = "data_generator",
                        LastUpdate = DateTime.Now,
                        LastUpdateBy = "data_generator"
                    }
                );
            }
            await context.SaveChangesAsync();

            // Add Cities
            List<City> cities = new List<City> {
                new City { City1 = "New York", Country = context.Country.Where(x => x.Country1 == "US").First() },
                new City { City1 = "Los Angeles", Country = context.Country.Where(x => x.Country1 == "US").First() },
                new City { City1 = "Toronto", Country = context.Country.Where(x => x.Country1 == "Canada").First() },
                new City { City1 = "Vancouver", Country = context.Country.Where(x => x.Country1 == "Canada").First() },
            };
            foreach (City city in cities)
            {
                city.CreateDate = DateTime.Now;
                city.CreatedBy = "data_generator";
                city.LastUpdate = DateTime.Now;
                city.LastUpdateBy = "data_generator";
                await context.AddAsync(city);
            }
            await context.SaveChangesAsync();

            // Add Addresses
            List<Address> addresses = new List<Address> {
                new Address {
                    Address1 = "123 Main",
                    Address2 = "",
                    City = context.City.Where(x => x.City1 == "New York").First(),
                    Phone = "123-555-1212",
                    PostalCode = "08330"},
                new Address {
                    Address1 = "456 Elm",
                    Address2 = "",
                    City = context.City.Where(x => x.City1 == "Toronto").First(),
                    Phone = "456-654-9999",
                    PostalCode = "92122" },
                new Address {
                    Address1 = "99 Oak",
                    Address2 = "",
                    City = context.City.Where(x => x.City1 == "Los Angeles").First(),
                    Phone = "987-654-1234",
                    PostalCode = "95414" },
            };
            foreach (Address address in addresses)
            {
                StringBuilder phone = new StringBuilder();
                _ = phone.Append(random.Next(100, 999).ToString());
                _ = phone.Append("-");
                _ = phone.Append(random.Next(100, 999).ToString());
                _ = phone.Append("-");
                _ = phone.Append(random.Next(1000, 9999).ToString());

                address.CreateDate = DateTime.Now;
                address.CreatedBy = "data_generator";
                address.LastUpdate = DateTime.Now;
                address.LastUpdateBy = "data_generator";
                address.Phone = phone.ToString();
                address.PostalCode = random.Next(10000, 99999).ToString();
                await context.AddAsync(address);
            }
            await context.SaveChangesAsync();

            // Add Customers
            List<Customer> customers = new List<Customer> {
                new Customer { CustomerName = "Barry Allen", Active = 1 },
                new Customer { CustomerName = "Clark Kent", Active = 1 },
                new Customer { CustomerName = "Peter Parker", Active = 1 },
                new Customer { CustomerName = "Selena Kyle", Active = 1 },
                new Customer { CustomerName = "Bruce Wayne", Active = 1 }
            };
            foreach (Customer customer in customers)
            {
                customer.AddressId = context.Address.ToList()[random.Next(context.Address.Count())].AddressId;
                customer.CreateDate = DateTime.Now;
                customer.CreatedBy = "data_generator";
                customer.LastUpdate = DateTime.Now;
                customer.LastUpdateBy = "data_generator";
                await context.AddAsync(customer);
            }
            await context.SaveChangesAsync();

            List<User> users = new List<User> {
                new User { UserName = "marco", Password = "marco", Active = 1 },
                new User { UserName = "polo", Password = "polo", Active = 1 },
                new User { UserName = "kublai", Password = "kublai", Active = 1 },
                new User { UserName = "khan", Password = "khan" , Active = 1 }
            };
            foreach (User user in users)
            {
                user.CreateDate = DateTime.Now;
                user.CreatedBy = "data_generator";
                user.LastUpdate = DateTime.Now;
                user.LastUpdateBy = "data_generator";
                await context.AddAsync(user);
            }
            await context.SaveChangesAsync();

            // Add apointments
            List<string> apptType = new List<string> { "Scrum", "Presentation", "Kick-Off", "Client Meeting" };
            List<int> startMin = new List<int> { 0, 15, 30, 45 };
            List<int> endMin = new List<int> { 15, 30, 45, 60, 90 };
            for (int i = 0; i < 50; i++)
            {
                DateTime start = DateTime.Today
                    .AddDays(random.Next(-30, 30))
                    .AddHours(random.Next(-6, 6))
                    .AddMinutes(startMin[random.Next(startMin.Count())]);
                DateTime end = start.AddMinutes(endMin[random.Next(startMin.Count())]);

                Appointment appointment = new Appointment { };

                appointment.CustomerId = context.Customer.ToList()[random.Next(context.Customer.Count())].CustomerId;
                appointment.UserId = context.User.ToList()[random.Next(context.Customer.Count())].UserId;
                appointment.Title = "Appt Title";
                appointment.Description = "Appt Description";
                appointment.Location = "Appt Location";
                appointment.Type = apptType[random.Next(apptType.Count())];
                appointment.Contact = "";
                appointment.Url = "";
                appointment.Start = start;
                appointment.End = end;
                appointment.CreateDate = DateTime.Now;
                appointment.CreatedBy = "data_generator";
                appointment.LastUpdate = DateTime.Now;
                appointment.LastUpdateBy = "data_generator";

                await context.AddAsync(appointment);
            }
            await context.SaveChangesAsync();
        }
    }
}