using Microsoft.AspNetCore.Mvc.Testing;
using MovieRental.Controllers;
using MovieRental.Movie;
using System.Net.Http.Json;

namespace MovieRental.Tests
{
    public class RentalIntegrationTests : WebApplicationFactory<Program>
    {
        [Fact]
        public async Task PayRental_AvailablePaymentMethod_CanPay()
        {
            HttpResponseMessage rentalResponse = await Rental("paypal");
            Assert.Equal(System.Net.HttpStatusCode.OK, rentalResponse.StatusCode);
        }

        [Fact]
        public async Task PayRental_InvalidPaymentMethod_CanPay()
        {
            HttpResponseMessage rentalResponse = await Rental("creditCard");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, rentalResponse.StatusCode);
        }

        [Fact]
        public async Task PayRental_UnavailablePaymentMethod_CanPay()
        {
            HttpResponseMessage rentalResponse = await Rental(" ");
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, rentalResponse.StatusCode);
        }

        private async Task<HttpResponseMessage> Rental(string paymentMethod)
        {
            HttpClient client = CreateClient();
            HttpResponseMessage movieResponse = await client.PostAsJsonAsync("/Movie", new Movie.Movie
            {
                Title = "The Mask"
            });
            movieResponse.EnsureSuccessStatusCode();

            IEnumerable<Movie.Movie> movies = await client.GetFromJsonAsync<IEnumerable<Movie.Movie>>("/Movie") ?? [];
            Movie.Movie movie = movies!.First(m => m.Title == "The Mask");

            HttpResponseMessage rentalResponse = await client.PostAsJsonAsync("/Rental", new Rental.Rental
            {
                MovieId = movie.Id,
                Customer = new Customer.Customer
                {
                    Name = "John Doe"
                },
                DaysRented = 3,
                PricePerDay = 2.99d,
                PaymentMethod = paymentMethod
            });
            return rentalResponse;
        }
    }
}