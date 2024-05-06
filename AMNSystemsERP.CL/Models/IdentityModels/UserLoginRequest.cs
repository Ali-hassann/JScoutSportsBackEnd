using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class UserLoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
