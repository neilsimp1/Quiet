using CommandLine;

namespace Quiet.Options {
	
	[Verb("delete", HelpText = "Delete ssh profile")]
	class DeleteOptions {

		[Option('n', "name", HelpText = "Specify a profile to delete")]
		public string Name { get; set; }

	}
	
}