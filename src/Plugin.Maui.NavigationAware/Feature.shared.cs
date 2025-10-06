namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Legacy class - use NavigationService and INavigationAware instead
/// </summary>
[Obsolete("This class is deprecated. Use NavigationService and INavigationAware instead.")]
public static class Feature
{
	static IFeature? defaultImplementation;

	/// <summary>
	/// Provides the default implementation for static usage of this API.
	/// </summary>
	[Obsolete("This property is deprecated. Use NavigationService and INavigationAware instead.")]
	public static IFeature Default =>
		defaultImplementation ??= new FeatureImplementation();

	internal static void SetDefault(IFeature? implementation) =>
		defaultImplementation = implementation;
}
