using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RequestAPI.Repository;

namespace RequestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        public RequestController(IRequestRepository requestRepository) => _requestRepository = requestRepository;
    }
}
