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

			return result;
		}


		private static int ExecuteConnect(ConnectOptions options) {
			var profile = pm.GetProfile(options.Profile);

			Process proc = new Process {
				StartInfo = new ProcessStartInfo {
					FileName = "ssh"
					, Arguments = buildCommand(profile)
					, UseShellExecute = false
					, RedirectStandardOutput = false
					, CreateNoWindow = false
				}
			};
			//Console.WriteLine("ssh " + buildCommand(profile));//////////////////////////////

			proc.Start();
			proc.WaitForExit();

			return 0;
		}

		private static int ExecuteAdd(AddOptions options) {
			if(pm.GetProfile(options.Name) != null){
				Console.WriteLine($"Profile with name `{options.Name}` already set");
				return -1;
			}

			var profile = new Profile {
				Name = options.Name
				, Username = options.Username
				, Hostname = options.Hostname
				, Group = options.Group
				, Port = options.Port
			};

			pm.AddProfile(profile);

			return 0;
		}

		private static int ExecuteList(ListOptions options) {
			var profiles = options.Group != null ? pm.FilterByGroup(options.Group) : pm.Profiles;

			foreach(var profile in profiles) Console.WriteLine(profile.ToString());

			return 0;
		}

		private static string buildCommand(Profile profile) {
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			
			sb.Append($"{profile.Username}@{profile.Hostname}");
			if(profile.Port != null) sb.Append($" -p {profile.Port}");

			return sb.ToString();
		} 


		[Verb("connect", HelpText = "Connect to a ssh profile")]
		class ConnectOptions {

			[Option('p', "profile", HelpText = "Specify a profile to connect to")]
			public string Profile { get; set; }

		}

		[Verb("add", HelpText = "Add a profile")]
		class AddOptions {

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
