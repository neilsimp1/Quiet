using CommandLine;

namespace Quiet.Options {
	class AddUpdateOptions {

		[Option('n', "name", HelpText = "Specify a profile name")]
		public string Name { get; set; }
		
		[Option('u', "username", HelpText = "Specify a username to use when connecting")]
		public string Username { get; set; }

		[Option('h', "host", HelpText = "Specify a host to connect to")]
		public string Hostname { get; set; }

		[Option('p', "port", HelpText = "Specify a default port to use (if using somethign other than 22)")]
		public string Port { get; set; }

		[Option('g', "group", Default = "default", HelpText = "Specify a profile group to list")]
		public string Group { get; set; }

		[Option('i', "interactive", HelpText = "Interactive prompt to add a profile")]
		public bool Interactive { get; set; }

	}
}