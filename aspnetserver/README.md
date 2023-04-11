# .NET application

Contents:

1. [Deploying to Azure](#deploying-to-azure)

## Deploying to Azure

1. Go to react app and run build

```bash
$ cd reactclient
$ npm run build
```

2. Copy over the `build` directory to aspnetserver's `wwwroot`

```bash
$ cd ../aspnetserver
$ rm -rf wwwroot # Deletes previous wwwroot directory
$ cp -R ../reactclient/build ./wwwroot
```

3. Deploy to Azure from Visual Studio using `Build > Publish to Azure...`
4. You should be able to visit the application at [https://launchhouse.azurewebsites.net/](https://launchhouse.azurewebsites.net/)