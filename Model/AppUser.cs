﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Model
{
	public class AppUser:IdentityUser
	{
		[PersonalData]
		[Column(TypeName ="nvarchar(150)")]
		public string FullName { get; set; }
		
		[PersonalData]
		[Column(TypeName ="nvarchar(10)")]
		public string Gender { get; set; }

		[PersonalData]
		public DateOnly DOB{ get; set; }
		
		[PersonalData]
		public int? Library{ get; set; }
	}
}
