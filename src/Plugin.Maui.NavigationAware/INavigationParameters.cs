namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Represents navigation parameters passed during navigation events
/// </summary>
public interface INavigationParameters : IDictionary<string, object>
{
    /// <summary>
    /// Gets the value associated with the specified key
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The key of the value to get</param>
    /// <returns>The value associated with the key, or default if not found</returns>
    T? GetValue<T>(string key);

    /// <summary>
    /// Tries to get the value associated with the specified key
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The key of the value to get</param>
    /// <param name="value">The value associated with the key</param>
    /// <returns>True if the value was found, false otherwise</returns>
    bool TryGetValue<T>(string key, out T? value);
}
