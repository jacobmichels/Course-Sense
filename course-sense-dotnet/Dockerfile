#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["course-sense-dotnet/course-sense-dotnet.csproj", "course-sense-dotnet/"]
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash
RUN apt-get install nodejs -y
RUN node -v
RUN npm -v
RUN npm install -g webpack
RUN dotnet restore "course-sense-dotnet/course-sense-dotnet.csproj"
COPY . .
WORKDIR "/src/course-sense-dotnet"
RUN npm install
RUN dotnet build "course-sense-dotnet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "course-sense-dotnet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "course-sense-dotnet.dll"]