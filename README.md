# RideSaver
RideSaver API Server contains all the microservices & the API gateway to handle the incoming traffic as well as reroute to the apropriate services based on the incoming requests.

## Server Structure

### API Gateway

The API Gateway utilizes the 3rd party package Ocelot which acts as a middleware between the services & the client. Ocelot.json is configured to reroute the incoming traffic to the appropriate service.

### Estimate Service

The Estimate Service manages the ride-estimates through invoking the web clinet helpers & retrieving live data from the ride-share APIs.
#### Controllers: 
EstimateController

### Request Service

The Request service manages the ride-requests through invoking the web client helpers & creating request informations to the ride-share APIs.
#### Controllers: 
RequestController

### Services Service

The Services service manages the list of available services in the client location through the locationController & service.
#### Controllers: 
ServicesController

### Location Service

The Location service retrieves the consumer location to provide accurate infomration for the ride estimates & requests.
#### Controllers: 
LocationController

### User Service

The User services manages the client account data & handles the registeration & login process with EF Core & SQL Server
The UserAPI contains a UserRepository which utilizes DbContext and EFCore for database queries with a code-first approach.
#### Controllers: 
UserController

## Required Packages

RideSaver utilizes the sidecar pattern for the web clients helpers & the openAPI specifications packages which are NuGet packages that referenced when needed.
- RideSaver.Server.dll (https://github.com/RideSaver/RideSaver_API_Specifications/packages/1674227)
- RideSaver.WebClients.dll (https://github.com/RideSaver/RideSaver_API_Clients) - IN PROGRESS
