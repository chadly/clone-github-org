using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CloneGithubOrg
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

				var process = new Process();
				process.StartInfo.FileName = "git";
				process.StartInfo.Arguments = $"clone {RepoArgs(repo.FullName)}";
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.UseShellExecute = false;

				process.StartInfo.RedirectStandardOutput = true;
				process.OutputDataReceived += (sender, data) =>
				{
					Console.WriteLine(data.Data);
				};

				process.StartInfo.RedirectStandardError = true;
				process.ErrorDataReceived += (sender, data) =>
				{
					Console.WriteLine(data.Data);
				};

				process.Start();
				process.WaitForExit();

				Console.WriteLine("Done");

				Console.WriteLine();
			}
		}

		string RepoArgs(string repo)
		{
			string auth = !String.IsNullOrWhiteSpace(opts.Username) && !String.IsNullOrWhiteSpace(opts.Password)
				? $"{opts.Username}:{opts.Password}@" : null;

			return $"https://{auth}github.com/{repo}.git";

		}
	}
}