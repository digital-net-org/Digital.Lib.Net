using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Digital.Net.Core.Extensions.HttpUtilities;

public static class ContextAccessor
{
    /// <summary>
    ///     Get the HttpContext from the IHttpContextAccessor.
    ///     Throws a NullReferenceException if the context is not defined.
    /// </summary>
    /// <param name="accessor">The IHttpContextAccessor to get the HttpContext from.</param>
    /// <returns>The HttpContext from the IHttpContextAccessor.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public static HttpContext GetContext(this IHttpContextAccessor accessor) =>
        accessor.HttpContext ?? throw new NullReferenceException("Http Context is not defined");

    /// <summary>
    ///     Get the HttpRequest from the IHttpContextAccessor.
    ///     Throws a NullReferenceException if the request is not defined.
    /// </summary>
    /// <param name="accessor">The IHttpContextAccessor to get the HttpRequest from.</param>
    /// <returns>The HttpRequest from the IHttpContextAccessor.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public static HttpRequest GetRequest(this IHttpContextAccessor accessor) =>
        accessor.GetContext().Request ?? throw new NullReferenceException("Http Request is not defined");

    /// <summary>
    ///     Get the HttpResponse from the IHttpContextAccessor.
    ///     Throws a NullReferenceException if the response is not defined.
    /// </summary>
    /// <param name="accessor">The IHttpContextAccessor to get the HttpResponse from.</param>
    /// <returns>The HttpResponse from the IHttpContextAccessor.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public static HttpResponse GetResponse(this IHttpContextAccessor accessor) =>
        accessor.GetContext().Response ?? throw new NullReferenceException("Http Response is not defined");

    /// <summary>
    ///     Add an item to the HttpContext.Items collection.
    ///     The item is serialized to a string before being added.
    /// </summary>
    /// <param name="context">The HttpContext to add the item to.</param>
    /// <param name="key">The key to add the item to.</param>
    /// <param name="content">The content to add to the HttpContext.Items collection.</param>
    /// <typeparam name="T">The type of the content to add to the HttpContext.Items collection.</typeparam>
    public static void AddItem<T>(this HttpContext context, string key, T content) =>
        context.Items[key] = JsonSerializer.Serialize(content);

    /// <summary>
    ///     Get an item from the HttpContext.Items collection.
    /// </summary>
    /// <param name="context">The HttpContext to get the item from.</param>
    /// <param name="key">The key to get the item from.</param>
    /// <typeparam name="T">The type of the item to get from the HttpContext.Items collection.</typeparam>
    /// <returns>The item from the HttpContext.Items collection.</returns>
    public static T? GetItem<T>(this HttpContext context, string key) =>
        context.Items[key] is not string item ? default : JsonSerializer.Deserialize<T>(item);
}