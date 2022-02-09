using Microsoft.AspNetCore.Mvc;
using RequestService.Policies;

namespace RequestService.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        //private readonly ClientPolicy _clientPolicy;

        private readonly IHttpClientFactory _clientFactory;

        public RequestController(//ClientPolicy clientPolicy,
            IHttpClientFactory clientFactory)
        {
            //_clientPolicy = clientPolicy;
            _clientFactory = clientFactory;
        }

        // GET api/v1/request
        [HttpGet]
        public async Task<ActionResult> MakeRequest()
        {
            var client = _clientFactory.CreateClient("Test");

            var response = await client.GetAsync("https://localhost:7155/api/v1/response/25");

            // var response = await _clientPolicy.ExponentialHttpRetry.ExecuteAsync(
            //     () => client.GetAsync("https://localhost:7155/api/v1/response/25"));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> ResponseService returned SUCCESS");
                return Ok();
            }
            Console.WriteLine("--> ResponseService returned FAILURE");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}