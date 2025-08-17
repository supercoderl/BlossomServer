using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Errors
{
    public static class DomainErrorCodes
    {
        public static class User
        {
            // User Validation
            public const string EmptyId = "USER_EMPTY_ID";
            public const string EmptyUsername = "USER_EMPTY_USERNAME";
            public const string EmptyFirstName = "USER_EMPTY_FIRST_NAME";
            public const string EmptyLastName = "USER_EMPTY_LAST_NAME";
            public const string EmptyPhoneNumber = "USER_EMPTY_PHONE_NUMBER";
            public const string EmailExceedsMaxLength = "USER_EMAIL_EXCEEDS_MAX_LENGTH";
            public const string FirstNameExceedsMaxLength = "USER_FIRST_NAME_EXCEEDS_MAX_LENGTH";
            public const string LastNameExceedsMaxLength = "USER_LAST_NAME_EXCEEDS_MAX_LENGTH";
            public const string InvalidEmail = "USER_INVALID_EMAIL";
            public const string InvalidIdentifier = "USER_INVALID_IDENTIFIER";
            public const string InvalidRole = "USER_INVALID_ROLE";

            // User Password Validation
            public const string EmptyPassword = "USER_PASSWORD_MAY_NOT_BE_EMPTY";
            public const string ShortPassword = "USER_PASSWORD_MAY_NOT_BE_SHORTER_THAN_6_CHARACTERS";
            public const string LongPassword = "USER_PASSWORD_MAY_NOT_BE_LONGER_THAN_50_CHARACTERS";
            public const string UppercaseLetterPassword = "USER_PASSWORD_MUST_CONTAIN_A_UPPERCASE_LETTER";
            public const string LowercaseLetterPassword = "USER_PASSWORD_MUST_CONTAIN_A_LOWERCASE_LETTER";
            public const string NumberPassword = "USER_PASSWORD_MUST_CONTAIN_A_NUMBER";
            public const string SpecialCharPassword = "USER_PASSWORD_MUST_CONTAIN_A_SPECIAL_CHARACTER";

            // General
            public const string AlreadyExists = "USER_ALREADY_EXISTS";
            public const string PasswordIncorrect = "USER_PASSWORD_INCORRECT";
        }

        public static class RefreshToken
        {
            // Refresh Token Validation
            public const string EmptyId = "REFRESH_TOKEN_EMPTY_ID";
            public const string EmptyUserId = "REFRESH_TOKEN_EMPTY_USER_ID";
            public const string EmptyToken = "REFESH_TOKEN_EMPTY_TOKEN";
        }

        public static class ServiceImage
        {
            // Service Image Validation
            public const string EmptyId = "SERVICE_IMAGE_EMPTY_ID";
            public const string EmptyName = "SERVICE_IMAGE_EMPTY_NAME";
            public const string EmptyConnectionId = "SERVICE_IMAGE_EMPTY_CONNECTION_ID";
        }

        public static class ServiceOption
        {
            // Service Option Validation
            public const string EmptyId = "SERVICE_OPTION_EMPTY_ID";
            public const string EmptyVariantName = "SERVICE_OPTION_EMPTY_NAME";
        }

        public static class Booking
        {
            // Booking Validation
            public const string EmptyId = "BOOKING_EMPTY_ID";
        }

        public static class Subcriber
        {
            // Subcriber Validation
            public const string EmptyEmail= "SUBCRIBER_EMPTY_EMAIL";
            public const string InvalidEmail = "SUBCRIBER_INVALID_EMAIL";
        }

        public static class Contact
        {
            // Contact Validation
            public const string EmptyName = "CONTACT_EMPTY_NAME";
            public const string EmptyEmail = "CONTACT_EMPTY_EMAIL";
            public const string EmptyMessage = "CONTACT_EMPTY_MESSAGE";
        }

        public static class ContactResponse
        {
            // Contact Response Validation
            public const string EmptyResponseText = "CONTACT_RESPONSE_EMPTY_RESPONSE_TEXT";
        }

        public static class Blog
        {
            // Blog Validation
            public const string EmptyId = "BLOG_EMPTY_ID";
        }
    }
}
