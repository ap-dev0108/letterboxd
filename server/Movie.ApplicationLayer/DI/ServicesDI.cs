using Movie.Application.Interface.FilmInterface;
using Movie.Application.Interface.FilmInterface.GenreInterface;
using Movie.Application.Interface.TokenInterface;
using Movie.Application.Interface.UserInterface;
using Movie.Application.Service;
using Movie.Application.Service.GenreService;
using Movie.Application.Service.TokenService;
using Movie.Application.Service.UserService;
using Movie.Domain.Entities.Token;

namespace Movie.Application.DI.Services;

public static class ServiceDI
{
    public static IServiceCollection ServiceDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IFilmInterface, FilmService>();
        services.AddScoped<FilmService>();

        services.AddScoped<IGenreInterface, GenreService>();
        services.AddScoped<GenreService>();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<TokenService>();
        services.AddTransient<TokenSettings>();

        services.AddScoped<IUserInterface, UserService>();
        services.AddScoped<UserService>();
        return services;
    }
}