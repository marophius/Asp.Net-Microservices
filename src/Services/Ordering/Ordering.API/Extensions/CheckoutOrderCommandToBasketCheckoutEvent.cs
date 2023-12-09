using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.Extensions
{
    public static class CheckoutOrderCommandToBasketCheckoutEvent
    {
        public static CheckoutOrderCommand ToCheckoutOrderCommand(this BasketCheckoutEvent message)
        {
            return new CheckoutOrderCommand
            {
                UserName = message.UserName,
                TotalPrice = message.TotalPrice,
                FirstName = message.FirstName,
                LastName = message.LastName,
                AddressLine = message.AddressLine,
                Country = message.Country,
                State = message.State,
                ZipCode = message.ZipCode,
                CardName = message.CardName,
                CardNumber = message.CardNumber,
                CVV = message.CVV,
                PaymentMethod = message.PaymentMethod,
                EmailAddress = message.EmailAddress,
                Expiration = message.Expiration
            };
        }
    }
}
