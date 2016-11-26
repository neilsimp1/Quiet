using CommandLine;

namespace Quiet.Options {
	
	[Verb("update", HelpText = "Update a profile")]
	class UpdateOptions : AddUpdateOptions {

		[Option('m', "newname", HelpText = "Specify a new profile name")]
		public string NewName { get; set; }
		
		[ValueAttribute(0)]
		public string UnboundName {
			set {
				this.Name = value;
			}
		}
		
	}
	
}