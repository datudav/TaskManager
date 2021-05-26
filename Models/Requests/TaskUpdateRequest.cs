using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Requests
{
	public class TaskUpdateRequest
	{
		public string Name { get; set; }

		public string Description { get; set; }

		[Range(1, long.MaxValue)]
		public int? Rank { get; set; }
	}
}
