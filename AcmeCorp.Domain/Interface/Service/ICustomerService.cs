using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Interface.Repository;
using AcmeCorp.Domain.Model;
using AcmeCorp.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Interface.Service
{
    public interface ICustomerService 
    {
        Task<BaseResponse> GetCustomers();
        Task<BaseResponse> GetCustomer(int Id);
        Task<BaseResponse> CreateCustomer(CreateCustomerDto customer);
        Task<BaseResponse> UpdateCustomer(int customerId, Customer updatedCustomer);
        Task<BaseResponse> DeleteCustomer(int customerId);
        Task<BaseResponse> Login(LoginDTO login);
    }
}
