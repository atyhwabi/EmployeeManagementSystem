
using BaseLibrary.DTOs;
using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
	public class Employee : AuditDTO
	{

		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
	    public DateTime EmploymentDate { get; set; }
		public string Fullname { get; set; }
		public  string JobTitle { get; set; }
		public string Photo { get; set; }
		public string FileNumber { get; set; }
		public GeneralDepartment? GeneralDepartment { get; set; }
		public int GeneralDepartmentId { get; set; }
		public Department? Department { get; set; }	
		public int DepartmentId { get; set; }
		public Town? Town { get; set; }
		public int TownId { get; set; }
	}
}