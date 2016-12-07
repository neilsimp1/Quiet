using CommandLine;

namespace Quiet.Options {
	
	[Verb("connect", HelpText = "Connect to a ssh profile")]
	class ConnectOptions {

		[Option('n', "name", HelpText = "Specify a profile to connect to")]
		public string Name { get; set; }

		[Option('p', "print", HelpText = "Print out generated ssh command, do not connect")]
		public bool Print { get; set; }

		[ValueAttribute(0)]
		public string UnboundName {
			set {
				this.Name = value;
			}
		}

	}
	
}