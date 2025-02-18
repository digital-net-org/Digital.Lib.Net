namespace Digital.Lib.Net.Authentication.Attributes;

[Flags]
public enum AuthorizeType
{
    ApiKey,
    Jwt,
    Any = ApiKey | Jwt
}