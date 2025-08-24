namespace BlossomServer.SharedKernel.Models
{
    public sealed class OAuthProviderOptions
    {
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public string AuthorizationEndpoint { get; set; } = "";
        public string TokenEndpoint { get; set; } = "";
        public string UserInfoEndpoint { get; set; } = "";
        public string RedirectUri { get; set; } = "";
        public string Scope { get; set; } = "";
        public string Provider { get; set; } = "";
    }
}
