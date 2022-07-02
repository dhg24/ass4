using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ass4.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;

namespace ass4.Controllers
{
    [ApiController]
    [Route("api/v1/albums")]
    public class AlbumsController : Controller
    {
        public AlbumsController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        private readonly IHttpClientFactory _httpClientFactory;
        private IConfiguration _config;

        ///api/v1/albums/get-all

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/albums/");

        
            var httpClient = _httpClientFactory.CreateClient();     
            var response = await httpClient.SendAsync(request);

            var validate = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var securityKey = _config.GetSection("Bearer").GetSection("key").Value;
            if (validate != securityKey)
            {
                throw new ArgumentException("The token provided must be invalid.");
            }


            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            string responseString = await response.Content.ReadAsStringAsync();
            return Ok(responseString);
        }

        //api/v1/albums/album?id=10
        [HttpGet]
        [Route("album")]
        public async Task<IActionResult> Get(int id)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/albums/{id}");

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);
            var validate = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var securityKey = _config.GetSection("Bearer").GetSection("key").Value;
            if (validate != securityKey)
            {
                throw new ArgumentException("The token provided must be invalid.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            string responseString = await response.Content.ReadAsStringAsync();
            return Ok(responseString);

        }

        //api/v1/albums/create
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Post(Albums newUsers)
        {
            var newAlbumsJson = JsonSerializer.Serialize<Albums>(newUsers);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://jsonplaceholder.typicode.com/albums/");

            request.Content = new StringContent(newAlbumsJson, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);
            var validate = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var securityKey = _config.GetSection("Bearer").GetSection("key").Value;
            if (validate != securityKey)
            {
                throw new ArgumentException("The token provided must be invalid.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Failed" });
            }

            return Ok(new { Message = "Success" });
        }


        //api/v1/albums/delete?id=2
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://jsonplaceholder.typicode.com/albums/{id}");

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);
            var validate = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var securityKey = _config.GetSection("Bearer").GetSection("key").Value;
            if (validate != securityKey)
            {
                throw new ArgumentException("The token provided must be invalid.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Failed To Delete Todo" });
            }
            return Ok(new { Message = "Successfully Deleted Todo" });
        }

        //api/v1/albums/update?id=2
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Put(int id, Albums newAlbums)
        {
            var newAlbumsJson = JsonSerializer.Serialize<Albums>(newAlbums);

            var request = new HttpRequestMessage(HttpMethod.Put, $"https://jsonplaceholder.typicode.com/albums/{id}");

            request.Content = new StringContent(newAlbumsJson, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);
            var validate = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var securityKey = _config.GetSection("Bearer").GetSection("key").Value;
            if (validate != securityKey)
            {
                throw new ArgumentException("The token provided must be invalid.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Failed To Update Todo" });
            }
            return Ok(new { Message = "Successfully Update Todo" });
        }

    }
}
