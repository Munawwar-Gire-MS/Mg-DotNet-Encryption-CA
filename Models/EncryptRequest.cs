namespace MyEncryptionApp.Models // Ensure this matches your project namespace
{
    public class EncryptRequest
    {
        public string Input { get; set; } = string.Empty; // Initialize to avoid nullability warning
    }
}
