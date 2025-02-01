from mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . .
RUN dotnet restore
RUN dotnet publish Diary.WebAPI -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN apt-get update && apt-get install -y fonts-dejavu-core fontconfig
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Diary.WebAPI.dll"]