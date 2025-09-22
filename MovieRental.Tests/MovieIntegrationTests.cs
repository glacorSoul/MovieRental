using Microsoft.AspNetCore.Mvc.Testing;
using MovieRental.Controllers;
using MovieRental.Movie;
using System.Net.Http.Json;

namespace MovieRental.Tests
{
    public class MovieIntegrationTests : WebApplicationFactory<Program>
    {
        [Fact]
        public async Task CreateMovieCanReturnMovie()
        {
            HttpClient client = CreateClient();
            HttpResponseMessage response = await client.PostAsJsonAsync("/Movie", new Movie.Movie
            {
                Title = "The Matrix"
            });
            response.EnsureSuccessStatusCode();

            IEnumerable<Movie.Movie>? movies = await client.GetFromJsonAsync<IEnumerable<Movie.Movie>>("/Movie");
            Assert.NotNull(movies);
            Assert.NotEmpty(movies);
            Assert.Contains(movies, m => m.Title == "The Matrix");
        }
    }
}