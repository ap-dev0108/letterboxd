namespace Movie.Application.Interface.DataInterface;

public interface IDataHelper
{
    public (string userId, string role) GetTokenDetails();
}