//using eCommerceApp.Models;
//using Microsoft.AspNetCore.Identity;
//using System.Diagnostics;
//using System.Net;

//namespace eCommerceApp.Data
//{
//    public class Seed
//    {
//        public static void SeedData(IApplicationBuilder applicationBuilder)
//        {
//            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
//            {
//                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

//                context.Database.EnsureCreated();

//                //Products

//                if (!context.Products.Any())
//                {
//                    context.Products.AddRange(new List<Product>()
//                    {
//                        new Product()
//                        {
//                            Name = "Strawberry",
//                            ImageUrl = "https://c02.purpledshub.com/uploads/sites/41/2023/09/GettyImages_154514873.jpg?w=750&webp=1",
//                            Description = "This is the description of the first product.",
//                            Stock = 20,
//                            Price = 5

//                         },
//                        new Product()
//                        {
//                            Name = "Plum",
//                            ImageUrl = "https://www.worldatlas.com/r/w960-q80/upload/d0/99/ef/shutterstock-645293734.jpg",
//                            Description = "This is the description of the second product.",
//                            Stock = 17,
//                            Price = 3

//                         },
//                        new Product()
//                        {
//                            Name = "Orange",
//                            ImageUrl = "https://www.fervalle.com/wp-content/uploads/2022/07/transparent-orange-apple5eacfeae85ac29.7815306015883956945475-300x300.png",
//                            Description = "This is the description of the third product.",
//                            Stock = 25,
//                            Price = 2

//                         },
//                        new Product()
//                        {
//                            Name = "Apple",
//                            ImageUrl = "https://www.collinsdictionary.com/images/thumb/apple_158989157_250.jpg?version=6.0.26",
//                            Description = "This is the description of the fourth product.",
//                            Stock = 30,
//                            Price = 1

//                         }
//                    });
//                    context.SaveChanges();

//                    //ShoppingCartItem

//                    //if (!context.ShoppingCartItems.Any())
//                    //{
//                    //    context.ShoppingCarts.AddRange(new List<ShoppingCart>()
//                    //    {
//                    //    new ShoppingCartItem()
//                    //    {
                            
//                    //    }
                        
//                    //});
//                    //    context.SaveChanges();
//                    //}
//                }
//            }

//        }

//        //public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
//        //{
//        //    using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
//        //    {
//        //        //Roles
//        //        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//        //        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
//        //            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
//        //        if (!await roleManager.RoleExistsAsync(UserRoles.User))
//        //            await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

//        //        //Users
//        //        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
//        //        string adminUserEmail = "teddysmithdeveloper@gmail.com";
//        //        string adminUserPassword;

//        //        var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
//        //        if (adminUser == null)
//        //        {
//        //            var newAdminUser = new User()
//        //            {
                        
//        //                Email = adminUserEmail,
//        //                PasswordHash = adminUserPassword,
                        
//        //            };
//        //            await userManager.CreateAsync(newAdminUser, "Coding@1234?");
//        //            await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
//        //        }

//        //        string appUserEmail = "user@etickets.com";

//        //        var appUser = await userManager.FindByEmailAsync(appUserEmail);
//        //        if (appUser == null)
//        //        {
//        //            var newAppUser = new User()
//        //            {
                        
//        //                Email = appUserEmail,
//        //                EmailConfirmed = true,
                        
//        //            };
//        //            await userManager.CreateAsync(newAppUser, "Coding@1234?");
//        //            await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
//        //        }
//        //    }
//        //}
//    }
//}
    

