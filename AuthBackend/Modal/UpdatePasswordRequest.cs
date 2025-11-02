namespace AuthBackend.Modal
{

    public class UpdatePasswordRequest
    {
        public string? UserId { get; set; }
        public string? NewPassword { get; set; }
    }
}
