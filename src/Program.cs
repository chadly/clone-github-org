using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LibGit2Sharp;
using Microsoft.Extensions.DependencyInjection;

namespace CloneOrg
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var serviceProvider = BuildServiceProvider(args[0]);

			var cloner = serviceProvider.GetRequiredService<Cloner>();

			await cloner.CloneReposForOrg(args[1]);
		}

		static IServiceProvider BuildServiceProvider(string apiKey)
		{
			var services = new ServiceCollection();

			services.AddHttpClient<GithubApi>(client =>
			{
				client.BaseAddress = new Uri("https://api.github.com");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", apiKey);
				client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CloneOrg", "v1"));
			});

			services.AddSingleton(new UsernamePasswordCredentials
			{
				Username = "chadly",
				Password = apiKey
			});

			services.AddSingleton<Cloner>();

			return services.BuildServiceProvider();
		}
	}
}
