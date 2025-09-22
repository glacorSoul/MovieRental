using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRental.Rental
{
    public class Rental
    {
        public virtual Customer.Customer? Customer { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        public int DaysRented { get; set; }

        [Key]
        public int Id { get; set; }

        public Movie.Movie? Movie { get; set; }

        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }

        public string PaymentMethod { get; set; }
        public double PricePerDay { get; set; }
    }
}