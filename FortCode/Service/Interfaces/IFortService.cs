using FortCode.Model;
using FortCode.Model.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FortCode.Service.Interfaces
{
    public interface IFortService
    {
        Task<int> AddUserAsync(AddUserRequest addUserRequest);

        Task<string> AuthenticateUserAsync(User authenticateUser);
        Task<List<Country>> GetAllCountryByUserAsync(int id);
        Task<int> AddCountryAsync(List<AddCountryRequest> addCountryRequest, int userId);
        Task<int> DeleteCountryAsync(int? CountryID);
    }
}
