using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models.Documents;
using Microsoft.AspNetCore.Http;

namespace Digital.Lib.Net.Files.Services;

public interface IDocumentService
{
    string GetDocumentPath(Document document);
    Task<Result<Document>> SaveImageDocumentAsync(IFormFile file, int? quality = null);
    Task<Result> RemoveDocumentAsync(Document? document);
    Task<Result> RemoveDocumentAsync(Guid id);
}