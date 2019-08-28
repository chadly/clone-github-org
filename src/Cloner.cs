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

		public Cloner(GithubApi api)
		{
			this.api = api;
		}

		public async Task CloneReposForOrg(string org)
		{
			var repos = await api.GetReposForOrg(org);

			Console.WriteLine($"About to clone {repos.Count()} repos:");
			Console.WriteLine();

			foreach (var repo in repos)
			{
				Console.WriteLine($"Cloning {repo.FullName}...");
				Repository.Clone($"https://github.com/{repo.FullName}.git", Path.Combine(Directory.GetCurrentDirectory(), repo.Name));
				Console.WriteLine("Done");

				Console.WriteLine();
			}
		}
	}
}