using Digital.Lib.Net.Authentication.Exceptions;
using Digital.Lib.Net.Authentication.Services.Authentication;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.Documents;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Files.Exceptions;
using Digital.Lib.Net.Files.Extensions;
using Digital.Lib.Net.Sdk.Services.Options;
using Microsoft.AspNetCore.Http;

namespace Digital.Lib.Net.Files.Services;

public class DocumentService(
    IAppOptionService appOptionService,
    IAuthenticationService authenticationService,
    IRepository<Document, DigitalContext> documentRepository
) : IDocumentService
{
    public string GetDocumentPath(Document document) => Path.Combine(
        appOptionService.Get<string>(OptionAccessor.FileSystemPath),
        document.FileName
    );

    public async Task<Result<Document>> SaveImageDocumentAsync(IFormFile form, int? quality = null)
    {
        var result = new Result<Document>();
        var compressed = await form.CompressImageAsync(quality: quality);
        if (compressed.HasError() || compressed.Value is null)
            return result.Merge(compressed);

        result = await SaveDocumentAsync(compressed.Value);
        return result;
    }

    public async Task<Result> RemoveDocumentAsync(Document? document)
    {
        var result = new Result();
        if (document is null)
            return result.AddError(new DocumentNotFoundException());

        var path = GetDocumentPath(document);
        if (File.Exists(path))
            File.Delete(path);

        documentRepository.Delete(document);
        await documentRepository.SaveAsync();
        return result;
    }

    public async Task<Result> RemoveDocumentAsync(Guid id)
    {
        var document = await documentRepository.GetByIdAsync(id);
        return await RemoveDocumentAsync(document);
    }

    private async Task<Result<Document>> SaveDocumentAsync(IFormFile file)
    {
        var result = new Result<Document>();
        var user = await authenticationService.GetAuthenticatedUserAsync();
        if (user is null)
            return result.AddError(new UnauthorizedException());

        result.Value = new Document(user, file);
        await documentRepository.CreateAsync(result.Value);

        await using var stream = new FileStream(GetDocumentPath(result.Value), FileMode.Create);
        await file.CopyToAsync(stream);

        await documentRepository.SaveAsync();
        return result;
    }
}