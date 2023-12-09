using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public static class OrdersToOrdersVmExtensions
    {
        public static OrdersVm ToOrdersVm(this Order order)
        {
            return new OrdersVm
            {
                Id = order.Id,
                UserName = order.UserName,
                TotalPrice = order.TotalPrice,
                FirstName = order.FirstName,
                LastName = order.LastName,
                EmailAddress = order.EmailAddress,
                AddressLine = order.AddressLine,
                Country = order.Country,
                State = order.State,
                ZipCode = order.ZipCode,
                CardName = order.CardName,
                CardNumber = order.CardNumber,
                PaymentMethod = order.PaymentMethod,
                Expiration = order.Expiration,
                CVV = order.CVV
            };
        }
    }
}
