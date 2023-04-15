using System.Text.Json;
using System.Text.Json.Nodes;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Json.Schema;

namespace AppSettingsManagerBff.Infrastructure.Tests;

public sealed class AutoTestData : AutoDataAttribute
{
    public AutoTestData() : base(
        () => new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true })
            .Customize(new FixtureCustomization())
        ){}
}

public sealed class FixtureCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => new Uri("http://www.example.com"));
        fixture.Register<Stream>(() => new MemoryStream(8));
        fixture.Register(() => new MemoryStream(8));
        fixture.Register(() => JsonDocument.Parse("{}"));
        fixture.Register(() => JsonNode.Parse("{}"));
        fixture.Register(() => JsonSerializer.Deserialize<JsonElement>("{}"));
        fixture.Register(() => JsonSchema.True);
    }
}