using System.ComponentModel.DataAnnotations;

namespace CRUDGenerator.Models
{
    public class DBColums
    {
      
        public string TABLE_SCHEMA { get; set; }
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        [Key]
        public int ORDINAL_POSITION { get; set; }
        public string IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
    }
}