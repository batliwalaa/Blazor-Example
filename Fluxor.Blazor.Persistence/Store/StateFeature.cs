namespace Fluxor.Blazor.Persistence.Store;

public class StateFeature : Feature<IDictionary<string, object>>
{
  public override string GetName() => "";

  protected override IDictionary<string, object> GetInitialState()
  {
    return new Dictionary<string, object>();
  }
}
