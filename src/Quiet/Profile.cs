﻿using System.Text;

namespace Quiet{
    public class Profile {

		public string Name { get; set; }
		public string Hostname { get; set; }
		public string Username { get; set; }
		public string Port { get; set; }
		public string Group { get; set; }

		public override string ToString() {
			var sb = new StringBuilder();
			sb.AppendLine($"Name:\t\t{Name}");
			if(Group != null) sb.AppendLine($"Group:\t\t{Group}");
			sb.AppendLine($"Hostname:\t{Hostname}");
			if(Username != null) sb.AppendLine($"Username:\t{Username}");
			if(Port != null) sb.AppendLine($"Port:\t\t{Port}");

			return sb.ToString();
		}
	}
}
