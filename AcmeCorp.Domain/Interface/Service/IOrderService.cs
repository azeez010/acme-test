using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Model;
using AcmeCorp.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Interface.Service
{
    public interface IOrderService 
    {
        Task<BaseResponse> CalculateCustomerOrderTotal(int customerId);
        Task<BaseResponse> CreateOrder(CreateOrderDto order);
        Task<BaseResponse> DeleteOrder(int orderId);
        Task<BaseResponse> GetAllOrders();
        Task<BaseResponse> GetOrderById(int orderId);
        Task<BaseResponse> GetOrdersByCustomerId(int customerId);
        Task<BaseResponse> UpdateOrder(int orderId);
        Task<BaseResponse> ProcessOrder(int customerId);

    }
}
