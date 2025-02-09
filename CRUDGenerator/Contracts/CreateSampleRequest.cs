using System.ComponentModel.DataAnnotations;
namespace CRUDGenerator.Contracts
{
    public class CreateSampleRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }= string.Empty;
        [StringLength(500)]
        public string Description { get; set; }= string.Empty;
        [Required]
        public DateTime DueDate { get; set; }
        [Range(1, 5)]
        public int Priority { get; set; }
    }
}