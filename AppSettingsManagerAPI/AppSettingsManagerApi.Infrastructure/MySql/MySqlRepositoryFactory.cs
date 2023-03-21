public class MySqlRepositoryFactory : IRepositoryFactory
{
    private readonly SettingsContext _settingsContext;
    private readonly IBidirectionalConverter<Model.Permission, Permission> _permissionConverter;

    public MySqlRepositoryFactory(
        SettingsContext settingsContext,
        IBidirectionalConverter<Model.Permission, Permission> permissionConverter
    )
    {
        _settingsContext = settingsContext;
        _permissionConverter = permissionConverter;
    }

    public IPermissionRepository CreatePermissionRepository()
    {
        return new MySqlPermissionRepository(_settingsContext, _permissionConverter);
    }
}