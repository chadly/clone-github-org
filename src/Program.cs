using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace CloneOrg
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Options opts = null;

			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(o => opts = o)
				.WithNotParsed(errs => Environment.Exit(1));

			var serviceProvider = BuildServiceProvider(opts);

			var cloner = serviceProvider.GetRequiredService<Cloner>();

			await cloner.CloneReposForOrg(opts.Org);
		}

		static IServiceProvider BuildServiceProvider(Options opts)
		{
			var services = new ServiceCollection();

			services.AddHttpClient<GithubApi>(client =>
			{
				client.BaseAddress = new Uri("https://api.github.com");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

				if (!String.IsNullOrWhiteSpace(opts.Password))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", opts.Password);
				}

				client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CloneOrg", "v1"));
			});

			services.AddSingleton(opts);

			services.AddSingleton<Cloner>();

			return services.BuildServiceProvider();
		}
	}
}
