using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Threading;

namespace MovieRental.Wpf
{
    public class MovieRentalViewModel : INotifyPropertyChanged
    {
        private static HttpClient Http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7271")
        };

        public MovieRentalViewModel()
        {
            _ = KeppDataUpToDate();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CustomerName { get; set; }

        public ObservableCollection<Movie.Movie> Movies { get; } = new();

        public ObservableCollection<Rental.Rental> Rentals { get; } = new();

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private async Task<IEnumerable<Movie.Movie>> GetMovies(CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<Movie.Movie> movies = await Http.GetFromJsonAsync<IEnumerable<Movie.Movie>>("/Movie", cancellationToken) ?? [];
                return movies;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        private async Task<IEnumerable<Rental.Rental>> GetRentalsByCustomerName(string customerName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                return [];
            }
            try
            {
                IEnumerable<Rental.Rental> rentals = await Http.GetFromJsonAsync<IEnumerable<Rental.Rental>>($"/Rental?customerName={customerName}", cancellationToken) ?? [];
                return rentals;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        private async Task KeppDataUpToDate()
        {
            while (true)
            {
                await RefreshData();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async Task RefreshData()
        {
            IEnumerable<Movie.Movie> movies = await GetMovies(CancellationToken.None);
            IEnumerable<Rental.Rental> rentals = await GetRentalsByCustomerName(CustomerName, CancellationToken.None);

            Movies.Clear();
            Rentals.Clear();

            foreach (Movie.Movie movie in movies)
            {
                Movies.Add(movie);
            }
            foreach (Rental.Rental rental in rentals)
            {
                Rentals.Add(rental);
            }
        }
    }
}