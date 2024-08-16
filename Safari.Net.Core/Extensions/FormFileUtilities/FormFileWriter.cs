using Microsoft.AspNetCore.Http;
using Safari.Net.Core.Messages;

namespace Safari.Net.Core.Extensions.FormFileUtilities;

public static class FormFileWriter
{
    public static Result TryWriteFile(this IFormFile form, string path)
    {
        var result = new Result<IFormFile>();
        try
        {
            using var stream = new FileStream(path, FileMode.Create);
            form.CopyTo(stream);
        }
        catch (Exception ex)
        {
            result.AddError(ex);
        }

        return result;
    }

    public static async Task<Result> TryWriteFileAsync(this IFormFile form, string path)
    {
        var result = new Result<IFormFile>();
        try
        {
            await using var stream = new FileStream(path, FileMode.Create);
            await form.CopyToAsync(stream);
        }
        catch (Exception ex)
        {
            result.AddError(ex);
        }

        return result;
    }

    public static Result TryRemoveFile(string path)
    {
        var result = new Result();
        try
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        catch (Exception ex)
        {
            result.AddError(ex);
        }

        return result;
    }
}
