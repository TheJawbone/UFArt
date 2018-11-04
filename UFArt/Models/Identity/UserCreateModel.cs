using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Identity
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Wprowadź nazwę użytkownka")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Wprowadź adres e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Wprowadź hasło")]
        public string Password { get; set; }
    }
}
