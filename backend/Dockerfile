# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy the .csproj file and restore any dependencies (via nuget)
COPY ["src/funscraper.csproj", "/src/"]
RUN dotnet restore "funscraper.csproj"

# Copy the entire source code and build the application
COPY src/. .
WORKDIR "/src"
RUN dotnet publish "funscraper.csproj" -c Release -o /app/publish

# Use the official .NET Runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory for the app inside the container
WORKDIR /app

# Copy the compiled application from the build container
COPY --from=build /app/publish .

# Expose the port the app will run on (default for ASP.NET Core is 80)
EXPOSE 80

# Define the entrypoint for the container (run the app)
ENTRYPOINT ["dotnet", "funscraper.dll"]
