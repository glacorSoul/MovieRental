using Microsoft.AspNetCore.Mvc;
using MovieRental.Movie;
using MovieRental.Rental;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }

        [HttpGet]
        public async Task<ActionResult> GetByCustomerName([FromQuery] string customerName, CancellationToken cancellationToken)
        {
            IEnumerable<Rental.Rental> rentals = await _features.GetRentalsByCustomerNameAsync(customerName, cancellationToken);
            return Ok(rentals);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rental.Rental rental, CancellationToken cancellationToken)
        {
            Rental.Rental savedRental = await _features.PayAsync(rental, cancellationToken);
            return Ok(savedRental);
        }
    }
}