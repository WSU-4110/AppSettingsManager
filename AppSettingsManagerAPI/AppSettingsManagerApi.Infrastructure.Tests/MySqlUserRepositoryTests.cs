using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.Exceptions;
using AppSettingsManagerApi.Infrastructure.MySql;
using AppSettingsManagerApi.Model.Requests;
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

    [Theory]
    [AutoTestData]
    public async Task AuthenticateUser_Succeeds(string userId, string password)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = userId;
        user.Password = password;

        _settingsContext.Users.Add(user);
        await _settingsContext.SaveChangesAsync();

        var authenticate = await _userRepository.AuthenticateUser(userId, password);

        Assert.True(authenticate);
    }

    [Theory]
    [AutoTestData]
    public async Task AuthenticateUser_Fails(string userId, string password)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = userId;
        user.Password = password;

        _settingsContext.Users.Add(user);
        await _settingsContext.SaveChangesAsync();

        var authenticate = await _userRepository.AuthenticateUser(userId, password + "1");

        Assert.False(authenticate);
    }

    [Theory]
    [AutoTestData]
    public async Task CreateUser_Succeeds(CreateUserRequest request)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = request.UserId;
        user.Password = request.Password;
        user.Email = request.Email;

        var expectedUser = _fixture.Build<Model.User>().Create();

        _userConverterMock
            .Setup(c => c.Convert(It.Is<User>(u => u.Id == request.UserId)))
            .Returns(expectedUser);

        var actualUser = await _userRepository.CreateUser(request);

        Assert.Equal(expectedUser, actualUser);
    }

    [Theory]
    [AutoTestData]
    public async Task CreateUser_Fails(CreateUserRequest request)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = request.UserId;
        user.Password = request.Password;
        user.Email = request.Email;

        _settingsContext.Users.Add(user);
        await _settingsContext.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _userRepository.CreateUser(request)
        );
    }

    [Theory]
    [AutoTestData]
    public async Task UpdateUserPassword_Succeeds(UpdateUserPasswordRequest request)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = request.UserId;
        user.Password = request.OldPassword;

        _settingsContext.Users.Add(user);
        await _settingsContext.SaveChangesAsync();

        var expectedUser = _fixture.Build<Model.User>().Create();

        _userConverterMock
            .Setup(c => c.Convert(It.Is<User>(u => u.Id == request.UserId)))
            .Returns(expectedUser);

        var actualUser = await _userRepository.UpdateUserPassword(request);

        Assert.Equal(expectedUser, actualUser);
    }
    
    [Theory]
    [AutoTestData]
    public async Task UpdateUserPassword_Fails(UpdateUserPasswordRequest request)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = request.UserId;
        user.Password = request.OldPassword + "1";

        _settingsContext.Users.Add(user);
        await _settingsContext.SaveChangesAsync();

        await Assert.ThrowsAsync<IncorrectPasswordException>(
            () => _userRepository.UpdateUserPassword(request)
        );
    }
    
    [Theory]
    [AutoTestData]
    public async Task DeleteUser_Succeeds(DeleteUserRequest request)
    {
        var user = _fixture.Build<User>().Create();
        user.Id = request.UserId;
        user.Password = request.Password;

        _settingsContext.Users.Add(user);
        await _settingsContext.SaveChangesAsync();

        await _userRepository.DeleteUser(request);
        
        var actualUser = await _settingsContext.Users.FindAsync(request.UserId);

        Assert.Null(actualUser);
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _settingsContext.Dispose();
    }
}