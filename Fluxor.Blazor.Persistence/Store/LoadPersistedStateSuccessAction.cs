namespace Fluxor.Blazor.Persistence.Store;

public class LoadPersistedStateSuccessAction
{
  public IDictionary<string, object> State { get; private set; }

  public LoadPersistedStateSuccessAction(IDictionary<string, object> state)
  {
    State = state;
  }
}
