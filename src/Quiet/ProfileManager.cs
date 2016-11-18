using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Quiet{
    public class ProfileManager {

		public IEnumerable<Profile> Profiles { get; set; }

		public ProfileManager() {
			var json = File.ReadAllText("data/profiles.json");
			Profiles = JsonConvert.DeserializeObject<IEnumerable<Profile>>(json);
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
