namespace MovieRental.Rental;

public interface IRentalFeatures
{
    IEnumerable<Rental> GetRentalsByCustomerName(string customerName);

    Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName, CancellationToken cancellationToken);

    Task<Rental> PayAsync(Rental rental, CancellationToken cancellationToken);

    Rental Save(Rental rental);

    Task<Rental> SaveAsync(Rental rental, CancellationToken cancellationToken);
}