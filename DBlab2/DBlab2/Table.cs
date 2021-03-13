using System.ComponentModel.DataAnnotations;

namespace DBlab2 {
    public class Table {
        public int Id { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Name { get; set; }
    }
}