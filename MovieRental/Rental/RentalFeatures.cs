using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Exceptions;
using MovieRental.PaymentProviders;
using System.ComponentModel.DataAnnotations;

namespace MovieRental.Rental
{
    public class RentalFeatures : IRentalFeatures
    {
        private readonly MovieRentalDbContext _movieRentalDb;
        private readonly PaymentProviderFactory _paymentProviderFactory;

        public RentalFeatures(MovieRentalDbContext movieRentalDb, PaymentProviderFactory paymentProviderFactory)
        {
            _movieRentalDb = movieRentalDb;
            _paymentProviderFactory = paymentProviderFactory;
        }

        public IEnumerable<Rental> GetRentalsByCustomerName(string customerName)
        {
            List<Rental> result = _movieRentalDb.Rentals
                .Include(r => r.Movie)
                .Include(r => r.Customer)
                .Where(r => r.Customer!.Name.Contains(customerName))
                .ToList();
            return result;
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName, CancellationToken cancellationToken)
        {
            List<Rental> result = await _movieRentalDb.Rentals
                .Include(r => r.Movie)
                .Include(r => r.Customer)
                .Where(r => r.Customer!.Name.Contains(customerName))
                .ToListAsync(cancellationToken);
            return result;
        }

        public async Task<Rental> PayAsync(Rental rental, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(rental.PaymentMethod))
            {
                throw new ValidationException("Invalid payment provider");
            }
            IPaymentProvider? provider = _paymentProviderFactory.Create(rental.PaymentMethod);
            if (provider is null)
            {
                throw new NotFoundException("Payment provider not supported");
            }
            bool payed = await provider.Pay(rental.PricePerDay * rental.DaysRented);
            if (!payed)
            {
                throw new NotFoundException("Payment was not sucessful");
            }
            return await SaveAsync(rental, cancellationToken);
        }

        public Rental Save(Rental rental)
        {
            _movieRentalDb.Rentals.Add(rental);
            _movieRentalDb.SaveChanges();
            return rental;
        }

        public async Task<Rental> SaveAsync(Rental rental, CancellationToken cancellationToken)
        {
            _movieRentalDb.Rentals.Add(rental);
            await _movieRentalDb.SaveChangesAsync(cancellationToken);
            return rental;
        }
    }
}