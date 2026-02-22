using Movie.Domain.Entities;

namespace Movie.Application.Interface.RepoInterface.AuthRepos;

public interface IAuthRepo
{
    Task<User> GetRolesAsync(string token);
}