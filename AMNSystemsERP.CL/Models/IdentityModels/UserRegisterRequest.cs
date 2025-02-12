﻿using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class UserRegisterRequest 
    {
        public long OrganizationRequestId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }  
    }
}
