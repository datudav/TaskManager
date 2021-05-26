using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Requests
{
	public class ToDoListGet
	{
		[Required]
		public DateTime CreatedDateStart { get; set; }

		[Required]
		public DateTime CreatedDateEnd { get; set; }

		public string Keyword { get; set; }
	}
}
