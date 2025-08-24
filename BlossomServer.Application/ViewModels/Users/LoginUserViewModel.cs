namespace BlossomServer.Application.ViewModels.Users
{
    public sealed record LoginUserViewModel(string Identifier, string Password);

    public sealed class LoginResponse
    {
        public Token AccessToken { get; set; } = new();
        public Token RefreshToken { get; set; } = new();

        public static LoginResponse Empty() => new();
    }

    public sealed class Token
    {
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Unix epoch seconds when token expired
        /// </summary>
        public long Exp { get; set; }
    }
}
