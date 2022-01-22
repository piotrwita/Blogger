using Application.Dto.Posts;
using EndToEndTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.Wrappers;
using Xunit;

namespace EndToEndTests.Controllers
{
    public class PostControllerTests
    {
        //dzieki temu mozemy uruchomic api w pamieci
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public PostControllerTests()
        {
            //Arrange - przygotowanie
            var projectDir = Helper.GetProjectPath("", typeof(Program).GetTypeInfo().Assembly);
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(projectDir)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.json")
                    .Build()
                )
                .UseStartup<Startup>());

            _client = _server.CreateClient();
        }

        [Fact]
        public async Task fetching_posts_should_return_not_empty_collection()
        {
            //Act
            var path = @"api/Posts";
            //wysyłanie żądania typu get na wskazany adres
            var response = await _client.GetAsync(path);
            //odczytanie rezeltatu
            var content = await response.Content.ReadAsStringAsync();
            //deserjalizacja zwracanego obiektu
            var pagedResponse = JsonConvert.DeserializeObject<PagedResponse<IEnumerable<PostDto>>>(content);

            //Assert
            //sprawdzenie czy api zwraca status 200 ok
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            pagedResponse.Data.Should().NotBeEmpty();
        }
    }
}
