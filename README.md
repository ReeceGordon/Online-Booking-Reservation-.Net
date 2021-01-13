# Online Barber Shop Reservation System with Member Management

A system designed to seemlessly integrate into currently existing websites/businesses to provide an online reservation service customisible in many ways for a barber shop or similar service provider.

Uses SQL Server for data storage with Entity Framework Core as the relational mapper, .Net Identity Framework for user login system with Admin, Staff and Customer 'rank' functionality. 'Staff' users can add their availability for days/timeslots with matching services 
they provide in turn allowing 'Customer' users to select such staff when making a booking reservation. 'Admin' users have access to root settings such as site logo, universal settings and modifying Email HTML template which is used to automatically send a confirmation email to a 'Customer' user reservation after completion. Uses MVC pattern with shared models, including registration/login data validation through ComponentModel.DataAnnotations. 

### Prerequisites

What things you need to install the software and how to install them

```
Visual Studio 2019 Preview (This was developed prior to Blazor backend being added to VS Live, preview was necessary but may not be anymore)
Blazor 3.1
```

## Deployment

System was temporarily deployed to Azure services for testing purposes, deployment is ultimately down to respective environments.

## Built With

* [Bootstrap](https://getbootstrap.com/) - The CSS framework used
* [.Net Core](https://dotnet.microsoft.com/download/dotnet-core) - Built using .Net Core Library and features
* [Blazor 3.1](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) - Used to mix Backend functionality with front end use cases with a uniform language (C#)
* [VS2019 Preview](https://visualstudio.microsoft.com/vs/preview/) - Used to add backend to Blazor 3.1 WebAssembly

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
