using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models.Responses
{
	[Table("ToDoList", Schema = "dbo")]
	public class ToDoList
	{

		[Key]
		public long ListId { get; set; }

		[ForeignKey("ToDoList_User_FK")]
		public long UserId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

	}
}
