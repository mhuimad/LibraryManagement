FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY database/src/LibraryManagement.Database.Build/ .
RUN dotnet build LibraryManagement.Database.Build.sqlproj -c Release -o /dacpac

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS deploy
RUN dotnet tool install --global microsoft.sqlpackage
ENV PATH="$PATH:/root/.dotnet/tools"
WORKDIR /dacpac
COPY --from=build /dacpac .
ENTRYPOINT ["sh", "-c", "sqlpackage /Action:Publish /SourceFile:LibraryManagement.Database.Build.dacpac /TargetConnectionString:\"$TARGET_CONNECTION_STRING\""]
