dotnet ef migrations add initial --project MovieRental

dotnet ef database update --project MovieRental --connection "Data Source=$env:LOCALAPPDATA\movierental.db"