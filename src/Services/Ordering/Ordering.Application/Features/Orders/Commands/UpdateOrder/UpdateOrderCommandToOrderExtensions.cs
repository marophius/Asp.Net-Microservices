using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public static class UpdateOrderCommandToOrderExtensions
    {
        

        public static void ChangeOrder(this Order order, UpdateOrderCommand command)
        {
            order.UserName = command.UserName;
            order.TotalPrice = command.TotalPrice;
            order.FirstName = command.FirstName;
            order.LastName = command.LastName;
            order.EmailAddress = command.EmailAddress;
            order.AddressLine = command.AddressLine;
            order.Country = command.Country;
            order.State = command.State;
            order.ZipCode = command.ZipCode;
            order.CardName = command.CardName;
            order.CardNumber = command.CardNumber;
            order.Expiration = command.Expiration;
            order.CVV = command.CVV;
            order.PaymentMethod = command.PaymentMethod;
        }
    }
}
