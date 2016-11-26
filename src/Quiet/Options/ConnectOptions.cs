using CommandLine;

namespace Quiet.Options {
	
	[Verb("connect", HelpText = "Connect to a ssh profile")]
	class ConnectOptions {

		[Option('n', "name", HelpText = "Specify a profile to connect to")]
		public string Name { get; set; }

	}
	
}