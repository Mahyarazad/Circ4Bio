using System;
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
using AM.Application.Contracts.User;

namespace AM.Application
{
    public class ListingApplication : IListingApplication
    {
        private readonly IListingRepository _listingRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserApplication _userApplication;
        private readonly IFileUploader _fileUploader;

        public ListingApplication(IListingRepository listingRepository,
            IHttpContextAccessor contextAccessor,
            IUserApplication userApplication,
            IFileUploader fileUploader)
        {
            _listingRepository = listingRepository;
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateListing command)
        {
            var result = new OperationResult();
            var issuer = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);
            var roleId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);
            // var typeofListing = _userApplication.GetUsertypes()
            //     .FirstOrDefault(x => x.TypeId == roleId).TypeName;
            var typeofListing = "admin";
            command.ImageString = _fileUploader
                    .Uploader(command.Image, $"Listing_Images/${typeofListing}", Guid.NewGuid().ToString());
            if (command.Image == null)
                command.ImageString = "listing-default.png";

            var listing = new Listing(command.Name, typeofListing, command.Description, command.ImageString,
                command.DeliveryMethod, command.Unit, command.UnitPrice, command.Amount, command.Status,
                issuer);
            _listingRepository.Create(listing);
            _listingRepository.SaveChanges();

            return result.Succeeded();
        }

        public OperationResult Edit(EditListing command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == command.Id))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var roleId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);
            var typeofListing = _userApplication.GetUsertypes()
                .FirstOrDefault(x => x.TypeId == roleId).TypeName;

            var imageFileName = _fileUploader.Uploader(command.Image, $"Listing_Images/${typeofListing}", command.Name);

            var target = _listingRepository.Get(command.Id);
            target.Edit(command.Name, command.Description, imageFileName,
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