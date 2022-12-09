target "docker-metadata-action" {}

target "api_gateway" {
  inherits = ["docker-metadata-action"]
  context = "APIGateway"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm/v7",
    "linux/arm64"
  ]
}

target "estimate_api" {
  inherits = ["docker-metadata-action"]
  context = "EstimateAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm64"
  ]
}

target "location_api" {
  inherits = ["docker-metadata-action"]
  context = "LocationAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm64"
  ]
}

target "request_api" {
  inherits = ["docker-metadata-action"]
  context = "RequestAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm64"
  ]
}

target "services_api" {
  inherits = ["docker-metadata-action"]
  context = "ServicesAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm64"
  ]
}

target "user_api" {
  inherits = ["docker-metadata-action"]
  context = "UserAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm64"
  ]
}

target "auth_service" {
  inherits = ["docker-metadata-action"]
  context = "AuthService"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64",
    "linux/arm/v6",
    "linux/arm64"
  ]
}
