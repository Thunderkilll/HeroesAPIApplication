using System.ComponentModel.DataAnnotations;

namespace HeroesAPIApp.Models
{
    public class Hero
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
