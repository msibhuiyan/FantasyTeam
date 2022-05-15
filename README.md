##Pre Installation

Run mongo locally on port 27017

Download and install .NET SDK version 3.1 from https://dotnet.microsoft.com/en-us/download/dotnet/3.1

To verify the installation and check version run dotnet --list-sdks

You will see the installed version 3.1.*** [C:\Program Files\dotnet\sdk]
##Project Setup
Clone the repository

cd .\Saiful-Islam\

dotnet clean

dotnet build

##Run tests
```
$ dotnet test
```
##Run project
```
$ dotnet run --project .\src\FantasyTeams.WebService\FantasyTeams.csproj
```
##Documentation
After ruuning the project go to https://localhost:5001/swagger/index.html to see documentation
