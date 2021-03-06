﻿using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using Quiet.Options;

namespace Quiet {
	public class Program {
		
		static ProfileManager pm = new ProfileManager();

		public static int Main(string[] args) {
			Startup();
			
			int result;

			if(args.Length == 1 && !IsVerb(args[0])){
				result = CommandLine.Parser.Default.ParseArguments<ConnectOptions>(args)
					.MapResult((ConnectOptions options) => ExecuteConnect(options), errs => 1);
				return result;
			}
			else{
				result = CommandLine.Parser.Default.ParseArguments<ConnectOptions, ListOptions, AddOptions
					, UpdateOptions, DeleteOptions>(args)
				.MapResult(
					(ConnectOptions options) => ExecuteConnect(options)
					, (AddOptions options) => options.Interactive ? ExecuteAddInteractive() : ExecuteAdd(options)
					, (UpdateOptions options) => options.Interactive ? ExecuteUpdateInteractive(options.Name) : ExecuteUpdate(options)
					, (DeleteOptions options) => ExecuteDelete(options)
					, (ListOptions options) => ExecuteList(options)
					, errs => 1
				);
			}

			return result;
		}

		private static void Startup() {
			var profilePath = ProfileManager.GetProfilesPath();
			if (!File.Exists(profilePath)){
				Directory.CreateDirectory(profilePath.Replace("/profiles.json", ""));
				File.WriteAllText(profilePath, "[]");
			}
		}

		private static int ExecuteConnect(ConnectOptions options) {
			var profile = pm.GetProfile(options.Name);
			if(profile == null){
				Console.WriteLine($"Profile with name `{options.Name}` not found");
				return -1;
			}

			var args = BuildCommand(profile);

			Process proc = new Process {
				StartInfo = new ProcessStartInfo {
					FileName = "ssh"
					, Arguments = args
					, UseShellExecute = false
					, RedirectStandardOutput = false
					, CreateNoWindow = false
				}
			};

			if(options.Print){
				Console.WriteLine($"ssh {args}");
			}
			else{
				proc.Start();
				proc.WaitForExit();
			}
			
			return 0;
		}

		private static int ExecuteList(ListOptions options) {
			var profiles = options.Group != null ? pm.FilterByGroup(options.Group) : pm.Profiles;

			foreach(var profile in profiles) Console.WriteLine(profile.ToString());

			return 0;
		}

		private static int ExecuteAdd(AddOptions options) {
			if(pm.GetProfile(options.Name) != null){
				Console.WriteLine($"Profile with name `{options.Name}` already set");
				return -1;
			}

			var profile = new Profile {
				Name = options.Name
				, Group = options.Group
				, Hostname = options.Hostname
				, Username = options.Username
				, Port = options.Port
			};

			pm.AddProfile(profile);

			return 0;
		}

		private static int ExecuteAddInteractive() {
			string name;
			while(true){
				Console.Write("Name: ");
				name = Console.ReadLine();
				if(pm.GetProfile(name) != null){
					Console.WriteLine($"Profile with name `{name}` already set");
				}
				else break;
			}

			Console.Write("Group (default): ");
			var _group = Console.ReadLine();
			var group = _group == string.Empty ? "default" : _group;

			Console.Write("Hostname: ");
			var hostname = Console.ReadLine();
			
			Console.Write("Username: ");
			var username = Console.ReadLine();

			Console.Write("Port (22): ");
			var _port = Console.ReadLine();
			var port = _port == string.Empty ? null : _port;

			var profile = new Profile {
				Name = name
				, Group = group
				, Hostname = hostname
				, Username = username
				, Port = port
			};

			pm.AddProfile(profile);

			return 0;
		}

		private static int ExecuteUpdate(UpdateOptions options) {
			if(pm.GetProfile(options.Name) == null){
				Console.WriteLine($"Profile with name `{options.Name}` not found");
				return -1;
			}

			var profile = new Profile {
				Name = options.NewName == null ? options.Name : options.NewName
				, Group = options.Group
				, Hostname = options.Hostname
				, Username = options.Username
				, Port = options.Port
			};

			pm.UpdateProfile(profile, options.Name);

			return 0;
		}

		private static int ExecuteUpdateInteractive(string profileName) {
			var oldProfile = pm.GetProfile(profileName);
			if(oldProfile == null){
				Console.WriteLine($"Profile with name `{profileName}` not found");
				return -1;
			}

			string _name;
			while(true){
				Console.Write($"Name ({oldProfile.Name}): ");
				_name = Console.ReadLine();
				if(_name != profileName && pm.GetProfile(_name) != null){
					Console.WriteLine($"Profile with name `{_name}` already set");
				}
				else break;
			}
			var name = _name == string.Empty ? oldProfile.Name : _name;

			Console.Write($"Group ({oldProfile.Group}): ");
			var _group = Console.ReadLine();
			var group = _group == string.Empty ? "default" : _group;
			
			Console.Write($"Hostname ({oldProfile.Hostname}): ");
			var _hostname = Console.ReadLine();
			var hostname = _hostname == string.Empty ? oldProfile.Hostname : _hostname;
			
			Console.Write($"Username ({oldProfile.Username}): ");
			var _username = Console.ReadLine();
			var username = _username == string.Empty ? oldProfile.Username : _username;

			var oldPort = oldProfile.Port != null ? oldProfile.Port : "22";
			Console.Write($"Port ({oldPort}): ");
			var _port = Console.ReadLine();
			var port = _port == string.Empty ? null : _port;

			var profile = new Profile {
				Name = name
				, Group = group
				, Hostname = hostname
				, Username = username
				, Port = port
			};

			pm.UpdateProfile(profile, oldProfile.Name);

			return 0;
		}

		private static int ExecuteDelete(DeleteOptions options) {
			if(pm.GetProfile(options.Name) == null){
				Console.WriteLine($"Profile with name `{options.Name}` not found");
				return -1;
			}
			
			pm.DeleteProfile(options.Name);

			return 0;
		}

		private static string BuildCommand(Profile profile) {
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			
			if(profile.Username != null)sb.Append($"{profile.Username}@");
			sb.Append($"{profile.Hostname}");
			if(profile.Port != null) sb.Append($" -p {profile.Port}");

			return sb.ToString();
		}

		private static bool IsVerb(string arg) {
			return (arg == "connect"
					|| arg == "list"
					|| arg == "add"
					|| arg == "update"
					|| arg == "delete"); 
		}

	}
}
