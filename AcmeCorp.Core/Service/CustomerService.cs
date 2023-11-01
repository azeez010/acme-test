using AcmeCorp.Core.Utilities;
using AcmeCorp.Domain;
using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Interface.Repository;
using AcmeCorp.Domain.Interface.Service;
using AcmeCorp.Domain.Model;
using AcmeCorp.Domain.Response;
using AcmeCorp.Infrastructure.Repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcmeCorp.Core.Service
{
    
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly JwtTokenConfig _jwtTokenConfig;

        public CustomerService(ICustomerRepository customerRepository, IOptions<JwtTokenConfig> jwtTokenConfig)
        {
            _customerRepository = customerRepository;
            _jwtTokenConfig = jwtTokenConfig.Value;
        }

        public async Task<BaseResponse> GetCustomers()
        {
            var allUser = await _customerRepository.GetAll();
            return BaseResponse.Success(data: allUser);
        }

        public async Task<BaseResponse> GetCustomer(int Id)
        {
            var user = await _customerRepository.GetById(Id);
            if (user is null)
            {
                return BaseResponse.Error(message: "Customer does not Exist, Try with different Details");
            }
            return BaseResponse.Success(data: user);
        }

        public async Task<BaseResponse> CreateCustomer(CreateCustomerDto customer)
        {
            if (customer == null)
            {
                return BaseResponse.Error(message: $"{nameof(customer)} is Empty");
            }

            if (string.IsNullOrEmpty(customer.FirstName) ||
            string.IsNullOrEmpty(customer.LastName) ||
            string.IsNullOrEmpty(customer.Email) ||
            string.IsNullOrEmpty(customer.UserName) ||
            string.IsNullOrEmpty(customer.Password))
            {
                return BaseResponse.Error(message: "Ensure all Data is Filled");
            }
            //check if password is strong
            if (Extension.IsStrongPassword(customer.Password) != true)
            {
                //we can create a Random Password Generator
                return BaseResponse.Error(message: "Weak Password! Enter a Strong Password which " +
                    "contain a Capital, Lower, Special Character, Number and a Lenght of 8 upward");
            }
            var userData = await _customerRepository.GetByData(x => x.UserName == customer.UserName || x.Email == customer.Email);

            if (userData.FirstOrDefault() is not null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "UserName/Email already Exist, Try with different Details");
            }

            // Check for validation or business logic before adding to the database
            if (IsEmailValid(customer.Email))
            {
                Customer cust = new Customer()
                {
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Password = HasherExtension.HashPassword(customer.Password),
                    UserName = customer.UserName
                };
                await _customerRepository.Add(cust);
                return BaseResponse.Success(data: customer, message: $"Customer Creation Success");

            }
            else
            {
                return BaseResponse.Error(message: "Invalid email address");
            }
        }

        public async Task<BaseResponse> UpdateCustomer(int customerId, Customer updatedCustomer)
        {
            if (updatedCustomer == null || customerId != updatedCustomer.CustomerId)
            {
                return BaseResponse.Error(message:"Invalid customer data");
            }

            var existingCustomer = _customerRepository.GetById(customerId);
            if (existingCustomer == null)
            {
                return BaseResponse.Error(message: "Customer not found");
            }

            await _customerRepository.Update(updatedCustomer);
            return BaseResponse.Success(data: updatedCustomer, message: $"Customer Info Updated Successfully");
            
        }

        public async Task<BaseResponse> DeleteCustomer(int customerId)
        {
            var existingCustomer = await _customerRepository.GetById(customerId);
            if (existingCustomer == null)
            {
                return BaseResponse.Error(message: "Customer not found");
            }

            _customerRepository.Delete(existingCustomer);
            return BaseResponse.Success(data:"Customer Successfully Removed");
        }

        public async Task<BaseResponse> Login(LoginDTO login)
        {
            var data = await _customerRepository.GetByData(x => x.Email == login.Email || x.UserName == login.Email);
            var user = data.FirstOrDefault();
            if (data is null)
                return BaseResponse.Error(message: "No User Exist Create an Account");
            bool status = HasherExtension.VerifyPassword(login.Password, user.Password);

            if (status == false)
                return BaseResponse.Error(message: "Invalid Password!");

            string trx = Guid.NewGuid().ToString().Replace("-", "");
            return BaseResponse.Success(data: generateJwtToken(user), message: "Login Successful");
        }

        // Add other methods for specific business logic or validation as needed

        private bool IsEmailValid(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private object generateJwtToken(Customer client)
        {
            DateTime now = DateTime.Now;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenConfig.Secret);
            var claims = new List<Claim>
            {
                new Claim("ID", client.CustomerId.ToString()),
                new Claim(ClaimTypes.UserData, client.UserName),
                new Claim(ClaimTypes.Email, client.Email),
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtTokenConfig.Issuer,
                Audience = _jwtTokenConfig.Audience,
                IssuedAt = now,
                Subject = new ClaimsIdentity(claims),
                Expires = now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenResp = new 
            { 
                AccessToke = tokenHandler.WriteToken(token),
                Id = client.CustomerId.ToString(),
                ExpirationTime = now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                RefreshToken = now.AddMonths(_jwtTokenConfig.RefreshTokenExpiration)
            };
           
            return tokenResp;
        }

    }

}
