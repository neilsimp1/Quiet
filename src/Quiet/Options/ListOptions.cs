using CommandLine;

namespace Quiet.Options {
	
	[Verb("list", HelpText = "List profiles")]
	class ListOptions {

		//// Save for telnet support?
		//[Option('t', "type", HelpText = "Specify a profile type to list (`ssh`, `telnet`)")]
		//public string Type { get; set; }

		[Option('g', "group", HelpText = "Specify a profile group to list")]
		public string Group { get; set; }

	}
	
}