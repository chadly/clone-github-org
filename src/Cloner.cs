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
		readonly Options opts;

		public Cloner(GithubApi api, Options opts)
		{
			this.api = api;
			this.opts = opts;
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
					CredentialsProvider = (url, user, cred) =>
					{
						if (String.IsNullOrWhiteSpace(opts.Username) || String.IsNullOrWhiteSpace(opts.Password))
							return null;

						return new UsernamePasswordCredentials
						{
							Username = opts.Username,
							Password = opts.Password
						};
					}
				});
				Console.WriteLine("Done");

				Console.WriteLine();
			}
		}
	}
}