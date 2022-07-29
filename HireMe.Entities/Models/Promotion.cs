using Ardalis.GuardClauses;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using System;

namespace HireMe.Entities.Models
{
    public class Promotion : BaseModel
    {
        public int productId { get; set; }
        public string UserId { get; set; }
        public PackageType Type { get; set; }
        public PremiumPackage premiumPackage { get; set; }
        public PostType PostType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime  { get; set; }

        public bool BoostedPost { get; set; }
        public bool BoostedPostInHome { get; set; }
        public int RefreshCount { get; set; }
        public bool AutoSuggestion { get; set; }

        public void Update(CreatePromotion viewModel,  User user)
        {
           // Id = viewModel.Id;

            Guard.Against.NullOrEmpty(viewModel.UserId, nameof(viewModel.UserId));
            UserId = user?.Id;

            Guard.Against.NegativeOrZero(viewModel.RefreshCount, nameof(viewModel.RefreshCount));
            productId = viewModel.productId;

            Guard.Against.EnumOutOfRange(viewModel.Type, nameof(viewModel.Type));
            Type = viewModel.Type;

            Guard.Against.EnumOutOfRange(viewModel.premiumPackage, nameof(viewModel.premiumPackage));
            premiumPackage = viewModel.premiumPackage;

            Guard.Against.EnumOutOfRange(viewModel.PostType, nameof(viewModel.PostType));
            PostType = viewModel.PostType;

            StartTime = viewModel.StartTime;
            EndTime = viewModel.EndTime;

            BoostedPost = viewModel.BoostedPost;
            BoostedPostInHome = viewModel.BoostedPostInHome;

            Guard.Against.Negative(viewModel.RefreshCount, nameof(viewModel.RefreshCount));
            RefreshCount = viewModel.RefreshCount;

            AutoSuggestion = viewModel.AutoSuggestion;
        }
    }
}