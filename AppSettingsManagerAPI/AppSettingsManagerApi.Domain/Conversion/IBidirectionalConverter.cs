namespace AppSettingsManagerApi.Domain.Conversion;

public interface IBidirectionalConverter<TSource, TDestination>
{
    TDestination Convert(TSource input);
    TSource Convert(TDestination input);
}
