target "docker-metadata-action" {}

target "estimate_api" {
  inherits = ["docker-metadata-action"]
  context = "EstimateAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64"
  ]
}

target "location_api" {
  inherits = ["docker-metadata-action"]
  context = "LocationAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64"
  ]
}

target "request_api" {
  inherits = ["docker-metadata-action"]
  context = "RequestAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64"
  ]
}

target "services_api" {
  inherits = ["docker-metadata-action"]
  context = "ServicesAPI"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64"
  ]
}

target "identity_service" {
  inherits = ["docker-metadata-action"]
  context = "IdentityService"
  dockerfile = "Dockerfile"
  platforms = [
    "linux/amd64"
  ]
}
