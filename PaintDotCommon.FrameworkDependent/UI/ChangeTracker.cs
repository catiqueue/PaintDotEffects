using System.Collections.Generic;
using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.Collections;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI;

public class ChangeTracker {
  private static readonly object DefaultValue = new();
  private readonly Dictionary<UiNodeBase, object> _previousState;
  private readonly Dictionary<UiNodeBase, object> _currentState;
  private readonly UiNodeBase[] _trackedProperties;

  public IEnumerable<UiNodeBase> Changes =>
    _trackedProperties.Where(prop => !_previousState[prop].Equals(_currentState[prop]));
  
  public ChangeTracker(UiNodeBase[] trackedProperties) {
    _trackedProperties = trackedProperties;
    int count = _trackedProperties.Length;
    _previousState = new Dictionary<UiNodeBase, object>(count);
    _currentState = new Dictionary<UiNodeBase, object>(count);
    Initialize();
  }
  
  private void Initialize() => _trackedProperties.ForEach(prop => {
    _previousState.Add(prop, DefaultValue);
    _currentState.Add(prop, DefaultValue);
  });

  public void Update(PropertyCollection properties) => _trackedProperties.ForEach(prop => {
    _previousState[prop] = _currentState[prop];
    _currentState[prop] = properties.GetPropertyValue<object>(prop.Name);
  });
}