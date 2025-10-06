using Plugin.Maui.NavigationAware;

namespace Plugin.Maui.NavigationAware.Sample;

public partial class MainPage : ContentPage
{
	readonly IFeature feature;

	public MainPage(IFeature feature)
	{
		InitializeComponent();
		
		this.feature = feature;
	}
}
