using Digital.Lib.Net.Core.Models;
using Digital.Lib.Net.Core.Random;
using Digital.Lib.Net.Entities.Models.Avatars;
using Digital.Lib.Net.Entities.Test.Models.Documents;

namespace Digital.Lib.Net.Entities.Test.Models.Avatars;

public class AvatarTest
{
    public static readonly Avatar TestAvatar = new()
    {
        X = Randomizer.GenerateRandomInt(),
        Y = Randomizer.GenerateRandomInt(),
        Document = DocumentTest.TestDocument
    };

    [Fact]
    public void AvatarModel_ReturnsValidModel()
    {
        var dto = Mapper.MapFromConstructor<Avatar, AvatarDto>(TestAvatar);
        Assert.NotNull(dto);
        Assert.IsType<AvatarDto>(dto);
        Assert.Equal(TestAvatar.Id, dto.Id);
        Assert.Equal(TestAvatar.Document?.Id, dto.DocumentId);
        Assert.Equal(TestAvatar.X, dto.X);
        Assert.Equal(TestAvatar.Y, dto.Y);
    }
}