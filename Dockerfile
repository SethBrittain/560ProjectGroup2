# Stage 1 - Build Angular project
FROM node:18 as build-stage1

WORKDIR /client

COPY Frontend/ ./

RUN npm install

RUN npm run build

# Stage 2 - Build ASP.NET project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-stage-2

WORKDIR /server

COPY Backend/src/ ./

RUN dotnet restore

COPY --from=0 /client/dist/database-project/ /server/wwwroot/

RUN dotnet publish . -c Release -o out

# Stage 3 - Copy React build to wwwroot of asp project
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS build-stage3

COPY --from=1 /server/out .

# Https traffic
ENV ASPNETCORE_URLS="http://+"

EXPOSE 5000

# Run ASP.NET app
ENTRYPOINT ["dotnet", "Pidgin.dll"]
