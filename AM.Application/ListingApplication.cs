using System.Collections.Generic;
using System.Linq;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Domain;
using AM.Domain.ListingAggregate;
using AM.Domain.Supplied.PurchasedAggregate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AM.Application
{
    public class ListingApplication : IListingApplication
    {
        private readonly IListingRepository _listingRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public ListingApplication(IListingRepository listingRepository,
            IHttpContextAccessor contextAccessor)
        {
            _listingRepository = listingRepository;
            _contextAccessor = contextAccessor;
        }

        public OperationResult Create(CreateListing command)
        {
            var result = new OperationResult();
            var issuer = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);
            var typeofListing = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);
            var listing = new Listing(command.Name, typeofListing, command.Description, command.Image,
                command.DeliveryMethod, command.Unit, command.UnitPrice, command.Amount, command.Status,
                issuer, new List<Deal>(), new List<PurchasedItem>(), new List<SuppliedItem>());
            _listingRepository.Create(listing);
            _listingRepository.SaveChanges();

            return result.Succeeded();

        }

        public OperationResult Edit(EditListing command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == command.Id))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var target = _listingRepository.Get(command.Id);
            target.Edit(command.Name, command.Description, command.Image,
                command.DeliveryMethod, command.Unit, command.UnitPrice, command.Amount);
            _listingRepository.SaveChanges();

            return result.Succeeded();
        }

        public List<ListingViewModel> GetUserListing(long id)
        {
            return _listingRepository.GetUserListing(id);
        }

        public EditListing GetEditListing(long listingId)
        {
            return _listingRepository.GetListingDetail(listingId);
        }
    }
}