using System;
using CommandLine;

namespace Quiet {
	public class Program {

		static ProfileManager pm = new ProfileManager();

		public static int Main(string[] args) {
			var result = CommandLine.Parser.Default.ParseArguments<AddOptions, ListOptions>(args)
			.MapResult(
				(AddOptions options) => ExecuteAdd(options)
				, (ListOptions options) => ExecuteList(options)
				, errs => 1
			);

			Console.ReadLine();
			return result;
		}


		private static int ExecuteAdd(AddOptions options) {
			throw new NotImplementedException();
		}

		private static int ExecuteList(ListOptions options) {
			var profiles = options.Group != null ? pm.FilterByGroup(options.Group) : pm.Profiles;

			foreach(var profile in profiles) Console.WriteLine(profile.ToString());

			return 0;
		}



		class AddOptions {

			[Option('g', "group", Default = "default", HelpText = "Specify a profile group to list")]
			public string Group { get; set; }

		}

		[Verb("list", HelpText = "List profiles")]
		class ListOptions {

			//// Save for telnet support?
			//[Option('t', "type", HelpText = "Specify a profile type to list (`ssh`, `telnet`)")]
			//public string Type { get; set; }

			[Option('g', "group", HelpText = "Specify a profile group to list")]
			public string Group { get; set; }

		}

	}
}
