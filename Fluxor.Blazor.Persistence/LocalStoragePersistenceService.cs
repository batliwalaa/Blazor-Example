using Blazored.LocalStorage;

namespace Fluxor.Blazor.Persistence;

internal sealed class LocalStoragePersistenceService
{
  private readonly ILocalStorageService _localStorageService;
  private readonly PersistOtions _persistOptions;

  public LocalStoragePersistenceService(
    ILocalStorageService localStorageService,
    PersistOtions persistOptions) =>
      (_localStorageService, _persistOptions) = (localStorageService, persistOptions);

  public async Task SaveAsync(IDictionary<string, object> state)
  {
    await _localStorageService.SetItemAsync(_persistOptions.PersistenceKey, state);
  }

  public async Task<IDictionary<string, object>> LoadAsync()
  {
    return await _localStorageService.GetItemAsync<IDictionary<string, object>>(
      _persistOptions.PersistenceKey) ?? new Dictionary<string, object>();
  }
}
