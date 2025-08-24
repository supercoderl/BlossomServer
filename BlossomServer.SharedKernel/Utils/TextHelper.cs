namespace BlossomServer.SharedKernel.Utils
{
    public static class TextHelper
    {
        public static string NomalizeGmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return email;

            var array = email.Split("@");

            if (array.Length != 2) return email;

            return $"{array[0].Replace(".", "")}@{array[1]}";
        }
    }
}
