using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.Lib.Net.Files.Services;

public interface IDocumentService
{
    FileResult? GetDocumentFile(Guid documentId, string? contentType = null);
    FileResult? GetDocumentFile(Document document, string? contentType = null);
    string GetDocumentPath(Document document);
    Task<Result<Document>> SaveImageDocumentAsync(IFormFile file, int? quality = null);
    Task<Result<Document>> SaveDocumentAsync(IFormFile file);
    Task<Result> RemoveDocumentAsync(Document? document);
    Task<Result> RemoveDocumentAsync(Guid id);
}