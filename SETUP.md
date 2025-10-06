# Quick Setup Guide

## For the Repository Owner

This guide helps you complete the setup of your Plugin.Maui.NavigationAware package.

### 1. Enable GitHub Pages

To publish the documentation website:

1. Go to your repository on GitHub
2. Click **Settings** → **Pages**
3. Under "Source", select:
   - Source: **Deploy from a branch**
   - Branch: **main** (or your default branch)
   - Folder: **/docs**
4. Click **Save**

Your documentation will be published at:
**https://mohammedmsadiq.github.io/Plugin.Maui.NavigationAware/**

(It may take a few minutes for the site to become available)

### 2. Set Up NuGet Publishing

To enable automatic NuGet package publishing:

1. Get your NuGet API key from https://www.nuget.org/account/apikeys
2. Go to your repository on GitHub
3. Click **Settings** → **Secrets and variables** → **Actions**
4. Click **New repository secret**
5. Name: `NUGET_API_KEY`
6. Value: Paste your NuGet API key
7. Click **Add secret**

### 3. Publish Your First Release

When you're ready to publish to NuGet:

```bash
# Create and push a version tag
git tag v1.0.0
git push origin v1.0.0

# Or create a tag with annotation
git tag -a v1.0.0 -m "First stable release"
git push origin v1.0.0
```

Then on GitHub:
1. Go to **Releases** → **Draft a new release**
2. Choose your tag (v1.0.0)
3. Write release notes (you can use content from CHANGELOG.md)
4. Click **Publish release**

The GitHub Action will automatically:
- Build the NuGet package
- Publish it to NuGet.org
- Create symbol packages

### 4. Marketing Your Plugin

Now that documentation is published, you can share:

- **Documentation URL**: https://mohammedmsadiq.github.io/Plugin.Maui.NavigationAware/
- **NuGet Package**: https://www.nuget.org/packages/Plugin.Maui.NavigationAware/
- **GitHub Repository**: https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware

Share on:
- Twitter/X with #DOTNETMAUI #MAUI
- LinkedIn
- Reddit r/dotnet
- .NET MAUI Community Discord
- Dev.to with .NET MAUI tag

### 5. Optional: Custom Domain for Docs

If you have a custom domain:

1. In your repository, go to **Settings** → **Pages**
2. Under "Custom domain", enter your domain (e.g., `navigationaware.yourdomain.com`)
3. Create a CNAME file in `/docs`:
   ```
   navigationaware.yourdomain.com
   ```
4. Configure your DNS provider to point to GitHub Pages

### 6. Maintaining the Package

- Update version numbers in the `.csproj` file
- Update `CHANGELOG.md` with changes
- Create new tags for new versions
- GitHub Actions will handle the rest!

## For Users Installing the Package

### Installation

```bash
dotnet add package Plugin.Maui.NavigationAware
```

### Quick Start

```csharp
using Plugin.Maui.NavigationAware;

public partial class MyPage : NavigationAwarePage
{
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        // Handle navigation to this page
    }

    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
        // Handle navigation away from this page
    }
}
```

See full documentation at: https://mohammedmsadiq.github.io/Plugin.Maui.NavigationAware/

## Support

- **Issues**: https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/issues
- **Discussions**: https://github.com/mohammedmsadiq/Plugin.Maui.NavigationAware/discussions
- **Documentation**: https://mohammedmsadiq.github.io/Plugin.Maui.NavigationAware/

## Next Steps After Setup

1. ✅ Enable GitHub Pages
2. ✅ Set up NuGet API key
3. ✅ Create first release (v1.0.0)
4. 📢 Share with the community
5. 📝 Write a blog post or tutorial
6. 🎥 Create a demo video
7. 💬 Engage with users in issues/discussions

Congratulations on creating your .NET MAUI plugin! 🎉
