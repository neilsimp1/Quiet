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
			var json = File.ReadAllText("data/profiles.json");
			Profiles = JsonConvert.DeserializeObject<IEnumerable<Profile>>(json);
		}

		private static string GetProfilesPath() {
			return System.Runtime.InteropServices.RuntimeInformation.OSDescription.Contains("Windows")
				? Environment.GetEnvironmentVariable("USERPROFILE")
				: "~/.quiet/profiles.json";
		}

		public Profile GetProfile(string profileName) {
			return Profiles.First(profile => profile.Name == profileName);
		}

		public IEnumerable<Profile> FilterByGroup(string group) {
			return Profiles.Where(profile => {
				var isInGroup = profile.Group == group;
				profile.Group = null;
				return isInGroup;
			});
		}

		//// Keep for telnet support
		// public IEnumerable<Profile> FilterByType(string type) {
		// 	return Profiles.Where(profile => {
		// 		var isOfType = profile.Type == type;
		// 		profile.Type = null;
		// 		return isOfType;
		// 	});
		// }

	}
}
