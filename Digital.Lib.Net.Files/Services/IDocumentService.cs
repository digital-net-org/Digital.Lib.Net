using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models.Documents;
using Digital.Lib.Net.Entities.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.Lib.Net.Files.Services;

public interface IDocumentService
{
    string GetDocumentPath(Document document);
    FileResult? GetDocumentFile(Guid documentId, string? contentType = null);
    Task<Result<Document>> SaveDocumentAsync(IFormFile file, User uploader);
    Task<Result<Document>> SaveImageDocumentAsync(IFormFile file, User uploader, int? quality = null);
    Task<Result> RemoveDocumentAsync(Guid id);
}