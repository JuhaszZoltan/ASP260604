using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;

namespace MoviesApp.Data;

public class SeedMovies
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using ApplicationDbContext context = new(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        if (context.Movies.Any()) return;

        context.Movies.AddRange(
            new Movie
            {
                Title = "Kill Bill Vol 1.",
                ReleaseDate = DateTime.Parse("2003-10-16"),
                Genre = "Action",
                Price = 6.99M,
            },
            new Movie
            {
                Title = "Kill Bill Vol 2.",
                ReleaseDate = DateTime.Parse("2004-04-16"),
                Genre = "Action",
                Price = 6.99M,
            },
            new Movie
            {
                Title = "Atomic Blonde",
                ReleaseDate = DateTime.Parse("2017-07-28"),
                Genre = "Thriller",
                Price = 10.49M,
            },
            new Movie
            {
                Title = "Enemy of the State",
                ReleaseDate = DateTime.Parse("1998-11-20"),
                Genre = "Thriller",
                Price = 5.79M,
            },
            new Movie
            {
                Title = "Judge Dredd",
                ReleaseDate = DateTime.Parse("1995-12-21"),
                Genre = "Sci-fi",
                Price = 4.99M,
            });
        context.SaveChanges();
    }
}
