using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models.Requests
{
	public class ToDoListAddRequest
	{
		[Required]
		public string Name { get; set; }

		public string Description { get; set; }
	}
}
