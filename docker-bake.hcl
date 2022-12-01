target "docker-metadata-action" {}

target "api_gateway" {
  inherits = ["docker-metadata-action"]
  context = "APIGateway"
  dockerfile = "APIGateway/Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm/v7",
    "linux/arm64",
    "linux/386"
  ]
}

target "estimate_api" {
  inherits = ["docker-metadata-action"]
  context = "EstimateAPI"
  dockerfile = "EstimateAPI/Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm/v7",
    "linux/arm64",
    "linux/386"
  ]
}

target "location_api" {
  inherits = ["docker-metadata-action"]
  context = "LocationAPI"
  dockerfile = "LocationAPI/Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm/v7",
    "linux/arm64",
    "linux/386"
  ]
}

target "request_api" {
  inherits = ["docker-metadata-action"]
  context = "RequestAPI"
  dockerfile = "RequestAPI/Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm/v7",
    "linux/arm64",
    "linux/386"
  ]
}

target "services_api" {
  inherits = ["docker-metadata-action"]
  context = "ServicesAPI"
  dockerfile = "ServicesAPI/Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm/v7",
    "linux/arm64",
    "linux/386"
  ]
}

target "user_api" {
  inherits = ["docker-metadata-action"]
  context = "UserAPI"
  dockerfile = "UserAPI/Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm/v7",
    "linux/arm64",
    "linux/386"
  ]
}

target "auth_service" {
  inherits = ["docker-metadata-action"]
  context = "AuthService"
  dockerfile = "AuthService/Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm/v7",
    "linux/arm64",
    "linux/386"
  ]
}
