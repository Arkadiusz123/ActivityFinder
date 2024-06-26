﻿using ActivityFinder.Server.Database;
using ActivityFinder.Server.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActivityFinderTests
{
    public class ActivityControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly DbConfig _dbConfig;

        public ActivityControllerTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.GetClientWithToken();
            _dbConfig = new DbConfig(factory);
        }

        [Fact]
        public async Task CreateTest()
        {
            await _dbConfig.InitializeData();

            var address = new AddressDTO { OsmId = "w1284375894" };
            var activity = new ActivityDTO 
            { 
                Address = address, 
                Title = "Testowe wydarzenie",
                Description = "desc",
                Date = DateTime.Now,
            };

            var response = await _client.PostAsJsonAsync("api/activity", activity);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var createdEntity = JsonSerializer.Deserialize<ActivityDTO>(responseString, options);

            Assert.NotNull(createdEntity);
            Assert.Equal("Testowe wydarzenie", createdEntity.Title);

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var entityInDb = await context.Activities.FindAsync(createdEntity.Id);
                Assert.NotNull(entityInDb);
                Assert.Equal("Testowe wydarzenie", entityInDb.Title);
            }
        }

        [Fact]
        public async Task CreateTestInvalidData()
        {
            await _dbConfig.InitializeData();

            var address = new AddressDTO { OsmId = "w1284375894" };
            var activity = new
            {
                Address = address,
                Description = "desc",
                Date = DateTime.Now,
            };

            var response = await _client.PostAsJsonAsync("api/activity", activity);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
