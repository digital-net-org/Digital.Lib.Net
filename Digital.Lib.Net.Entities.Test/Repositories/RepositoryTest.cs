using Digital.Lib.Net.Core.Random;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.Avatars;
using Digital.Lib.Net.Entities.Models.Documents;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.TestTools;
using Digital.Lib.Net.TestTools.Data;

namespace Digital.Lib.Net.Entities.Test.Repositories;

public class RepositoryTest : UnitTest
{
    private static Document GetTestDocument() => new()
    {
        FileName = Randomizer.GenerateRandomString(Randomizer.AnyCharacter),
        MimeType = Randomizer.GenerateRandomString(Randomizer.AnyCharacter),
        FileSize = 1
    };

    private readonly Repository<Document, DigitalContext> _documentRepository;
    private readonly Repository<Avatar, DigitalContext> _avatarRepository;

    public RepositoryTest()
    {
        var context = new SqliteMemoryDb<DigitalContext>().Context;
        _documentRepository = new Repository<Document, DigitalContext>(context);
        _avatarRepository = new Repository<Avatar, DigitalContext>(context);
    }

    [Fact]
    public async Task UpdateNestedEntities_ShouldUpdateNestedEntity()
    {
        const string payload = "test";
        var document = await _documentRepository.CreateAndSaveAsync(GetTestDocument());
        var avatar = await _avatarRepository.CreateAndSaveAsync(new Avatar { DocumentId = document.Id });
        avatar.Document!.FileName = payload;
        await _documentRepository.UpdateAndSaveAsync(document);

        var result = _documentRepository.Get().First();
        Assert.Equal(payload, result.FileName);
    }

    [Fact]
    public async Task AddTimestamps_ShouldSetNow_OnCreatedAtAndUpdatedAt()
    {
        var document = await _documentRepository.CreateAndSaveAsync(GetTestDocument());
        Assert.True(document.CreatedAt > DateTime.UtcNow.AddSeconds(-60));
        Assert.True(document.UpdatedAt is null);

        document = await _documentRepository.UpdateAndSaveAsync(document);
        Assert.True(document.UpdatedAt > DateTime.UtcNow.AddSeconds(-60));
    }
}