﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RequestAPI.Repository;
using RideSaver.Server.Controllers;
using System.ComponentModel.DataAnnotations;

namespace RequestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : RequestApiController
    {
        private readonly IRequestRepository _requestRepository;
        public RequestController(IRequestRepository requestRepository) => _requestRepository = requestRepository;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> CancelRide([FromRoute(Name = "rideId"), Required] string rideId)
        {
            string authToken = string.Empty;

            Request.Headers.TryGetValue("Authorization", out StringValues authToken);
            return new OkObjectResult(await _requestRepository.CancelRideRequestAsync(new Guid(rideId), authToken));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetRide([FromRoute(Name = "rideId"), Required] string rideId)
        {
            string authToken = string.Empty;

            Request.Headers.TryGetValue("Authorization", out StringValues authToken);
            return new OkObjectResult(await _requestRepository.GetRideRequestAsync(new Guid(rideId), authToken));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> RequestRide([FromRoute(Name = "estimateId"), Required] string estimateId)
        {
            string authToken = string.Empty;

            Request.Headers.TryGetValue("Authorization", out StringValues authToken);
            return new OkObjectResult(await _requestRepository.CreateRideRequestAsync(new Guid(estimateId), authToken));
        }
    }
}
