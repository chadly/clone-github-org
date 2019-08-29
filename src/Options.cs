using CommandLine;

namespace CloneGithubOrg
{
	public class Options
	{
		[Option('u', "username", Required = false, HelpText = "The username to use when authenticating to clone repositories.")]
		public string Username { get; set; }

		[Option('p', "password", Required = false, HelpText = "The password (personal access token) to use when authenticating to the Github API and to clone repositories.")]
		public string Password { get; set; }

		[Option('o', "org", Required = true, HelpText = "The Github organization from which to clone repositories.")]
		public string Org { get; set; }
	}
}
