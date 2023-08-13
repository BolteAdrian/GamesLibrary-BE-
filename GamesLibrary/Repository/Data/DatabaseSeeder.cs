using GamesLibrary.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace GamesLibrary.Repository.Data
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(GamesLibraryDbContext dbContext)
        {

            // ADD ROLES
            dbContext.Roles.AddRange(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = "1"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Client",
                    NormalizedName = "CLIENT",
                    ConcurrencyStamp = "2"
                }
            );

            // ADD USERS
            dbContext.Users.AddRange(
                new IdentityUser
                {
                    Id = "06785032-45f9-43d6-bfd1-ebaffeeb2eab",
                    UserName = "bob.roberts@example.com",
                    NormalizedUserName = "BOB.ROBERTS@EXAMPLE.COM",
                    Email = "bob.roberts@example.com",
                    NormalizedEmail = "BOB.ROBERTS@EXAMPLE.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEDGB+at5UDDEXOdlzpTiMwQggOAkoyt0Eqrjm0Dzbg1CpUpG7H0H80kbgOP0HetXug==",
                    SecurityStamp = "62UHWDU3RSPSP3F5USVCRVCSFCFC33S5",
                    ConcurrencyStamp = "8e91ffcf-6922-41a5-8351-fedd25289f8d",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                },
                new IdentityUser
                {
                    Id = "630066b6-d090-44f6-8c18-07ac9ca6cb6f",
                    UserName = "testul_mare",
                    NormalizedUserName = "TESTUL_MARE",
                    Email = "testul_mare@example.com",
                    NormalizedEmail = "TESTUL_MARE@EXAMPLE.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEL8QCMcA2sXeJ6ZtkVjyzkSmeNd2hq+5NdI3zNB1544HlQD1CDsu01pe/RgWh0SccQ==",
                    SecurityStamp = "JCFNKOR5RESEXOL36QFEWQSE5HH462WJ",
                    ConcurrencyStamp = "dbdc2394-b95a-4e15-a411-50fa3c2836d8",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                },
                new IdentityUser
                {
                    Id = "63eab635-a02e-4ab4-9b96-a67e3abb8bdd",
                    UserName = "bolte-adrian@yahoo.com",
                    NormalizedUserName = "BOLTE-ADRIAN@YAHOO.COM",
                    Email = "bolte-adrian@yahoo.com",
                    NormalizedEmail = "BOLTE-ADRIAN@YAHOO.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAENtfYycvBb5AVMF3OkSGrAOZl5eRZ5pa+6Y4enPYWCWdEiV5cyHiNWtLQsOxW8z4GA==",
                    SecurityStamp = "UOOJ4P6JCE4MABQKY6DFFM4DU2DL47OG",
                    ConcurrencyStamp = "d575c013-b149-42db-a819-3cd2c48348a5",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                },

                new IdentityUser
                {
                    Id = "67907cec-7e1c-49b9-a92b-a728ee84a9a9",
                    UserName = "mary.johnson@example.com",
                    NormalizedUserName = "MARY.JOHNSON@EXAMPLE.COM",
                    Email = "mary.johnson@example.com",
                    NormalizedEmail = "MARY.JOHNSON@EXAMPLE.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEMkeLQ5/kaCXjipSrJYSEByw3l+4Q5ws040cAIkEyOzBHHEG8O8s70Qxtp7TD5+SbA==",
                    SecurityStamp = "DJOUKS3QLU3IO3SXHBIZERQWKG4JYCLW",
                    ConcurrencyStamp = "5ec99941-61ed-439f-9195-60dd288da47f",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                } 
            );

            //ADD USER ROLES
            dbContext.UserRoles.AddRange(
                new IdentityUserRole<string>
                {
                    UserId = "06785032-45f9-43d6-bfd1-ebaffeeb2eab",
                    RoleId = "2"
                },
                new IdentityUserRole<string>
                {
                    UserId = "630066b6-d090-44f6-8c18-07ac9ca6cb6f",
                    RoleId = "2"
                },
                new IdentityUserRole<string>
                {
                    UserId = "63eab635-a02e-4ab4-9b96-a67e3abb8bdd",
                    RoleId = "1"
                },
                new IdentityUserRole<string>
                {
                    UserId = "67907cec-7e1c-49b9-a92b-a728ee84a9a9",
                    RoleId = "2"
                }
            );

            // ADD GAMES
            dbContext.Games.AddRange(
                            new Game
                            {
                                Id = 1,
                                Title = "TEST NUME JOC2",
                                Description = "Survival action-adventure game",
                                ReleaseDate = new DateTime(2020, 6, 19),
                                Genre = "Action",
                                Developer = "EU",
                                Platform = "PlayStation 5",
                                PictureURL="url/picture",
                                Price = 59.99m
                            },
                            new Game
                            {
                                Id = 2,
                                Title = "Red Dead Redemption 2",
                                Description = "Action-adventure game set in the Wild West",
                                ReleaseDate = new DateTime(2018, 10, 26),
                                Genre = "Action",
                                Developer = "Rockstar Games",
                                Platform = "PlayStation 4, Xbox One, PC",
                                PictureURL = "url/picture",
                                Price = 49.99m
                            },
                            new Game
                            {
                                Id = 3,
                                Title = "The Legend of Zelda: Breath of the Wild",
                                Description = "Open-world action-adventure game",
                                ReleaseDate = new DateTime(2017, 3, 3),
                                Genre = "Action, Adventure",
                                Developer = "Nintendo",
                                Platform = "Nintendo Switch, Wii U",
                                PictureURL = "url/picture",
                                Price = 49.99m
                            },
                            new Game
                            {
                                Id = 4,
                                Title = "Cyberpunk 2077",
                                Description = "Role-playing game set in a dystopian future",
                                ReleaseDate = new DateTime(2020, 12, 10),
                                Genre = "RPG",
                                Developer = "CD Projekt Red",
                                Platform = "PlayStation 4, Xbox One, PC",
                                PictureURL = "url/picture",
                                Price = 39.99m
                            },
                            new Game
                            {
                                Id = 5,
                                Title = "Fortnite",
                                Description = "Battle Royale game",
                                ReleaseDate = new DateTime(2017, 7, 25),
                                Genre = "Action",
                                Developer = "Epic Games",
                                Platform = "PlayStation 4, Xbox One, PC, Nintendo Switch, Mobile",
                                PictureURL = "url/picture",
                                Price = 0.00m
                            },
                            new Game
                            {
                                Id = 6,
                                Title = "Call of Duty: Warzone",
                                Description = "Battle Royale game",
                                ReleaseDate = new DateTime(2020, 3, 10),
                                Genre = "Action",
                                Developer = "Infinity Ward, Raven Software",
                                Platform = "PlayStation 4, Xbox One, PC",
                                PictureURL = "url/picture",
                                Price = 0.00m
                            },
                            new Game
                            {
                                Id = 7,
                                Title = "FIFA 22",
                                Description = "Football simulation game",
                                ReleaseDate = new DateTime(2021, 10, 1),
                                Genre = "Sports",
                                Developer = "EA Vancouver",
                                Platform = "PlayStation 4, Xbox One, PC, Nintendo Switch",
                                PictureURL = "url/picture",
                                Price = 59.99m
                            },
                            new Game
                            {
                                Id = 8,
                                Title = "Assassin's Creed Valhalla",
                                Description = "Action role-playing game",
                                ReleaseDate = new DateTime(2020, 11, 10),
                                Genre = "Action, RPG",
                                Developer = "Ubisoft Montreal",
                                Platform = "PlayStation 4, Xbox One, PC, PlayStation 5, Xbox Series X/S",
                                PictureURL = "url/picture",
                                Price = 49.99m
                            },
                            new Game
                            {
                                Id = 9,
                                Title = "Minecraft",
                                Description = "Sandbox game",
                                ReleaseDate = new DateTime(2011, 11, 18),
                                Genre = "Adventure",
                                Developer = "Mojang Studios",
                                Platform = "PlayStation 4, Xbox One, PC, Nintendo Switch, Mobile",
                                PictureURL = "url/picture",
                                Price = 19.99m
                            },
                            new Game
                            {
                                Id = 10,
                                Title = "God of War (2018)",
                                Description = "Action-adventure game",
                                ReleaseDate = new DateTime(2018, 4, 20),
                                Genre = "Action",
                                Developer = "Santa Monica Studio",
                                Platform = "PlayStation 4",
                                PictureURL = "url/picture",
                                Price = 39.99m
                            },
                            new Game
                            {
                                Id = 11,
                                Title = "TEST NUME JOC",
                                Description = "Survival action-adventure game",
                                ReleaseDate = new DateTime(2020, 6, 19),
                                Genre = "Action",
                                Developer = "EU",
                                Platform = "PlayStation 5",
                                PictureURL = "url/picture",
                                Price = 59.99m
                            }
                        );

            // ADD REVIEWS
            dbContext.Reviews.AddRange(
                            new Review
                            {
                                Id = 1,
                                UserId = "06785032-45f9-43d6-bfd1-ebaffeeb2eab",
                                GameId = 5,
                                Rating = 4,
                                Comment = "Great game!"
                            },
                            new Review
                            {
                                Id = 2,
                                UserId = "630066b6-d090-44f6-8c18-07ac9ca6cb6f",
                                GameId = 5,
                                Rating = 5,
                                Comment = "Absolutely loved it!"
                            },
                            new Review
                            {
                                Id = 3,
                                UserId = "630066b6-d090-44f6-8c18-07ac9ca6cb6f",
                                GameId = 1,
                                Rating = 5,
                                Comment = "Absolutely loved it!"
                            },
                            new Review
                            {
                                Id = 4,
                                UserId = "630066b6-d090-44f6-8c18-07ac9ca6cb6f",
                                GameId = 2,
                                Rating = 1,
                                Comment = "WEEEAK !!"
                            }
                        );

            // ADD PURCHASES
            dbContext.Purchases.AddRange(
                        new Purchase
                        {
                            Id = 1,
                            UserId = "06785032-45f9-43d6-bfd1-ebaffeeb2eab",
                            GameId = 5,
                            PurchaseDate = DateTime.Parse("2023-07-29 20:22:26.7640000")
                        },
                        new Purchase
                        {
                            Id = 2,
                            UserId = "630066b6-d090-44f6-8c18-07ac9ca6cb6f",
                            GameId = 5, 
                            PurchaseDate = DateTime.Parse("2023-07-29 20:22:26.7640000")
                        },
                        new Purchase
                        {
                            Id = 3,
                            UserId = "63eab635-a02e-4ab4-9b96-a67e3abb8bdd",
                            GameId = 2,
                            PurchaseDate = DateTime.Parse("2023-07-29 20:22:26.7640000")
                        },
                        new Purchase
                        {
                            Id =4,
                            UserId = "67907cec-7e1c-49b9-a92b-a728ee84a9a9",
                            GameId = 8, 
                            PurchaseDate = DateTime.Parse("2023-07-29 20:22:26.7640000")
                        }

                    );

            dbContext.SaveChanges();
        }
    }
}
