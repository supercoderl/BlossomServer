using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Errors
{
    public static class ErrorCodes
    {
        public const string CommitFailed = "COMMIT_FAILED";
        public const string ObjectNotFound = "OBJECT_NOT_FOUND";
        public const string InsufficientPermissions = "UNAUTHORIZED";
        public const string InvalidPassword = "INVALID_PASSWORD";
        public const string UploadFailed = "UPLOAD_FAILED";
        public const string ExpiredToken = "EXPIRED_TOKEN";
        public const string ObjectAlreadyExists = "OBJECT_ALREADY_EXISTS";
    }
}
