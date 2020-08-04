dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\tests\TauCode.Cli.Tests.UnitTests\TauCode.Cli.Tests.UnitTests.csproj
dotnet test -c Release .\tests\TauCode.Cli.Tests.UnitTests\TauCode.Cli.Tests.UnitTests.csproj

nuget pack nuget\TauCode.Cli.nuspec
