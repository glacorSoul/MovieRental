using MovieRental;
using MovieRental.Data;
using MovieRental.Movie;
using MovieRental.PaymentProviders;
using MovieRental.Rental;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddEntityFrameworkSqlite().AddDbContext<MovieRentalDbContext>();
        builder.Services.AddExceptionHandler<ExceptionHandler>();

        builder.Services.AddScoped<IRentalFeatures, RentalFeatures>();
        builder.Services.AddScoped<IMovieFeatures, MovieFeatures>();
        builder.Services.AddSingleton<PaymentProviderFactory>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler("/error");

        app.UseAuthorization();

        app.MapControllers();

        using (var client = new MovieRentalDbContext())
        {
            client.Database.EnsureCreated();
        }

        app.Run();
    }
}