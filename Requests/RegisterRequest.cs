﻿namespace api_lib.Requests
{
    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
