using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.API.Extensions
{
    public static class BasketCheckoutToBasketCheckoutEvent
    {
        public static BasketCheckoutEvent ToBasketCheckoutEvent(this BasketCheckout checkout)
        {
            return new BasketCheckoutEvent
            {
                UserName = checkout.UserName,
                TotalPrice = checkout.TotalPrice,
                FirstName = checkout.FirstName,
                LastName = checkout.LastName,
                EmailAddress = checkout.EmailAddress,
                AddressLine = checkout.AddressLine,
                State = checkout.State,
                Country = checkout.Country,
                ZipCode = checkout.ZipCode,
                CardName = checkout.CardName,
                CardNumber = checkout.CardNumber,
                PaymentMethod = checkout.PaymentMethod,
                CVV = checkout.CVV,
                Expiration = checkout.Expiration
            };
        }
    }
}
