namespace HireMe.Areas.Identity.Pages.Checkout
{
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Payments;
    using HireMe.Payments.Models;
    using HireMe.Services.Interfaces;
    using HireMe.StoredProcedures.Enums;
    using HireMe.StoredProcedures.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IContestantsService _contestantsService;
        private readonly IspJobService _spJobService;
        private readonly IBaseService _baseService;
        private readonly IPromotionService _promotionService;

        private readonly string _clientID;
        private readonly string _clientSecret;

        private readonly PaypalClient _paypalClient;

        public IndexModel(
            IConfiguration config, 
            PaypalClient paypalClient,
            UserManager<User> userManager,
            IspJobService spJobService,
            IContestantsService contestantsService,
            IBaseService baseService,
            IPromotionService promotionService)
        {
            _userManager = userManager;
            _baseService = baseService;
            _contestantsService = contestantsService;
            _spJobService = spJobService;
            _promotionService = promotionService;

            _paypalClient = paypalClient;
            _clientID = config.GetValue<string>("PayPal:ClientId");
            _clientSecret = config.GetValue<string>("PayPal:ClientSecret");
        }


        public User UserEntity { get; set; }
        public bool BoostedPost { get; set; } = false;
        public bool BoostedPostInHome { get; set; } = false;
        public int RefreshCount { get; set; } = 0;
        public bool AutoSuggestion { get; set; } = false;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel 
        {
            public int Price { get; set; }
            public double DDS { get; set; } = 0.49661;
            public double PayPrice { get; set; }
            public string PackageName { get; set; }

            public int productId { get; set; }
            public PostType postType { get; set; }
            public PremiumPackage PremiumPackage { get; set; }
        }


        public async Task<IActionResult> OnGetAsync(int package, int productId, int postType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.AccountType == 0)
            {
                return RedirectToPage("/Account/Manage/Pricing", new { Area = "Identity" });
            }

            UserEntity = user;
            PremiumPackage type = EnumHelper.GetEnumValue<PremiumPackage>(package);

            Input = new InputModel
            {
                productId = productId,
                PremiumPackage = type,
                postType = EnumHelper.GetEnumValue<PostType>(postType)
            };

            switch (type)
            {
                case PremiumPackage.Bronze:
                    Input.PackageName = "Bronze";
                    Input.Price = 1;
                    Input.PayPrice = Math.Round(Input.DDS + Input.Price, 2);
                    break;
                case PremiumPackage.Silver:
                    Input.PackageName = "Silver";
                    Input.Price = 3;
                    Input.PayPrice = Math.Round((Input.DDS + Input.DDS + (Input.DDS / 2)) + Input.Price, 2);
                    break;
                case PremiumPackage.Gold:
                    Input.PackageName = "Gold";
                    Input.Price = 5;
                    Input.PayPrice = Math.Round((Input.DDS + Input.DDS + Input.DDS + Input.DDS + Input.DDS) + Input.Price, 2);
                    break;
                case PremiumPackage.None:
                    Input.PayPrice = 0;
                    Input.PackageName = "Няма";
                    Input.Price = 0;
                    break;
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;


            switch (Input.PremiumPackage)
            {
                case PremiumPackage.Bronze:
                    Input.PackageName = "Bronze";
                    Input.Price = 1;
                    Input.PayPrice = Math.Round(Input.DDS + Input.Price, 2);
                    break;
                case PremiumPackage.Silver:
                    Input.PackageName = "Silver";
                    Input.Price = 3;
                    Input.PayPrice = Math.Round((Input.DDS + Input.DDS + (Input.DDS / 2)) + Input.Price, 2);
                    break;
                case PremiumPackage.Gold:
                    Input.PackageName = "Gold";
                    Input.Price = 5;
                    Input.PayPrice = Math.Round((Input.DDS + Input.DDS + Input.DDS + Input.DDS + Input.DDS) + Input.Price, 2);
                    break;
                case PremiumPackage.None:
                    Input.PayPrice = 0;
                    Input.PackageName = "Няма";
                    Input.Price = 0;
                    break;
            };
          //  productId = Input.productId;
          //  PackageType = Input.PremiumPackage;
           // postType = Input.postType;

            switch (Input.postType)
            {
                case PostType.Job:
                    {

                        var job = await _spJobService.GetByIdAsync<Jobs>(Input.productId);

                        if (job is not null)
                        {
                            var PayingResult = await PayingAction(Input.Price, Input.PackageName, 1, Input.productId, Input.PremiumPackage, Input.postType);
                            if (PayingResult.Success)
                            {
                                /*await _promotionService.Create(new CreatePromotion { 
                                    productId = job.Id, 
                                    premiumPackage = Input.PremiumPackage, 
                                    PostType = PostType.Job,
                                    StartTime = DateTime.Now, 
                                    EndTime = job.ExpiredOn, 
                                }, user);*/

                               return Redirect(PayingResult.SuccessMessage);
                            }
                        return Redirect(PayingResult.FailureMessage);
                        }
                    }
                    break;
                case PostType.Contestant:
                    {
                        var contestant = await _contestantsService.GetByIdAsync(Input.productId);

                        if (contestant is not null)
                        {
                            var PayingResult = await PayingAction(Input.Price, Input.PackageName, 1, Input.productId, Input.PremiumPackage, Input.postType);
                            if (PayingResult.Success)
                            {
                             //   return RedirectToPage("./Index", pageHandler: "Callback", new { PayingResult.SuccessMessage });
                                 return Redirect(PayingResult.SuccessMessage);
                            }
                            return Redirect(PayingResult.FailureMessage);

                        }
                    }
                    break;
            }
     
            return RedirectToPage();
        }


        public async Task<OperationResult> PayingAction(double Price, string Name, int Quantity, int productId, PremiumPackage packageType, PostType postType)
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
                                    Value = Price.ToString(),
                                    Breakdown = new Breakdown()
                                    {
                                        Item_total = new Amount()
                                        {
                                            Currency_code= "EUR",
                                            Value = Price.ToString()
                                        }
                                    }
                                },
                                Description = "Content Boosting",
                                Items = new List<Items>
                                {
                                    new Items()
                                    {
                                        Name = Name,
                                        Unit_amount = new Amount()
                                        {
                                            Currency_code = "EUR",
                                            Value = Price.ToString()
                                        },
                                        Quantity = Quantity.ToString()
                                    }
                                }
                            }
                        },
                    Application_context = new ApplicationContext()
                    {
                        Brand_name = "GrandJob",
                        Landing_page = "NO_PREFERENCE",
                        User_action = "PAY_NOW", //Accion para que paypal muestre el monto de pago
                        Return_url = "https://localhost:44360/identity/checkout/Index?handler=Callback&"+$"package={(int)packageType}&" + $"productId={productId}&" + $"postType={(int)postType}",// cuando se aprovo la solicitud del cobro
                        Cancel_url = "https://localhost:44360/identity/checkout/Cancel"// cuando cancela la operacion
                    }
                };
                var orderResult = await _paypalClient.CreateOrder(order, accesstoken);
                if (orderResult != null)
                {
                    return OperationResult.SuccessResult(orderResult.Links[1].Href);
                }
               
            }

            return OperationResult.FailureResult("~/Identity/Checkout/Cancel");
        }

        public async Task<IActionResult> OnGetCallbackAsync(string token, int package, int productId, int postType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            switch (EnumHelper.GetEnumValue<PremiumPackage>(package))
            {
                case PremiumPackage.Bronze:
                    BoostedPost = true;
                    break;
                case PremiumPackage.Silver:
                    BoostedPost = true;
                    RefreshCount = 3;
                    break;
                case PremiumPackage.Gold:                
                    BoostedPost = true;
                    RefreshCount = 3;
                    BoostedPostInHome = true;
                    AutoSuggestion = true;
                    break;
            };


            var accesstoken = await _paypalClient.GetToken(_clientID, _clientSecret);
            if (accesstoken != null)
            {
                var result = await _paypalClient.CaptureOrder(accesstoken, token);
                if (result)
                {
                    var posttype = EnumHelper.GetEnumValue<PostType>(postType);
                    switch (EnumHelper.GetEnumValue<PostType>(postType))
                    {
                        case PostType.Job:
                            {
                                var job = await _spJobService.GetByIdAsync<Jobs>(productId);

                                if (job is not null)
                                {
                                    var packageCreate = await _promotionService.Create(new CreatePromotion
                                    {
                                        UserId = user.Id,
                                        productId = job.Id,
                                        premiumPackage = EnumHelper.GetEnumValue<PremiumPackage>(package),
                                        PostType = PostType.Job,
                                        StartTime = DateTime.Now,
                                        EndTime = DateTime.Now.AddDays(7.0),
                                        BoostedPost = BoostedPost,
                                        BoostedPostInHome = BoostedPostInHome,
                                        AutoSuggestion = AutoSuggestion,
                                        RefreshCount = RefreshCount
                                    }, user);

                                    if (packageCreate.Success)
                                    {
                                        var update = await _spJobService.CRUD(new { PremiumPackage = EnumHelper.GetEnumValue<PremiumPackage>(package), Id = job.Id }, JobCrudActionEnum.UpdatePromotion, false, null, null);
                                        if (update.Success)
                                            return RedirectToPage("./Success", new { token = token });
                                    }
                                }
                            }
                            break;
                        case PostType.Contestant:
                            {
                                var contestant = await _contestantsService.GetByIdAsync(productId);

                                if (contestant is not null)
                                {
                                   var packageCreate = await _promotionService.Create(new CreatePromotion
                                    {
                                        UserId = user.Id,
                                        productId = contestant.Id,
                                        premiumPackage = EnumHelper.GetEnumValue<PremiumPackage>(package),
                                        PostType = PostType.Contestant,
                                        StartTime = DateTime.Now,
                                        EndTime = DateTime.Now.AddDays(7.0),
                                        BoostedPost = BoostedPost,
                                        BoostedPostInHome = BoostedPostInHome,
                                        AutoSuggestion = AutoSuggestion,
                                        RefreshCount = RefreshCount
                                   }, user);

                                    if (packageCreate.Success)
                                    {
                                        var update = await _contestantsService.UpdatePromotion(EnumHelper.GetEnumValue<PremiumPackage>(package), contestant);
                                        if(update.Success)
                                            return RedirectToPage("./Success", new { token = token });
                                    }
                                }
                            }
                            break;
                    }
                    //return Redirect("~/Identity/Checkout/Success");
                }
            }

            return Redirect("~/Identity/Checkout/Cancel");

        }
    }
}