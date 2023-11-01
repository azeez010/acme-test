using AcmeCorp.Core.Utilities;
using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Interface.Repository;
using AcmeCorp.Domain.Interface.Service;
using AcmeCorp.Domain.Model;
using AcmeCorp.Domain.Response;
using AcmeCorp.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcmeCorp.Core.Service
{
    
    public class OrderService : IOrderService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public async Task<BaseResponse> CalculateCustomerOrderTotal(int customerId)
        {
            var data = await _customerRepository.GetByData(x => x.CustomerId == customerId);
            Customer cust = data.FirstOrDefault();
            if (cust is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "Invalid Customer Id");
            }
            decimal totalAmount = cust.Orders.Sum(order => order.Amount);
            return BaseResponse.Success(data: $"Total Order amount for {cust.FirstName} {cust.LastName} is {totalAmount}", message: "Order Successfully Removed");
        }

        public async Task<BaseResponse> CreateOrder(CreateOrderDto order)
        {
            if (order.Amount <= 0)
            {
                return BaseResponse.Error(message: "Invalid Amount");
            }
            var cust = await _customerRepository.GetByData(x => x.CustomerId == order.CustomerId);
            Customer customer = cust.FirstOrDefault();
            if (customer is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "No Valid Customer for this Order");
            }
            Order newOrder = new()
            {
                Amount = order.Amount,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status
            };
            await _orderRepository.Add(newOrder);

            customer.Orders.Add(newOrder);
            await _customerRepository.Update(customer);

            return BaseResponse.Success(data: newOrder);
        }

        public async Task<BaseResponse> DeleteOrder(int orderId)
        {
            var data = await _orderRepository.GetByData(x => x.OrderId == orderId);
            Order order = data.FirstOrDefault();
            if (order is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "Invalid Order Id");
            }
            
            var cust = await _customerRepository.GetByData(x => x.CustomerId == order.CustomerId);
            Customer customer = cust.FirstOrDefault();
            if (customer is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "No Valid Customer for this Order");
            }

            _orderRepository.Delete(order);
            customer.Orders.Remove(order);
            await _customerRepository.Update(customer);
            return BaseResponse.Success(data: order, message: "Order Successfully Removed");
        }

        public async Task<BaseResponse> GetAllOrders()
        {
            var allData = await _orderRepository.GetAll();
            if (allData is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "Try Again no Order is Available");
            }
            return BaseResponse.Success(data: allData);
        }

        public async Task<BaseResponse> GetOrderById(int orderId)
        {
            var data = await _orderRepository.GetByData(x => x.OrderId == orderId);
            Order cust = data.FirstOrDefault();
            if (cust is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "Invalid Order Id");
            }
            return BaseResponse.Success(data: cust);
        }

        public async Task<BaseResponse> GetOrdersByCustomerId(int customerId)
        {
            var data = await _customerRepository.GetByData(x => x.CustomerId == customerId);
            if (data is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "Invalid Customer Id");
            }
            return BaseResponse.Success(data: data);
        }

        public async Task<BaseResponse> ProcessOrder(int customerId)
        {
            var data = await _customerRepository.GetByData(x => x.CustomerId == customerId);
            Customer cust = data.FirstOrDefault();
            if (cust is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "Invalid Customer Id");
            }
            decimal amount = 0;
            //Orders that have not been processed
            var unprocessedOrder = cust.Orders.Where(x => x.Status == false);
            foreach(Order item in unprocessedOrder)
            {
                item.Status = true;
                amount = amount + item.Amount;
                await _orderRepository.Update(item);
            }
            return BaseResponse.Success(data: $"{cust.FirstName} {cust.LastName} Order has been processed with a Price of {amount}");
        }

        public async Task<BaseResponse> UpdateOrder(int orderId)
        {
            var data = await _orderRepository.GetByData(x => x.OrderId == orderId);
            Order order = data.FirstOrDefault();
            if (order is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "Order does not exist");
            }
            //Update Order for checkout
            if (order.Status == true)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "Order has been Processed");
            }
            order.Status = true;
            await _orderRepository.Update(order);
            return BaseResponse.Success(data: order);
        }
    }

}
