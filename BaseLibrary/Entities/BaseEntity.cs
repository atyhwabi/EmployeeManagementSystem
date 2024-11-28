
using System.Text.Json.Serialization;
using BaseLibrary.DTOs;
using BaseLibrary.Entities;


namespace BaseLibrary.Entities
{
    public class BaseEntity : AuditDTO
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
