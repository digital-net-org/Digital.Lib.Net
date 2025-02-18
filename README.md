<h1>
    <img width="300" src="https://raw.githubusercontent.com/digital-net-org/.github/refs/heads/master/assets/logo_v2025.svg">
</h1>
<div justify="center">
    <a href="https://dotnet.microsoft.com/en-us/languages/csharp"><img src="https://img.shields.io/badge/C%23-blue.svg?color=622075"></a>
    <a href="https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview?WT.mc_id=dotnet-35129-website"><img src="https://img.shields.io/badge/Dotnet-blue.svg?color=4f2bce"></a>
</div>

_@digital-net-org/Digital.Lib.Net_

A collection of packages that provide a set of tools and utilities to build Dotnet applications.

## :package: Installation

### Submodules

Use the following command to add the library to your project:
```bash
git@github.com:digital-net-org/Digital.Lib.Net.git packages/Digital.Lib
```

Then update your `<project>.sln` file with needed submodules or use your IDE to add the library to your solution.

## Rest API Configuration
Using `Digital.Lib.Net.Bootstrap` and `Digital.Lib.Net.Core`, inject the following configuration with an 
`appsettings.json` file or environment variables. If using environment variables, replace the separator `:` with `__`. 

### Base options
| Accessor                  | Default value                                                  | Type       | Example                                                       | Description                                                                                                                                                  |
|---------------------------|----------------------------------------------------------------|------------|---------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Domain                    | **Mandatory**                                                  | `string`   | `example.com`                                                 | Describes your application domain. Used to **prefix Cookies**, setup JWT **Audience/Issuer** and all subdomains will be added the allowed **CORS policies**. |
| CorsAllowedOrigins        | `[]`                                                           | `string[]` | `[example.com]`                                               | All entries will be added the allowed **CORS policies** _(be aware that Domain is automatically added to allowed origins)_.                                  |
| ConnectionStrings:Default | **Mandatory**                                                  | `string`   | `"Host=host;Port=5432;Database=db;Username=usr;Password=psw"` | **Postgres** Database connection string.                                                                                                                     |
| FileSystem:Path           | `"/digital_net_storage"`                                       | `string`   | -                                                             | Path to folder where the application will save uploaded files.                                                                                               |
| FileSystem:MaxAvatarSize  | `2097152`                                                      | `number`   | -                                                             | Max size for stored user avatar expressed in bytes.                                                                                                          |
| Auth:JwtRefreshExpiration | `1800000`                                                      | `number`   | -                                                             | Refresh token expiration expressed in milliseconds                                                                                                           |
| Auth:JwtBearerExpiration  | `300000`                                                       | `number`   | -                                                             | Bearer token expiration expressed in milliseconds                                                                                                            |
| Auth:JwtSecret            | **Mandatory**                                                  | `string`   | `superLongSecretThatNeedsToBeSuperLongAndSecure`              | Secret for Jwt configuration, must be a least 46 characters long.                                                                                            |
| Auth:PasswordRegex        | `^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{12,128}$` | `string`   | -                                                             | Regex that enforce password policy.                                                                                                                          |