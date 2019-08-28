using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Archon.Http;
using Newtonsoft.Json;

namespace CloneOrg
{
	public class GithubApi
	{
		readonly HttpClient client;

		public GithubApi(HttpClient client)
		{
			this.client = client;
		}

		public Task<IEnumerable<Repo>> GetReposForOrg(string org, string type = "all")
		{
			return AggregatePaginatedResponse<Repo>($"/orgs/{org}/repos?type={type}&per_page=100");
		}

		async Task<IEnumerable<T>> AggregatePaginatedResponse<T>(string url)
		{
			var results = new List<T>();

			string nextPageLink = url;

			while (nextPageLink != null)
			{
				var response = await client.GetAsync(nextPageLink);
				await response.EnsureSuccess();

				results.AddRange(await ReadAsAsync<IEnumerable<T>>(response.Content));

				nextPageLink = ParseNextPageLink(response.Headers);
			}

			return results;
		}

		string ParseNextPageLink(HttpResponseHeaders headers)
		{
			IEnumerable<string> headerLinks;
			if (!headers.TryGetValues("Link", out headerLinks))
				return null;

			string links = headerLinks.SingleOrDefault();
			if (String.IsNullOrWhiteSpace(links))
				return null;

			string[] parts = links.Split(',');

			string nextLink = parts.SingleOrDefault(p => p.Contains("rel=\"next\""));
			if (String.IsNullOrWhiteSpace(nextLink))
				return null;

			if (!nextLink.Contains("<") && !nextLink.Contains(">"))
				return null;

			return nextLink.Trim().Substring(1, nextLink.IndexOf(">") - 1);
		}

		async Task<T> ReadAsAsync<T>(HttpContent content)
		{
			string json = await content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}