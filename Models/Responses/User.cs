using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models.Responses
{
	[Table("User", Schema = "dbo")]
	public class User
	{
		[Key]
		public long UserId { get; init; }

		public string UserName { get; init; }

		public DateTime LastLoginDate { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		public override string ToString()
		{
			return UserName;
		}
	}
}
