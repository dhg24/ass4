using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ass4.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ass4.Controllers
{
    
    [ApiController]
    [Route("api/v1/collections")]
    public class CollectionsController : Controller

    {      

        public CollectionsController   (IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        private readonly IHttpClientFactory _httpClientFactory;
        private IConfiguration _config;

        //api/v1/collections/get-all
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var request1 = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/posts/");

          
            var httpClient1 = _httpClientFactory.CreateClient();
            var response1 = await httpClient1.SendAsync(request1);


          
            var validate = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var securityKey = _config.GetSection("Bearer").GetSection("key"). Value;  
            if (validate != securityKey)
            {
                throw new ArgumentException("The token provided must be invalid.");
            }


            if (!response1.IsSuccessStatusCode)
            {
                return NotFound();
            }
           
            string responseString1 = await response1.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Posts> Posts = JsonSerializer.Deserialize<List<Posts>>(responseString1, options1);
            //Geting first 30 results 
            List<Posts> postElements = Posts.GetRange(0, 30);
            String postString = JsonSerializer.Serialize(postElements);
            

            var request2 = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/albums/");
            var httpClient2 = _httpClientFactory.CreateClient();
            var response2 = await httpClient2.SendAsync(request2);

            if (!response2.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string responseString2 = await response2.Content.ReadAsStringAsync();
          
            
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Albums> Albums = JsonSerializer.Deserialize<List<Albums>>(responseString2, options2);
            //Geting first 30 results 
            List<Albums> albumElements = Albums.GetRange(0, 30);
            String albumString = JsonSerializer.Serialize(albumElements);




            var request3 = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/users/");
            var httpClient3 = _httpClientFactory.CreateClient();
            var response3 = await httpClient3.SendAsync(request3);

            if (!response3.IsSuccessStatusCode)
            {
                return NotFound();
            }
            string responseString3 = await response3.Content.ReadAsStringAsync();



           //Combine results
            var collectionString =  "post: \n" + postString +"\n album: \n"+ albumString  +"\n user: \n"+  responseString3;
               

            return Ok(collectionString);


        }

    }
}
