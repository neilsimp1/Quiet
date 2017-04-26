using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Quiet{
    public class ProfileManager {

		public IEnumerable<Profile> Profiles { get; set; }
		private readonly string profilesPath = GetProfilesPath();

		public ProfileManager() {
			var json = File.ReadAllText(profilesPath);
			Profiles = JsonConvert.DeserializeObject<IEnumerable<Profile>>(json);
		}

		public static string GetProfilesPath() {
			var path = "/.quiet/profiles.json";
			return System.Runtime.InteropServices.RuntimeInformation.OSDescription.Contains("Windows")
				? Environment.GetEnvironmentVariable("USERPROFILE") + path
				: Environment.GetEnvironmentVariable("HOME") + path;
		}

		public void AddProfile(Profile profile) {
			Profiles = Profiles.Concat(new[] { profile });
			var json = JsonConvert.SerializeObject(Profiles, Formatting.Indented);
			File.WriteAllText(profilesPath, json);
		}

		public void UpdateProfile(Profile profile, string profileName) {
			var oldProfile = Profiles.First(p => p.Name == profileName);
			Profiles = Profiles.Where(p => p.Name != profileName);

			var newProfile = new Profile {
				Name = profile.Name
				, Hostname = profile.Hostname != null ? profile.Hostname : oldProfile.Hostname
				, Username = profile.Username != null ? profile.Username : oldProfile.Username
				, Port = profile.Port != null ? profile.Port : oldProfile.Port
				, Group = profile.Group != null ? profile.Group : oldProfile.Group
			};

			Profiles = Profiles.Concat(new[] { newProfile });
			
			var json = JsonConvert.SerializeObject(Profiles, Formatting.Indented);
			File.WriteAllText(profilesPath, json);
		}
		
		public void DeleteProfile(string profileName) {
			Profiles = Profiles.Where(p => p.Name != profileName);
			var json = JsonConvert.SerializeObject(Profiles, Formatting.Indented);
			File.WriteAllText(profilesPath, json);
		}

		public Profile GetProfile(string profileName) {
			try{
				return Profiles.First(p => p.Name == profileName);
			}
			catch(InvalidOperationException){
				return null;
			} 
		}

		public IEnumerable<Profile> FilterByGroup(string group) {
			return Profiles.Where(p => {
				var isInGroup = p.Group == group;
				p.Group = null;
				return isInGroup;
			});
		}

		//// Keep for telnet support
		// public IEnumerable<Profile> FilterByType(string type) {
		// 	return Profiles.Where(p => {
		// 		var isOfType = p.Type == type;
		// 		p.Type = null;
		// 		return isOfType;
		// 	});
		// }

	}
}
