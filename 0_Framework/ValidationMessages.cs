namespace _0_Framework
{
    public class ValidationMessages
    {
        public const string IsRequired = "This field is manditory";
        public const string SizeError2M = "The file cannot be larger than 2 MB";
        public const string SizeError1M = "The file cannot be larger than 1 MB";
        public const string InvalidFileFormat = "Please provide JPEG, JPG or PNG file format";
        public const string InvalidUploadFileFormat = "We only accept PDF file format";
        public const string EmailRequired = "Please enter a E-mail address";
        public const string DeliveryLocation = "Address cannot be empty";
        public const string DeliveryLocationCity = "City cannot be empty";
        public const string DeliveryLocationCountry = "Country cannot be empty";
        public const string DeliveryLocationName = "Please provide a name for this address";
        public const string Password = "Please enter password";
        public const string ConfirmPassword = "Please enter confirm password";
        public const string StrongerPassword8Character = "Password must be at least 8 characters long, but no more than 32";
        public const string StrongerPasswordDigit = "Password must contain at least one digit";
        public const string StrongerPasswordUpperCaseLetter = "Password must contain at least uppercase letter";
        public const string StrongerPassword = "Password must have at least one numeric, one uppercase, and lowercase letter";
        public const string SelectUserType = "Please select the user type";
        public const string PasswordNotMatch = "Password and confirm password do not match";
        public const string MaxChar500 = "Description should be less between 50 words and 100 words";
        public const string MaxChar2000 = "Description should be less between 250 words and 500 words";
        public const string MaxChar3000 = "Description should be less between 500 words and 750 words";
        public const string MaxCharAddress = "Address should be less between 10 words and 13 words";
        public const string MaxCharName = "Name should be less than 20 characters";
        public const string MaxCharCurrency = "Currency should be less than three charectors";
        public const string UnitError = "Symbol for units should be written in letters";
        public const string FieldRequired = "This field is required";

        //Deals
        public const string DeliveryMethod = "Delivery method cannot be empty!";
        public const string Currency = "You have to select at least one currency";

    }
}