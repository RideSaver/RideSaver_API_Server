# RideSaver
RideSaver API Server contains all the microservices & the API gateway to handle the incoming traffic as well as reroute to the apropriate services based on the incoming requests.

## Server Structure

### IdentityService

The Identity Service handles all form of user-actions such as registeration, authentication, and authorization throughout RideSaver. This service also acts as the Authentication Service by providing user-tokens, refresh tokens, and validation for accessing other services. 

#### Controllers: 

> **Authentication Controller**\
The Authentication Controller hosts the REST endpoints that handle the user-authentication via JWT tokens, token validation, refresh-token-generation, and revoking tokens. These endpoints communicate with sub-services to retrieve the proper data to handle all user-validation actions.

> **Users Controller**\ 
The Users Controller hosts the REST endpoints that handle the user account creation, deletion, updates, and service-authorization. These endpoints communicate with sub-services & the database repository to retrieve/update/create the needed information for all user-account actions.

#### Sub-Services: 
> **Token Service**\
The Token Service utilizes JWT tokens & ASP.NET Core Identity to generate, validate, and retrieve user-tokens which are used for endpoints-authorization, and inter-service gRPC calls authentication. 

> **Auth Service**\
The Auth Service utilizes multiple hashing & salting algorithms to handle the user-passwords by not storing any passwords in clear-text, rather in a hash which is then validated through comparison to a unique-salt for each user. This sub-service also handles the user sub-tokens (RefreshTokens) which are used within Services as user-access tokens.

> **Access Token Service**\
The Access Token Service uses the JWT tokens within the gRPC requests headers metadata for inter-service communications to retrieve the user refresh token per request & provide a list of available services & products based on the provided parameters, the Access Token Service has direct access to the database through the repository which allows for asynchronous data retrieval. 

### Estimate Service

The Estimate Service handles retrieving live-data for price estimates for a range of products & services offered by the configured-clients to provide the customer with a list of price-estimates and available products in the requested area to the specified destination.

#### Controllers: 

> **Estimate Controller**\
The Estimate Controller hosts the REST endpoints that communicate with the user-interface to collect the starting & ending datapoints which would be used for retrieving the correct information.

#### Sub-Services: 
> **Client Discovery**\
The Client Discovery service enables the Estimate Service to host a range of 3rd party clients while allowing for the integration of new instances during runtime without any down-time. The Client Discovery searches the Kubernetes Cluster Configuration for all deployed web client-pods & retrieves their information for inter-service gRPC calls. 

### Request Service

The Request Service handles creating & sending the ride-requests to their respective clients through generating the proper request parameters based on user-input and interacting with the 3rd party APIs to make the requests. This is all done within RideSaver thus the consumer wouldnt have to switch applications.

#### Controllers: 
> **Request Controller**\
The Request Controller hosts the REST endpoints that communicate with the user-interface to collect the ride-request information which then translates them to to the proper web-clients through gRPC

#### Sub-Services: 
> **Client Discovery**\
The Client Discovery service enables the Request Service to host a range of 3rd party clients while allowing for the integration of new instances during runtime without any down-time. The Client Discovery searches the Kubernetes Cluster Configuration for all deployed web client-pods & retrieves their information for inter-service gRPC calls. 

### Products Service

The Products service (ServicesAPI) handles hosting the variety of service IDs & products offered by 3rd party clients. It acts as the main repository & registry for all available services as well as user-authorized services through translating the products into applicable service IDs stored in the DB which are then retrieved upon ride-requests per-user-basis. 

#### Controllers: 

> **Services Controller**\
The Services Controller hosts the REST endpoints that retrieve the list of available services, providers, and authorized services when needed through an internal service-registry & database access. 

#### Sub-Services: 
> **Internal Services**\
The Internal Services is a gRPC server that listens for service-requests to retrieve service hashes, avaialble services, and service registeration into thge database. Service IDs are trnaslted into a uniform-UUID through an internal-hashing algorithm that allows all 3rd party services to be accepted by our internal API.

### Location Service

The Location service retrieves the consumer location to provide accurate infomration for the ride estimates & requests.
#### Controllers: 

> **Location Controller**\
In Progress - TBA.

## Required Packages

RideSaver utilizes the sidecar pattern for the web clients helpers & the openAPI specifications packages which are NuGet packages that referenced when needed.
- RideSaver.Server.dll (https://github.com/RideSaver/RideSaver_API_Specifications/packages/1674227)
- RideSaver.WebClients.dll (https://github.com/RideSaver/RideSaver_API_Clients) - IN PROGRESS
