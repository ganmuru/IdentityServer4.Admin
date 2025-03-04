﻿using System;

namespace IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Grant
{
	public class PersistedGrantDto
	{
		public string Key { get; set; }
		public string Type { get; set; }
		public string SubjectId { get; set; }
		public string SubjectName { get; set; }
		public string ClientId { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime? Expiration { get; set; }
		public string Data { get; set; }
	}
}