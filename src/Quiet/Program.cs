using System;
using System.Diagnostics;
using CommandLine;

namespace Quiet {
	public class Program {

		static ProfileManager pm = new ProfileManager();

		public static int Main(string[] args) {
			var result = CommandLine.Parser.Default.ParseArguments<ConnectOptions, AddOptions, ListOptions>(args)
			.MapResult(
				(ConnectOptions options) => ExecuteConnect(options)
				, (AddOptions options) => ExecuteAdd(options)
				, (ListOptions options) => ExecuteList(options)
				, errs => 1
			);

			Console.ReadLine();
			return result;
		}


		private static int ExecuteConnect(ConnectOptions options) {
			var profile = pm.GetProfile(options.Profile);

			Process proc = new Process {
				StartInfo = new ProcessStartInfo {
					FileName = "ssh"
					, Arguments = $"{profile.Username}@{profile.Hostname}"
					, UseShellExecute = false
					, RedirectStandardOutput = true
					, CreateNoWindow = false
				}
			};

			proc.Start();
			//// NEED TO DO SOMETHING HERE. PROGRAM IDEALLY SHOULD EXIT, THEN RUN THIS COMMAND.
			//// IF CANNOT DO THIS, IT SHOULD WAIT while() until IT EXITS, THEN RETURN EXIT CODE 
			//while(!proc.StandardOutput.EndOfStream) {
			//	string line = proc.StandardOutput.ReadLine();
			//	// do something with line
			//}

			return 0;
		}

		private static int ExecuteAdd(AddOptions options) {
			throw new NotImplementedException();
		}

		private static int ExecuteList(ListOptions options) {
			var profiles = options.Group != null ? pm.FilterByGroup(options.Group) : pm.Profiles;

			foreach(var profile in profiles) Console.WriteLine(profile.ToString());

			return 0;
		}


		[Verb("connect", HelpText = "Connect to a ssh profile")]
		class ConnectOptions {

			[Option('p', "profile", HelpText = "Specify a profile to connect to")]
			public string Profile { get; set; }

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
