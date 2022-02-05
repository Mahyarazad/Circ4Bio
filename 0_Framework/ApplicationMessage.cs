namespace _0_Framework
{
    public class ApplicationMessage
    {
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
        public const string SubmitRequiredInfo = "Please provide the required information to post requests and messages to other clients.";
        public const string SystemMessage = "System Message";

        //Listing Messages
        public const string ListingNewItemListed = "You added a new item in the listings";
        public const string ListingPlantOwner = "Plant owner has been listed a new Item in the listing";
        public const string ListingTechnologyProvider = "Technology Provider has been listed a new Item in the listing";
        public const string ListingSupplierRawMaterial = "Supplier of Biomass-Raw Material has been listed a new Item in the listing";
        public const string CannotDeduct = "Cannot deduct more than the available amount";
        public const string DuplicateNegotiation = "You have an open negotiation on this item, you cannot open another negotiation.";

        //Negotiate Messaging
        public const string NewMessage = "You have a new message";
        public const string EmptyMessage = "You cannot send an empty message";
        public const string SubmitNegotiationRequest = "You have requested a negotiation";
        public const string ReceivedNegotiation = "Negotiation opened for your listing";
        public const string CanceledNegotiationRequest = "Negotiation Caceled!";

        //Email Title
        public const string ResetPassword = "Reset Password";
        public const string AccountActivated = "Your account is activated";
    }
}