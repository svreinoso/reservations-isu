# reservations-isu

- Project created with ASP.NET Core 5.0 with Entity Framework Core Code-First pattern and Sql Server.

- Angular 11 in the front end, some libraries like bootstrap, angular material, datatables.net and others.

- An User can add, edit or delete contacts and or reservations, add to favorite and add new rating.

- Responsive design
- Use of stored procedures 

### Setup

- Clone the repository 

- Setup the Sql Server connection string in Reservation.API/appSettings.json 
```sh
    git clone https://github.com/svreinoso/reservations-isu.git
    cd reservations-isu/Reservation.API
    dotnet ef database update
```

- Run with Visual Studio
    - Just open the solution and run the projects Reservation.API and Reservation.UI
    
- Run with dotnet backend
```hs
    cd reservations-isu/Reservation.API
    dotnet restore
    dotnet build
    dotnet run
```
- Run with dotnet frontend
```hs
    cd reservations-isu/Reservation.UI/ClientApp
    npm install
    ng serve
```

- Run in Spanish 
```sh
    ng serve --configuration es
```
Only the header text is translate to demostrate the use and implementation of i18n

### Run unit test
```
    cd Reservation.Test
    dotnet test
```
Only some test for a controller and a service to demostrate the implementation of unit test
