# Documentation

This folder contains the complete documentation for Plugin.Maui.NavigationAware.

## Files

- **index.html** - Main HTML documentation landing page
- **index.md** - Main markdown documentation (for GitHub Pages or other static site generators)
- **getting-started.md** - Getting started guide
- **api-reference.md** - Complete API reference
- **examples.md** - Practical usage examples
- **migration-guide.md** - Migration guide from other frameworks

## Publishing Documentation

### Option 1: GitHub Pages

You can publish the HTML documentation using GitHub Pages:

1. Go to your repository Settings
2. Navigate to Pages section
3. Set Source to "Deploy from a branch"
4. Select branch: `main` (or your default branch)
5. Select folder: `/docs`
6. Click Save

Your documentation will be available at: `https://mohammedmsadiq.github.io/Plugin.Maui.NavigationAware/`

### Option 2: Read the Docs

You can publish the documentation using Read the Docs, which will automatically build and host it:

1. **Sign up at Read the Docs**:
   - Go to [Read the Docs](https://readthedocs.org/)
   - Sign in with your GitHub account

2. **Import your GitHub repository**:
   - Click "Import a Project"
   - Select the `Plugin.Maui.NavigationAware` repository from your GitHub account
   - Click the "+" button next to it

3. **Configure the project** (optional):
   - The repository is already configured with `.readthedocs.yaml`
   - The documentation will be built automatically using MkDocs
   - Default settings should work out of the box

4. **Build and publish**:
   - Click "Build Version" or wait for the automatic build to start
   - Your documentation will be available at: `https://plugin-maui-navigationaware.readthedocs.io/`
   - Every push to the main branch will automatically rebuild the documentation

**Configuration files:**
- `.readthedocs.yaml` - Read the Docs configuration
- `mkdocs.yml` - MkDocs documentation generator configuration
- `docs/requirements.txt` - Python dependencies for building documentation

### Option 3: DocFX

For a more advanced documentation site, you can use DocFX:

```bash
# Install DocFX
dotnet tool install -g docfx

# Generate documentation
docfx init
# Configure docfx.json to point to your source code and markdown files
docfx build

# Serve locally
docfx serve _site
```

### Option 4: Self-Hosting

You can host the HTML files on any web server:

1. Copy all files from `/docs` folder to your web server
2. Ensure your server is configured to serve static HTML files
3. Access `index.html` from your browser

## Updating Documentation

To update the documentation:

1. Edit the relevant markdown files
2. If you made changes to code examples, verify they work
3. Commit and push changes
4. Documentation will be automatically updated if using GitHub Pages

## Generating from Code

For API documentation generated from code comments, consider using:

- **DocFX** - Generates documentation from XML comments
- **Sandcastle** - Generates comprehensive API documentation
- **Doxygen** - Cross-platform documentation generator

## Contributing

If you'd like to improve the documentation, please:

1. Fork the repository
2. Make your changes to the markdown files
3. Test the changes locally
4. Submit a pull request

## License

Documentation is licensed under the same MIT license as the project.
