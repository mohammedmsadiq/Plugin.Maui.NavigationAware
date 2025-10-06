namespace Plugin.Maui.NavigationAware;

/// <summary>
/// Provides attached properties for automatic ViewModel location and binding in XAML
/// </summary>
public static class ViewModelLocator
{
    /// <summary>
    /// Attached property to enable automatic ViewModel wiring
    /// </summary>
    public static readonly BindableProperty AutoWireViewModelProperty =
        BindableProperty.CreateAttached(
            "AutoWireViewModel",
            typeof(bool),
            typeof(ViewModelLocator),
            false,
            propertyChanged: OnAutoWireViewModelChanged);

    /// <summary>
    /// Gets the AutoWireViewModel attached property value
    /// </summary>
    /// <param name="bindable">The bindable object</param>
    /// <returns>True if auto-wiring is enabled, false otherwise</returns>
    public static bool GetAutoWireViewModel(BindableObject bindable)
    {
        return (bool)bindable.GetValue(AutoWireViewModelProperty);
    }

    /// <summary>
    /// Sets the AutoWireViewModel attached property value
    /// </summary>
    /// <param name="bindable">The bindable object</param>
    /// <param name="value">True to enable auto-wiring, false otherwise</param>
    public static void SetAutoWireViewModel(BindableObject bindable, bool value)
    {
        bindable.SetValue(AutoWireViewModelProperty, value);
    }

    private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is bool autoWire && autoWire)
        {
            ViewModelLocationProvider.AutoWireViewModel(bindable);
        }
    }

    /// <summary>
    /// Attached property to specify a specific ViewModel type
    /// </summary>
    public static readonly BindableProperty ViewModelTypeProperty =
        BindableProperty.CreateAttached(
            "ViewModelType",
            typeof(Type),
            typeof(ViewModelLocator),
            null,
            propertyChanged: OnViewModelTypeChanged);

    /// <summary>
    /// Gets the ViewModelType attached property value
    /// </summary>
    /// <param name="bindable">The bindable object</param>
    /// <returns>The ViewModel type</returns>
    public static Type? GetViewModelType(BindableObject bindable)
    {
        return (Type?)bindable.GetValue(ViewModelTypeProperty);
    }

    /// <summary>
    /// Sets the ViewModelType attached property value
    /// </summary>
    /// <param name="bindable">The bindable object</param>
    /// <param name="value">The ViewModel type</param>
    public static void SetViewModelType(BindableObject bindable, Type? value)
    {
        bindable.SetValue(ViewModelTypeProperty, value);
    }

    private static void OnViewModelTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is Type viewModelType)
        {
            ViewModelLocationProvider.AutoWireViewModel(bindable, viewModelType);
        }
    }
}
