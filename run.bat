dotnet clean

dotnet restore

dotnet build

cd .\ChatRoom.WebApplication\

dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialCreate

dotnet ef database update
