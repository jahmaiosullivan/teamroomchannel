using System.ComponentModel.DataAnnotations;
using HobbyClue.Common.Attributes;

namespace HobbyClue.Data.Models
{
    public class Question : ModelBase
    {
        [Required]
        [StringLength(1024, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Title { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string Body { get; set; }
        
        [Required]
        public long ProductId { get; set; }
        public int Views { get; set; }
        public QuestionState State { get; set; }
    }
}
