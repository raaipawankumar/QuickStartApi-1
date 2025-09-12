using System;

namespace QuickStart.Api.Common.Endpoints;

public interface IEndpoint
{
  public void Map(IEndpointRouteBuilder app);
}
