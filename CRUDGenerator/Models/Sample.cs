using System.ComponentModel.DataAnnotations;

namespace CRUDGenerator.Models
{
    public class Sample
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Sample()
        {
            IsComplete = false;
        }
    }
}