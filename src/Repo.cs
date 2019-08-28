using Newtonsoft.Json;

namespace CloneOrg
{
	/// <summary>
	/// https://developer.github.com/v3/activity/events/types/#pullrequestevent
	/// </summary>
	public class Repo
	{
		public int Id { get; set; }

		public string Name { get; set; }
		[JsonProperty("full_name")]
		public string FullName { get; set; }

		public string Description { get; set; }

		public bool Private { get; set; }
		public bool Fork { get; set; }
	}
}
