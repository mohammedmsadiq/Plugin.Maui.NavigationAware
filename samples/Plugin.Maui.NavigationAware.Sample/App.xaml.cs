﻿namespace Plugin.Maui.NavigationAware.Sample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}