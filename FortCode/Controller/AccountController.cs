using FortCode.Common;
using FortCode.Model;
using FortCode.Model.Request;
using FortCode.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FortCode.Controller
{
    [Authorize]
    [Route(Routes.AccountRoute)]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IFortService _fortService;
        public AccountController(IFortService fortService)
        {
            _fortService = fortService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Route(Routes.AddUserRoute)]
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserRequest addUserRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _fortService.AddUserAsync(addUserRequest);
                    return response <= 0
                        ? response == -1
                            ? BadRequest("User already exists")
                            : StatusCode(StatusCodes.Status422UnprocessableEntity, "User Creation Failed")
                        : Ok("User created Successfully");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Route(Routes.AuthenticateUserRoute)]
        public async Task<IActionResult> AuthenticateUserAsync([FromBody] User addUserRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IActionResult responses = Unauthorized();
                    var response = await _fortService.AuthenticateUserAsync(addUserRequest);
                    return (response == "Wrong password" || response == "User Mail Address not found.") ? BadRequest(response) : (IActionResult)Ok(response);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Route(Routes.AddCountryRoute)]
        public async Task<IActionResult> AddCountryAsync([FromBody] List<AddCountryRequest> addCountryRequest)
        {
            try
            {
                var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
                var Claim = claimsIdentity.FindFirst("Id");
                var response = await _fortService.AddCountryAsync(addCountryRequest, Claim != null ? Convert.ToInt32(Claim.Value) : 0);
                return response > 0 ? Ok("Success") : StatusCode(StatusCodes.Status422UnprocessableEntity, "User Creation Failed");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [Route(Routes.GetCountryRoute)]
        public async Task<IActionResult> GetAllCountryByUserAsync()
        {
            try
            {
                var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
                var Claim = claimsIdentity.FindFirst("Id");
                var user = await _fortService.GetAllCountryByUserAsync(Claim.Value == null ? 0 : Convert.ToInt32(Claim.Value));

                return user == null ? BadRequest(new { message = "Username or password is incorrect" }) : (IActionResult)Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Route(Routes.DeleteCountryRoute)]
        public async Task<IActionResult> DeleteCountryAsync(int? CountryID)
        {
            try
            {
                var response = await _fortService.DeleteCountryAsync(CountryID);
                return response > 0 ? Ok("Country Deleted Successfully") : StatusCode(StatusCodes.Status422UnprocessableEntity, "Country Deleted Failed");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}