namespace HireMe.Payments.Models
{
using System.Collections.Generic;


    public class Order
    {
        public string Intent { get; set; }
        public List<PurchaseUnit> Purchase_units { get; set; }
        public ApplicationContext Application_context { get; set; }
    }

    public class OrderResult
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public List<LinkDescription> Links { get; set; }
    }

    public class LinkDescription
    {
        public string Href { get; set; }

        public string Rel { get; set; }

        public string Method { get; set; }
    }

    public class PurchaseUnit
    {
        public Amount Amount { get; set; }
        public string Description { get; set; }
        public List<Items> Items { get; set; }
    }

    public class Items
    {
        public string Name { get; set; }
        public Amount Unit_amount { get; set; }
        public string Quantity { get; set; }
    }

    public class Amount
    {
        public string Currency_code { get; set; }
        public string Value { get; set; }
        public Breakdown Breakdown { get; set; }
    }

    public class Breakdown
    {
        public Amount Item_total { get; set; }
    }

    public class ApplicationContext
    {
        public string Brand_name { get; set; }
        public string Landing_page { get; set; }
        public string User_action { get; set; }
        public string Return_url { get; set; }
        public string Cancel_url { get; set; }
    }

    public class AccessToken
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public int expires_in { get; set; }
        public string nonce { get; set; }
    }

}
