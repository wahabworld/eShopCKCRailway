
# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files and publish
COPY . ./

# Copy Pics to a known location in publish folder
RUN mkdir -p /app/publish/Pics && cp -r Pics/* /app/publish/Pics/

RUN dotnet publish -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Replace with your DLL name
ENTRYPOINT ["dotnet", "eShopCKC.dll"]
