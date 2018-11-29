FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 56186
EXPOSE 44372

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY leo.cap.study/leo.cap.study.csproj leo.cap.study/
RUN dotnet restore leo.cap.study/leo.cap.study.csproj
COPY . .
WORKDIR /src/leo.cap.study
RUN dotnet build leo.cap.study.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish leo.cap.study.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "leo.cap.study.dll"]
