using AutoFixture;

namespace AppSettingsManagerApi.Tests;

public static class FixtureBuilder
{
    public static Fixture BuildFixture()
    {
        var fixture = new Fixture();

        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var customization = new FixtureCustomization();
        customization.Customize(fixture);
        
        return fixture;
    }
}