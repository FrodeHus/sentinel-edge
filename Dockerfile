FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
ADD SentinelEdge.Api/*.csproj .
RUN dotnet restore

ADD SentinelEdge.Api/ .
RUN dotnet publish -c Release --runtime alpine-x64 --self-contained true /p:PublishTrimmed=true /p:PublishSingleFile=true -o ./publish

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine
RUN adduser --disabled-password --home /app --gecos '' app && chown -R app /app
USER app
WORKDIR /app
COPY --from=build /src/publish .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
HEALTHCHECK CMD curl --fail http://localhost/healthz || exit

ENTRYPOINT ["./SentinelEdge.Api"]