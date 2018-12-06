FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["MyWpfApp.Server/MyWpfApp.Server.csproj", "MyWpfApp.Server/"]
RUN dotnet restore "MyWpfApp.Server/MyWpfApp.Server.csproj"
COPY . .
WORKDIR "/src/MyWpfApp.Server"
RUN dotnet build "MyWpfApp.Server.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyWpfApp.Server.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyWpfApp.Server.dll"]