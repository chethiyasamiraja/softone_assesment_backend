namespace AuthBackend.Modal
{ 
    public class LoginResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public int ResponseCode { get; set; }
    }
}
