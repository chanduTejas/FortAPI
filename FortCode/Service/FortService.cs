using FortCode.Model;
using FortCode.Model.Request;
using FortCode.Repository.Interfaces;
using FortCode.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortCode.Service
{
    public class FortService : IFortService
    {
        private readonly IFortRepository _fortRepository;
        public FortService(IFortRepository fortRepository)
        {
            _fortRepository = fortRepository;
        }

        public async Task<int> AddUserAsync(AddUserRequest addUserRequest)
        {
            if (string.IsNullOrEmpty(addUserRequest.Email))
            {
                throw new ArgumentNullException(nameof(addUserRequest.Email));
            }
            return await _fortRepository.AddUserAsync(addUserRequest);
        }

        public async Task<string> AuthenticateUserAsync(User authenticateUser)
        {
            var user = await _fortRepository.AuthenticateUserAsync(authenticateUser);

            if (user == null)
                return null;
            else
                return user;
        }

        public async Task<List<Country>> GetAllCountryByUserAsync(int id)
        {
            return await _fortRepository.GetAllCountryByUserAsync(id);
        }

        public async Task<int> AddCountryAsync(List<AddCountryRequest> addCountryRequest, int userId)
        {
            if (addCountryRequest.Count() == 0)
            {
                throw new ArgumentNullException("Empty Data");
            }
            return await _fortRepository.AddCountryAsync(addCountryRequest, userId);
        }

        public async Task<int> DeleteCountryAsync(int? CountryID)
        {
            return await _fortRepository.DeleteCountryAsync(CountryID);
        }

    }
}
