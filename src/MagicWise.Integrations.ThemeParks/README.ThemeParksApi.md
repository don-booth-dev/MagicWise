ThemeParksApi integration
=========================

This folder contains a small ThemeParksApi HTTP client for the themeparks.wiki v1 API.

Registration examples
---------------------

1) Simple registration (no resilience)

```csharp
// using Microsoft.Extensions.DependencyInjection;
// in Program.cs or Startup.cs
services.AddHttpClient<ThemeParksApi>(c =>
{
	c.BaseAddress = new Uri("https://themeparks.wiki/api/");
	c.DefaultRequestHeaders.UserAgent.ParseAdd("MagicWise/1.0");
});
```

2) Registration with Polly-based retry/backoff

```csharp
// using Polly;
// using Polly.Extensions.Http;
// using Microsoft.Extensions.DependencyInjection;

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
	return HttpPolicyExtensions
		.HandleTransientHttpError()
		.OrResult(msg => (int)msg.StatusCode == 429)
		.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

services.AddHttpClient<ThemeParksApi>(c =>
{
	c.BaseAddress = new Uri("https://themeparks.wiki/api/");
	c.DefaultRequestHeaders.UserAgent.ParseAdd("MagicWise/1.0");
})
	.AddPolicyHandler(GetRetryPolicy());
```

Notes
-----
- The client expects an HttpClient with BaseAddress set to the API base URL. If you prefer, pass full absolute URLs into the methods (the client currently uses relative URIs like /v1/entity/{id}).
- Methods return strongly typed DTOs (in Models/). DTOs are conservative; update properties to reflect actual API responses for improved typing.

Usage examples
--------------

Inject ThemeParksApi in a service or controller:

```csharp
public class MyService
{
	private readonly ThemeParksApi _api;

	public MyService(ThemeParksApi api) => _api = api;

	public async Task DoWork()
	{
		var destinations = await _api.GetDestinationsAsync();
		var entity = await _api.GetEntityAsync("parks/epcot");
		var schedule = await _api.GetEntityScheduleAsync("parks/epcot", 2026, 8);
		// handle nulls and use JsonElement fields when needed
	}
}
```

Testing
-------

For unit tests, mock the HttpMessageHandler and construct an HttpClient with it, or use RichardSzalay.MockHttp to create canned responses and validate the deserialization logic and error handling.
