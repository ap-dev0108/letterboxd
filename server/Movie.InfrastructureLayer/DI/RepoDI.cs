using Movie.Application.Interface.RepoInterface.GenreRepo;
using Movie.Application.Interface.RepoInterface.UserRepo;
using Movie.Infrastructure.Repository.FilmRepo;
using Movie.Infrastructure.Repository.GenreRepo;
using Movie.Infrastructure.Repository.UserRepo;

namespace Movie.Infrastructure.DI.Repo;

public static class RepoDI
{
    public static IServiceCollection RepoDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IFilmRepo, FilmRepo>();
        services.AddScoped<IGenreRepo, GenreRepo>();
        services.AddScoped<IUserRepo, UserRepo>();
        return services;
    }
}