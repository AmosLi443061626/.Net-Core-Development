FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 80
COPY . /app
ENTRYPOINT ["dotnet", "startup.dll"]
#时区与中国的差别启动，共享时区 : -v /etc/localtime:/etc/localtime:ro
