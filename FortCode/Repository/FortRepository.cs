using System.Threading.Tasks;
using FortCode.Repository.Interfaces;
using FortCode.Service.Interfaces;
using FortCode.Model.Request;
using Dapper;
using System.Transactions;
using FortCode.Common;
using FortCode.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Utility = FortCode.Common.Utility;

namespace FortCode.Repository.FortRepository
{
    public class FortRepository : IFortRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IConfiguration _config;

        public FortRepository(IDbConnectionFactory dbConnectionFactory, IConfiguration config)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _config = config;
        }

        public async Task<int> AddUserAsync(AddUserRequest addUserRequest)
        {
            try
            {
                addUserRequest.Password = Utility.Encryptdata(addUserRequest.Password.ToString());
                if (UserExist(addUserRequest.Email))
                {
                    // "User already exists.";
                    return -1;
                }
                using var databaseConnection = _dbConnectionFactory.GetConnection();
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var rowsAffected = await databaseConnection.ExecuteAsync(SqlQueries.InsertUserQuery, new
                {
                    addUserRequest.Username,
                    addUserRequest.Password,
                    addUserRequest.Email
                });
                transaction.Complete();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UserExist(string Email)
        {
            try
            {
                using var databaseConnection = _dbConnectionFactory.GetConnection();
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var userData = databaseConnection.QuerySingleOrDefault<bool>(SqlQueries.UserExistQuery, new
                {
                    Email
                });
                transaction.Complete();
                return userData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> AuthenticateUserAsync(User authenticateUser)
        {
            try
            {
                using var databaseConnection = _dbConnectionFactory.GetConnection();
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var userData = await databaseConnection.QueryFirstOrDefaultAsync<User>(SqlQueries.AuthenticateUserQuery, new
                {
                    authenticateUser.Email
                });
                transaction.Complete();
                if (userData == null)
                {
                    return "User Mail Address not found.";
                }
                else if (!VerifyPasswordHash(authenticateUser.Password, userData.Password.ToString()))
                {
                    return "Wrong password";
                }
                return GenerateJWTToken(userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool VerifyPasswordHash(string password, string DBpassword)
        {
            string EncrPassword = Utility.Encryptdata(password);
            if (EncrPassword == DBpassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<int> AddCountryAsync(List<AddCountryRequest> addCountryRequest, int userId)
        {
            try
            {
                using var databaseConnection = _dbConnectionFactory.GetConnection();
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var dynamicpar = new List<DynamicParameters>();
                foreach (var item in addCountryRequest)
                {
                    var param = new DynamicParameters();
                    param.Add("@userId", userId);
                    param.Add("@CountryName", item.CountryName);
                    param.Add("@City", item.City);
                    dynamicpar.Add(param);
                }
                var rowsAffected = await databaseConnection.ExecuteAsync(SqlQueries.InsertCountryQuery, dynamicpar);
                transaction.Complete();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Country>> GetAllCountryByUserAsync(int id)
        {
            try
            {
                using var databaseConnection = _dbConnectionFactory.GetConnection();
                var userCountryData = await databaseConnection.QueryAsync<Country>(SqlQueries.GetAllCountryByUserQuery, new { id });
                return userCountryData.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteCountryAsync(int? CountryID)
        {
            try
            {
                using var databaseConnection = _dbConnectionFactory.GetConnection();
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var rowsAffected = await databaseConnection.ExecuteAsync(SqlQueries.DeleteCountry, new
                {
                    CountryID
                });
                transaction.Complete();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string GenerateJWTToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new System.Security.Claims.Claim("id",userInfo.Id.ToString()),
                new System.Security.Claims.Claim("Name", userInfo.Name),
                new System.Security.Claims.Claim("Email", userInfo.Email),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}

