using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models.Entities
{
    public class UserLR
    {
        public Guid ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
