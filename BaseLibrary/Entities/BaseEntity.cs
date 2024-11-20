
using System.Text.Json.Serialization;
using BaseLibrary.Entities;


namespace BaseLibrary.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
