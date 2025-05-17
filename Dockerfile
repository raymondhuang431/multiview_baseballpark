# 構建階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 複製專案文件
COPY ["Mutiview_BaseballPark.csproj", "./"]
RUN dotnet restore

# 複製其餘源代碼
COPY . .
RUN dotnet build -c Release -o /app/build

# 發布階段
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# 運行階段
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 設置環境變量
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# 暴露端口
EXPOSE 80

# 啟動應用
ENTRYPOINT ["dotnet", "Mutiview_BaseballPark.dll"] 