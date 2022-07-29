using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using HireMe.Payments.Models;
using HireMe.Payments;

namespace HireMe.Controllers.Payments
{
    public class CheckoutController : Controller
    {
        private readonly string _clientID;
        private readonly string _clientSecret;

        private readonly PaypalClient _paypalClient;

        public CheckoutController(IConfiguration config, PaypalClient paypalClient)
        {
            _paypalClient = paypalClient;

            _clientID = config.GetValue<string>("PayPal:ClientId");
            _clientSecret = config.GetValue<string>("PayPal:ClientSecret");
        }

        [HttpPost]
        public async Task<IActionResult> Paypal()
        {
            var accesstoken = await _paypalClient.GetToken(_clientID, _clientSecret);
            if (accesstoken != null)
            {
                var order = new Order()
                {
                    Intent = "CAPTURE",
                    Purchase_units = new List<PurchaseUnit>() {

                            new PurchaseUnit() {

                                Amount = new Amount() {
                                    Currency_code = "EUR",
                                    Value = "5",
                                    Breakdown = new Breakdown()
                                    {
                                        Item_total = new Amount()
                                        {
                                            Currency_code="EUR",
                                            Value ="5"
                                        }
                                    }
                                },
                                Description = "Content Boosting",
                                Items = new List<Items>
                                {
                                    new Items()
                                    {
                                        Name = "Gold Package",
                                        Unit_amount = new Amount()
                                        {
                                            Currency_code = "EUR",
                                            Value = "5"
                                        },
                                        Quantity = "1"
                                    }
                                }
                            }
                        },
                    Application_context = new ApplicationContext()
                    {
                        Brand_name = "GrandJob.eu",
                        Landing_page = "NO_PREFERENCE",
                        User_action = "PAY_NOW", //Accion para que paypal muestre el monto de pago
                        Return_url = "https://localhost:44360/identity/checkout/Success",// cuando se aprovo la solicitud del cobro
                        Cancel_url = "https://localhost:44360/identity/checkout/Cancel"// cuando cancela la operacion
                    }
                };
                var orderResult = await _paypalClient.CreateOrder(order, accesstoken);
                if (orderResult != null)
                {
                    return Redirect(orderResult.Links[1].Href);
                }
            }
            return Redirect("~/Identity/Checkout/Cancel");
        }

        [HttpGet]
        public async Task<IActionResult> Success(string token)
        {
            var accesstoken = await _paypalClient.GetToken(_clientID, _clientSecret);
            if (accesstoken != null)
            {
                var result = await _paypalClient.CaptureOrder(accesstoken, token);
                if (result)
                {
                    return Redirect("~/Identity/Checkout/Success");
                }
            }
            return Redirect("~/Identity/Checkout/Cancel");
        }

        [HttpGet]
        public async Task<IActionResult> Cancel()
        {
            return Redirect("~/Identity/Checkout/Cancel");
        }
    }
}
