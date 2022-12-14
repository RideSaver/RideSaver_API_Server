FROM --platform=amd64 mcr.microsoft.com/devcontainers/dotnet:0-6.0 AS builder

ARG github_username
ARG github_token
ARG TARGETPLATFORM




WORKDIR /server

RUN dotnet nuget add source --username $github_username --password $github_token --store-password-in-clear-text --name github "https://nuget.pkg.github.com/RideSaver/index.json"
RUN dotnet tool install --global dotnet-ef

COPY ServicesAPI.csproj .
RUN dotnet restore

# Copy all files
COPY . .
RUN dotnet publish -p:PublishProfile=PublishTrimmed --sc --os="linux" -o publish
RUN /root/.dotnet/tools/dotnet-ef migrations bundle --self-contained -r linux-x64 --output publish/efbundle

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
# Add labels to add information to the image
LABEL org.opencontainers.image.source=https://github.com/RideSaver/RideSaver_API_Server
LABEL org.opencontainers.image.description="Services API Service for RideSaver"
LABEL org.opencontainers.image.licenses=MIT

EXPOSE 80
EXPOSE 443
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

# Copy
WORKDIR /app
COPY --from=builder /server/publish ./

CMD ["./ServicesAPI", "--urls", "http://0.0.0.0:80"]
