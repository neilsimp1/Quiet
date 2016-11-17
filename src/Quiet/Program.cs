using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using Newtonsoft.Json;

namespace Quiet {
	public class Program {

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
			var profiles = GetProfiles(options.Group);
			foreach(var profile in profiles) Console.WriteLine(profile.ToString());

			return 0;
		}

		private static IEnumerable<Profile> GetProfiles(string group) {
			var json = File.ReadAllText("data/profiles.json");
			var profiles = JsonConvert.DeserializeObject<IEnumerable<Profile>>(json);

			if(group != null) {
				profiles = profiles.Where(profile => {
					var isInGroup = profile.Group == group;
					profile.Group = null;
					return isInGroup;
				});
			}

			return profiles;
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
