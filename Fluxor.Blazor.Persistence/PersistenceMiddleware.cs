using Blazored.LocalStorage;
using Fluxor.Blazor.Persistence.Store;

namespace Fluxor.Blazor.Persistence;

internal sealed class PersistenceMiddleware : Middleware
{
  private IStore? _store;
  private PersistOtions _persistOtions;
  private readonly LocalStoragePersistenceService _localStoragePersistenceService;
  private IDispatcher? _dispatcher;
  private readonly object SyncRoot = new();

  public PersistenceMiddleware(
    PersistOtions persistOtions,
    LocalStoragePersistenceService localStoragePersistenceService)
  {
    _persistOtions = persistOtions;
    _localStoragePersistenceService = localStoragePersistenceService;
  }

  public override async Task InitializeAsync(IDispatcher dispatcher, IStore store)
  {
    _store = store;
    _dispatcher = dispatcher;

    await Task.CompletedTask;
  }

  public override void AfterDispatch(object action)
  {
    lock (SyncRoot)
    {
      IDictionary<string, object> state = GetState();
      _localStoragePersistenceService.SaveAsync(state).ConfigureAwait(false);
    }
  }


  public override void AfterInitializeAllMiddlewares()
  {
    _dispatcher?.Dispatch(new LoadPersistedStateAction());
  }

  private IDictionary<string, object> GetState()
  {
    var state = new Dictionary<string, object>();
    if (_store != null)
    {
      foreach (IFeature feature in _store.Features.Values.OrderBy(x => x.GetName()))
      {
        state[feature.GetName()] = feature.GetState();
      }
    }

    return state;
  }
}
