namespace Fluxor.Blazor.Persistence.Store;

public class LoadPersistedStateAction
{
  public IDictionary<string, object> State { get; private set; }

  public LoadPersistedStateAction(IDictionary<string, object> state)
  {
    State = state;
  }
}
