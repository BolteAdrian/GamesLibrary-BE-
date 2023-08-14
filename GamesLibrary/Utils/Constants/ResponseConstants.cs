using static GamesLibrary.Utils.Constants.ResponseConstants;
using System.Threading.Channels;

namespace GamesLibrary.Utils.Constants
{
    public class ResponseConstants
    {
        public const string UNKNOWN = "Unknown Error!";
        public const string UNREACHABLE = "Resource Unavailable!";
        public const string INVALID_ID = "Invalid ID received!";
        public const string INVALID_DATA = "Invalid data received!";

        public static class PURCHASE
        {
            public const string NOT_FOUND = "Purchase not found!";
            public const string NOT_SAVED = "Could not save purchase!";
            public const string ERROR_UPDATING = "Could not update purchase!";
            public const string ERROR_DELETING = "Could not delete purchase!";
            public const string SUCCES_UPDATING = "Purchase updated successfully!";
            public const string SUCCES_DELETING = "Purchase deleted successfully!";
        }

        public static class GAME
        {
            public const string NOT_FOUND = "Game not found!";
            public const string NOT_SAVED = "Could not save game!";
            public const string ERROR_UPDATING = "Could not update game!";
            public const string ERROR_DELETING = "Could not delete game!";
            public const string SUCCES_UPDATING = "Game updated successfully!";
            public const string SUCCES_DELETING = "Game deleted successfully!";
        }

        public static class REVIEW
        {
            public const string NOT_FOUND = "Review not found!";
            public const string NOT_SAVED = "Could not save review!";
            public const string ERROR_UPDATING = "Could not update review!";
            public const string ERROR_DELETING = "Could not delete review!";
            public const string SUCCES_UPDATING = "Review updated successfully!";
            public const string SUCCES_DELETING = "Review deleted successfully!";
        }

        public static class USER
        {
            public const string NOT_SAVED = "Could Not Register User!";
            public const string EMAIL_TAKEN = "That Email Is Taken!";
            public const string EMAIL_ERROR = "Could Not Change User email!";
            public const string PASSWORD_ERROR = "Could Not Change User password!";
            public const string NEW_PASSWORD_ERROR = "The password must have:\nAt least 8 characters (required for your Muhlenberg password) — the more characters, the better.\nA mixture of both uppercase and lowercase letters.\nA mixture of letters and numbers.\nInclusion of at least one special character, e.g., ! @ # ?";
            public const string NOT_FOUND = "User not found!";
            public const string SUCCES_UPDATING = "User updated successfully!";
            public const string SUCCES_REGISTRATION = "Registration successful!";
            public const string SUCCES_LOGIN = "Login successful!";
            public const string ERROR_LOGIN = "Invalid login attempt!";
            public const string ERROR_REGISTER = "Invalid register attempt!";
            public const string ERROR_UPDATING_EMAIL = "Could not update the email!";
            public const string ERROR_DELETING = "Could not delete user!";
            public const string SUCCES_DELETING = "User deleted successfully!";
            public const string SUCCES_UPDATING_EMAIL = "The email was updated!";
            public const string SUCCES_UPDATING_PASSWORD = "The password was changed!";
            public const string INVALID_EMAIL = "Invalid email received!";
            public const string INVALID_TOKEN = "Invalid token!";
            public const string RESET_EMAIL_SEND = "Password reset email sent!";
            public const string CHANGE_ROLE = " User role changed successfully!";
            public const string ALREADY_HAS_THE_ROLE = "User already has the requested role!";
            public const string ERROR_CHANGE_ROLE = "Error changing user role!";
        }

        public static class EMAIL
        {
            public const string CONFIG_NULL = "Email configuration is null!";
            public const string EMAIL_NULL_OR_EMPTY = "Email address cannot be null or empty!";
            public const string ERROR_SENDING = "Could Not Send Email!";
        }
    }
}
