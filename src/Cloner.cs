using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace CloneOrg
{
	public class Cloner
	{
		readonly GithubApi api;
		readonly UsernamePasswordCredentials credentials;

		public Cloner(GithubApi api, UsernamePasswordCredentials credentials)
		{
			this.api = api;
			this.credentials = credentials;
		}

		public async Task CloneReposForOrg(string org)
		{
			var repos = await api.GetReposForOrg(org);

			Console.WriteLine($"About to clone {repos.Count()} repos:");
			Console.WriteLine();

			foreach (var repo in repos)
			{
				Console.WriteLine($"Cloning {repo.FullName}...");

				Repository.Clone($"https://github.com/{repo.FullName}.git", Path.Combine(Directory.GetCurrentDirectory(), repo.Name), new CloneOptions
				{
					CredentialsProvider = (url, user, cred) => credentials
				});
				Console.WriteLine("Done");

				Console.WriteLine();
			}
		}
	}
}