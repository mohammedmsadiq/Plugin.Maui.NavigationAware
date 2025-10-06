using System.Collections;

namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Default implementation of INavigationParameters
/// </summary>
public class NavigationParameters : INavigationParameters
{
    private readonly Dictionary<string, object> _parameters = new();

    /// <summary>
    /// Initializes a new instance of NavigationParameters
    /// </summary>
    public NavigationParameters()
    {
    }

    /// <summary>
    /// Initializes a new instance of NavigationParameters with initial values
    /// </summary>
    /// <param name="parameters">Initial parameters</param>
    public NavigationParameters(IDictionary<string, object>? parameters)
    {
        if (parameters != null)
        {
            foreach (var kvp in parameters)
            {
                _parameters[kvp.Key] = kvp.Value;
            }
        }
    }

    /// <inheritdoc/>
    public T? GetValue<T>(string key)
    {
        if (_parameters.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    /// <inheritdoc/>
    public bool TryGetValue<T>(string key, out T? value)
    {
        if (_parameters.TryGetValue(key, out var obj) && obj is T typedValue)
        {
            value = typedValue;
            return true;
        }
        value = default;
        return false;
    }

    #region IDictionary Implementation
    public object this[string key]
    {
        get => _parameters[key];
        set => _parameters[key] = value;
    }

    public ICollection<string> Keys => _parameters.Keys;
    public ICollection<object> Values => _parameters.Values;
    public int Count => _parameters.Count;
    public bool IsReadOnly => false;

    public void Add(string key, object value) => _parameters.Add(key, value);
    public void Add(KeyValuePair<string, object> item) => _parameters.Add(item.Key, item.Value);
    public void Clear() => _parameters.Clear();
    public bool Contains(KeyValuePair<string, object> item) => _parameters.Contains(item);
    public bool ContainsKey(string key) => _parameters.ContainsKey(key);
    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) =>
        ((ICollection<KeyValuePair<string, object>>)_parameters).CopyTo(array, arrayIndex);
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _parameters.GetEnumerator();
    public bool Remove(string key) => _parameters.Remove(key);
    public bool Remove(KeyValuePair<string, object> item) => ((ICollection<KeyValuePair<string, object>>)_parameters).Remove(item);
    public bool TryGetValue(string key, out object value) => _parameters.TryGetValue(key, out value!);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}
