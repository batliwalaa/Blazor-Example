namespace Fluxor.Blazor.Persistence.Store;

public static class PersistedStateReducers
{
  [ReducerMethod]
  public static IDictionary<string, object> OnLoadPersistedState(
    IDictionary<string, object> state,
    LoadPersistedStateSuccessAction action)
  {
    return state;
  }
}
