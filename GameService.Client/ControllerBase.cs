using GameService.Abstraction;

namespace GameService.Client;

public abstract class ControllerBase(IHttpClientFactory factory) : IController
{
    protected virtual HttpClient Client => factory.CreateClient(IController.Name);
}