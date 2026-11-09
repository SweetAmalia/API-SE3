A .NET Minimal API with Swagger/OpenAPI documentation for seamless local development and testing. 

Setup repo on any device by following the next steps:

1. Make sure you have the following programs needed to run this locally:
   - .NET SDK (Requires .10 version)
   - Vscode (this entire project is using VSCode)
   - SQL Server (needed to make sure all data is saved.)

2. The following extensions needed:
   - C#
   - C# Devkit

To make everything work probably and allow for testing, I use Swagger. 
You can add swagger through NuGet Package: Swashbuckle.AspNetCore

Make sure you have all the latest versions of the programs/extensions (You can check that by opening the WebApplication1.csproj file and it should list all with the correct version.).

I use the Minimal API within the projects which allows for a less complicated and more structural option in comparison with Controllers. 
The main reason for choosing Minimal API comes down to the application I am working on, I am simply using this to make my website portfolio better
which is a lightweighted project so with minimal API I will have faster start times and better performances overall. 

The database is running locally using the SQL Server but it's initialised using EF Migrations which acts as communication between Backend and Database. 
Using EF Migrations turns all your C# automatically into SQL Database like for example:
The model "Project" would turn into the right tables, columns and relations automatically. 

To summarize what EF Migrations does is:
- EF Core creates the data needed in your database
- EF Core looks at your existing models, compares them with previous versions, writes a script to see which data needs to be changed or updated. (dotnet ef migrations add)
- EF Core sends all changes to your SQL Server so the C# is alligned accordingly. (dotnet ef database update)

Now I have given enough information about each element within the project, I also make sure that the project code structure stays clean. 
My Code structure is as following:
- `WebApplication1.sln`: Solution file grouping the API and test projects.
- `WebApplication1`: The central project where all the configurations, dependency injections and endpoints are defined.
- `Data`: Contains the Database context and configuration for the Entity Framwork core / SQL Server. 
- `Features`: Includes all the endpoints, services and DTOs (Data Transfer Objects) and Entities for the project.
- `WebApplication1.Tests`: Dedicated xUnit Integration testing project. 

Once all the above is sorted out it's pretty straightforward work.
- Clone the repository
- Restore the dependencies (dotnet restore)
- Run the application (dotnet run)

To view the endpoints and services , you can use swagger at your own localhost: example below
(http://localhost:5xxx/swagger) 


