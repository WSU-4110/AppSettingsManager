namespace AppSettingsManagerApi.Domain.Conversion;

public interface IBidirectionalConverter<TSource, TDestination>
{
    TDestination Convert(TSource source);
    TSource Convert(TDestination source);
}
