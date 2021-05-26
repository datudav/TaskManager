using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models.Responses
{
	[Table("Task", Schema = "dbo")]
	public class Task
	{
		[Key]
		public long TaskId { get; set; }

		public long UserId { get; set; }

		public long ListId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Rank { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }
	}
}
