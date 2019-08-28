using System;
using System.Linq;
using System.Threading.Tasks;

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
				Console.WriteLine(repo.FullName);
			}
		}
	}
}