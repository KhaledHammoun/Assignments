using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Job
    {
        [Key]
        public int Id { set; get; }
        public string JobTitle { get; set; }
        public int Salary { get; set; }
    }
}