﻿namespace _0_Framework
{
    public class ApplicationMessage
    {
        //Accounts Messages
        public const string AccountDeactivated = "Your account has been deactivated.";
        public const string AccountDeactivatedFailed = "You have an active deal! You can deactivate your account after finishing active deals.";
        public const string RecordNotFound = "Cannot find the current item";
        public const string RecordExists = "Another record with the same value exists in database";
        public const string PasswordsAreNotMatched = "Passwords are not macth, Please try again";
        public const string UserNotExists = "This user does not exist in database";
        public const string WrongPassword = "The input password is not correct";
        public const string UserNotActive = "You've not activated your account, please check your emial for activation link";
        public const string SuccessfulRegister = "Activation link has been sent to your email, Please activate your account.";
        public const string SuccessfulActivation = "Your account has been activated. You can login to your account now.";
        public const string AccountVerification = "Please Verify Your Account";
        public const string SuccessLogin = "Successfully logged in";
        public const string SomethingWentWrong = "Something went wrong, please try again!";
        public const string ActivationUrlError = "Activation link is not valid, please check your email for the valid link.";
        public const string ResetPasswordGuidance = "Reset password link has been sent to your email, please check your email.";
        public const string ResetPasswordLinkExipre = "Reset password link has been expired, please try again.";
        public const string ResetPasswordSuccess = "Your password has been changed successfully.";
        public const string ResetPasswordLinkIsInvalid = "Reset password link is not valid.";
        public const string ContactUsSuccess = "Your message has been successfully sent, thanks for your inquery.";
        public const string SubmitRequiredInfo = "Please provide the required information to register, post requests and exchange messages with other users. Your application is completed upon approval of your application you will be able to add your listing.";
        public const string SystemMessage = "System Message";

        //Listing Messages
        public const string ListingNewItemListed = "You added a new item in the listings";
        public const string ListingAdmin = "Site Admin has been listed a new Item in the listing";
        public const string ListingPlantOwner = "Plant owner has been listed a new Item in the listing";
        public const string ListingTechnologyProvider = "Technology Provider has been listed a new Item in the listing";
        public const string ListingSupplierRawMaterial = "Supplier of Biomass-Raw Material has been listed a new Item in the listing";
        public const string CannotDeduct = "Cannot deduct more than the available amount";
        public const string DuplicateNegotiation = "You have an open negotiation on this item, you cannot open another negotiation.";

        //Negotiate Messaging
        public const string NewMessage = "You have a new message";
        public const string EmptyMessage = "You cannot send an empty message";
        public const string SubmitNegotiationRequest = "You have requested for a negotiation";
        public const string ReceivedNegotiation = "Negotiation has opened for your listing";
        public const string CanceledNegotiationRequest = "Negotiation Canceled!";

        //Email Title
        public const string ResetPassword = "Reset Password";
        public const string AccountActivated = "Your Account is Activated";
        public const string QuotationCreated = "Quotation Created";
        public const string ProvideInformation = "Please Provide the Required Informations";

        //Deal Title
        public const string DealsCreated = "New deal has issued";
        public const string ActiveDeal = "Your deal is active now";
        public const string ActiveDealDetail = "Please follow the link below to check your deal status.";
        public const string PaymentDone = "Your payment has been done";

        //DeliveryLocationAddress
        public const string DuplicateDeliveryLocationName = "This name has been taken, please chose another name.";

        //Welcome Message
        public const string WelcomeMessage = "Thanks for registration and welcome to Circ4Bio";


    }
}