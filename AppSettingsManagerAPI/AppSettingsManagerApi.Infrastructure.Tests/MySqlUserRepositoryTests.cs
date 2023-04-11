using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Infrastructure.MySql;
using AppSettingsManagerApi.Tests;
using AutoFixture;

namespace AppSettingsManagerApi.Infrastructure.Tests;

public class MySqlUserRepositoryTests : IDisposable
{
    private readonly SettingsContext _settingsContext;
    private readonly Fixture _fixture;
    private readonly Mock<IBidirectionalConverter<Model.User, User>> _userConverterMock = new();

    private readonly MySqlUserRepository _userRepository;

    public MySqlUserRepositoryTests()
    {
        _settingsContext = SettingContextBuilder.BuildTestSettingsContext();

        _fixture = FixtureBuilder.BuildFixture();
        _userRepository = new MySqlUserRepository(_settingsContext, _userConverterMock.Object);
    }

    [Fact]
    public void Test1() { }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _settingsContext.Dispose();
    }
}
