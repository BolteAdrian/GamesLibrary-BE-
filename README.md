# GamesLibrary-BE-
This project was created in .Net 6

Target project app:
Create a functional web API for an online games library app (for example https://www.gog.com/).
The web api should have all the required endpoints to serve the data to any frontend component/or any other client.
The API endpoints need to be secured, the security mechanims can be choosen.
The open api spec need to be created togheder with a Postman colection containing all the endpoints with sample requests.
The web api will use in memory database with seeded data.

DataBase GamesLibrary:

Games Table:

Id (int, Primary Key, Auto-generated)
Description (nvarchar(max), Required)
Developer (nvarchar(max), Required)
Genre (nvarchar(max), Required)
Platform (nvarchar(max), Required)
Price (decimal(18,2))
ReleaseDate (datetime2)
Title (nvarchar(max), Required)
Purchases Table:

Id (int, Primary Key, Auto-generated)
GameId (int, Foreign Key, References Games.Id)
PurchaseDate (datetime2)
UserId (nvarchar(450), Foreign Key, References IdentityUser.Id)
AspNetRoles Table (IdentityRole):

Id (nvarchar(450), Primary Key)
ConcurrencyStamp (nvarchar(max))
Name (nvarchar(256), Max Length: 256)
NormalizedName (nvarchar(256), Max Length: 256)
AspNetRoleClaims Table (IdentityRoleClaim<string>):

Id (int, Primary Key, Auto-generated)
ClaimType (nvarchar(max))
ClaimValue (nvarchar(max))
RoleId (nvarchar(450), Foreign Key, References IdentityRole.Id)
Users Table (IdentityUser):

Id (nvarchar(450), Primary Key)
AccessFailedCount (int)
ConcurrencyStamp (nvarchar(max))
Email (nvarchar(256), Max Length: 256)
EmailConfirmed (bit)
LockoutEnabled (bit)
LockoutEnd (datetimeoffset)
NormalizedEmail (nvarchar(256), Max Length: 256)
NormalizedUserName (nvarchar(256), Max Length: 256)
PasswordHash (nvarchar(max))
PhoneNumber (nvarchar(max))
PhoneNumberConfirmed (bit)
SecurityStamp (nvarchar(max))
TwoFactorEnabled (bit)
UserName (nvarchar(256), Max Length: 256)
AspNetUserClaims Table (IdentityUserClaim<string>):

Id (int, Primary Key, Auto-generated)
ClaimType (nvarchar(max))
ClaimValue (nvarchar(max))
UserId (nvarchar(450), Foreign Key, References IdentityUser.Id)
AspNetUserLogins Table (IdentityUserLogin<string>):

LoginProvider (nvarchar(450), Primary Key)
ProviderKey (nvarchar(450), Primary Key)
ProviderDisplayName (nvarchar(max))
UserId (nvarchar(450), Foreign Key, References IdentityUser.Id)
AspNetUserRoles Table (IdentityUserRole<string>):

UserId (nvarchar(450), Primary Key, Foreign Key, References IdentityUser.Id)
RoleId (nvarchar(450), Primary Key, Foreign Key, References IdentityRole.Id)
AspNetUserTokens Table (IdentityUserToken<string>):

UserId (nvarchar(450), Primary Key, Foreign Key, References IdentityUser.Id)
LoginProvider (nvarchar(450), Primary Key)
Name (nvarchar(450), Primary Key)
Value (nvarchar(max))
