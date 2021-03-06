// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using Nest;
using System.ComponentModel;

namespace Examples.Search.Request
{
	public class NamedQueriesAndFiltersPage : ExampleBase
	{
		[U]
		[Description("search/request/named-queries-and-filters.asciidoc:7")]
		public void Line7()
		{
			// tag::0aad4321e968effc6e6ef2b98c6c71a5[]
			var searchResponse = client.Search<object>(s => s
				.AllIndices()
				.Query(q => q
					.Bool(b => b
						.Should(sh => sh
							.Match(m => m
								.Field("name.first")
								.Query("shay")
								.Name("first")
							), sh => sh
							.Match(m => m
								.Field("name.last")
								.Query("banon")
								.Name("last")
							)
						)
						.Filter(f => f
							.Terms(t => t
								.Field("name.last")
								.Terms("banon", "kimchy")
								.Name("test")
							)
						)
					)
				)
			);
			// end::0aad4321e968effc6e6ef2b98c6c71a5[]

			searchResponse.MatchesExample(@"GET /_search
			{
			    ""query"": {
			        ""bool"" : {
			            ""should"" : [
			                {""match"" : { ""name.first"" : {""query"" : ""shay"", ""_name"" : ""first""} }},
			                {""match"" : { ""name.last"" : {""query"" : ""banon"", ""_name"" : ""last""} }}
			            ],
			            ""filter"" : {
			                ""terms"" : {
			                    ""name.last"" : [""banon"", ""kimchy""],
			                    ""_name"" : ""test""
			                }
			            }
			        }
			    }
			}", e => e.ApplyBodyChanges(json => json["query"]["bool"].ToLongFormBoolQuery()));
		}
	}
}
