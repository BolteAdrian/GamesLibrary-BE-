namespace GamesLibrary.Utils.Constants
{
    public class ResponseConstants
    {
        public const string UNKNOWN = "Unknown Error!";
        public const string UNREACHABLE = "Resource Unavailable!";

        public static class PURCHASE
        {
            public const string NOT_FOUND = "Purchase not found!";
            public const string NOT_SAVED = "Could not save purchase!";
            public const string ERROR_UPDATING = "Could not update purchase!";
            public const string ERROR_DELETING = "Could not delete purchase!";
        }

        public static class GAME
        {
            public const string NOT_FOUND = "Game not found!";
            public const string NOT_SAVED = "Could not save game!";
            public const string ERROR_UPDATING = "Could not update game!";
            public const string ERROR_DELETING = "Could not delete game!";
        }

        public static class USER
        {
            public const string NOT_SAVED = "Could Not Register User!";
            public const string ERROR_REMOVING_USER = "Could Not Remove User From DB!";
            public const string EMAIL_TAKEN = "That Email Is Taken!";
            public const string EMAIL_ERROR = "Could Not Change User email!";
            public const string PASSWORD_ERROR = "Could Not Change User password!";
            public const string NEW_PASSWORD_ERROR = "The password must have:\nAt least 8 characters (required for your Muhlenberg password) — the more characters, the better.\nA mixture of both uppercase and lowercase letters.\nA mixture of letters and numbers.\nInclusion of at least one special character, e.g., ! @ # ?";
            public const string NOT_FOUND = "User with ID {0} not found.";
        }

        public static class EMAIL
        {
            public const string CONFIG_NULL = "Email configuration is null!";
            public const string EMAIL_NULL_OR_EMPTY = "Email address cannot be null or empty!";
            public const string ERROR_SENDING = "Could Not Send Email!";
        }
    }
}
