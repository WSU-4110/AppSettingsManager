using System.Text.Json;
using AppSettingsManagerApi.Domain.Conversion;
using AppSettingsManagerApi.Domain.MySql;
using AppSettingsManagerApi.Model.Requests;
using Microsoft.EntityFrameworkCore;

namespace AppSettingsManagerApi.Infrastructure.MySql;


    interface ISettingRepositoryFactory
    {
        private ISettingRepository CreateSettingRepository();
    }
    // Creates SettingsContext object
    private readonly SettingsContext _settingsContext;
    private readonly IBidirectionalConverter<Model.Setting, Setting> _settingsConverter;
    private readonly IBidirectionalConverter<Model.SettingGroup, SettingGroup> _settingGroupConverter;

    public MySqlSettingRepositoryFactory implements ISettingRepositoryFactory(
        private ISettingRepository CreateSettingRepository();
        SettingsContext settingsContext,
        IBidirectionalConverter<Model.Setting, Setting> settingsConverter, 
        IBidirectionalConverter<Model.SettingGroup, SettingGroup> settingGroupConverter
    )
    {
        private ISettingRepository CreateSettingRepository();
        // Injects SettingsContext configured in ServiceConfiguration.cs into _settingsContext object
        _settingsContext = settingsContext;
        // Injects biverter that we set up into _settingsConverter object
        _settingsConverter = settingsConverter;
        //Injects group biverter that we set up into _settingsGroupConverter object
        _settingGroupConverter = settingGroupConverter;

    }

    interface ISettingRepository
    {
        public async Task<Model.SettingGroup> GetSettingGroup(string settingGroupId);
        public async Task<IEnumerable<Model.SettingGroup>> GetSettingGroupsByUser(string userId);
        public async Task<Model.SettingGroup> CreateSetting(CreateSettingRequest request);
        public async Task<Model.SettingGroup> CreateSettingGroup(string settingGroupId, strubg CreatedBy);
        public async Task<Model.SettingGroup> ChangeTargetSettingVersion(UpdateTargetSettingRequest request);
        public async Task<IEnumerable<Model.Setting>> DeleteSetting(string settingGroupId);
        private async Task<SettingGroup> GetSettingGroupFromContext(string settingGroupId)


    }

    public class MYSQLSettingRepository implements ISettingRepository
    {
     public async Task<Model.SettingGroup> GetSettingGroup(string settingGroupId)
        {
            
            var settingGroup = await GetSettingGroupFromContext(settingGroupId);
        

         return _settingGroupConverter.Convert(settingGroup);
        }

        public async Task<IEnumerable<Model.SettingGroup>> GetSettingGroupsByUser(string userId)
        {
            var settingGroups = await _settingsContext.Permissions.Include(p => p.SettingGroup).ThenInclude(sg => sg.Settings)
                .Where(p => p.UserId == userId).Select(p => p.SettingGroup).ToListAsync();
        
         return setingGroups.Select(_settingGroupConverter.Convert);
    
     }

        public async Task<Model.SettingGroup> CreateSetting(CreateSettingRequest request)
        {
            var settingGroup = await GetSettingGroupFromContext(request.SettingGroupId);
            var initialSetting = !settingGroup.Settings.Any();
            var newVersionNumber = !initialSetting ? settingGroup.Settings.Select(s => s.Version).Max() + 1 : 1;
        
            var setting = new Setting
            {
            
             SettingGroupId = request.SettingGroupId,
                Version = newVersionNumber;
                Input = JsonSerializer.Serialize(request.Input),
                IsCurrent = initialSetting,
                CreatedBy = request.UserId,
                CreatedAt = DateTimeOffset.UtcNow
            };

         // You can also use .AddAsync but awaiting both of these operations seems like overkill
         _settingsContext.Settings.Add(setting);
         await _settingsContext.SaveChangesAsync();

            // Actually retrieves the new setting from the table to confirm it's there
            return _settingGroupConverter.Convert(await GetSettingGroupFromContext(request.SettingGroupId));
        }

    
        public async Task<Model.SettingGroup> CreateSettingGroup(string settingGroupId, strubg CreatedBy)
        {
            var settingGroup = new SettingGroup
         {
                Id = settingGroupId, CreatedBy = createdBy
            };

            _settingsContext.Add(settingGroup);

            await _settingsContext.SaveChangesAsync();  

            return _settingGroupConverter.Convert(settingGroup);
        }

        public async Task<Model.SettingGroup> ChangeTargetSettingVersion(UpdateTargetSettingRequest request)
         {
          var settingGroup = await GetSettingGroupFromContext(request.SettingGroupId);
          var currentSetting = settingGroup.Settings.Single(s=> s.IsCurrent);
          var newCurrentSetting = settingGroup.Settings.Single(s=> s.Version == request.NewCurrentVersion);

         currentSetting.IsCurrent = false;
         newCurrentSetting.IsCurrent = true;

         await _settingsContext.SaveChangesAsync();
         return _settingGroupConverter.Convert(settingGroup);
    
         }

     // Not even sure what the use-case is for this other than the user deciding not to use the service anymore
     // Still a good method to have available though, and good for demonstrating that we have full control of the data
     //
        // Also assuming for now that we'd only want to delete the full group of settings (all versions)
     public async Task<IEnumerable<Model.Setting>> DeleteSetting(string settingGroupId)
     {
           var settingGroup = await GetSettingGroupFromContext(settingGroupId);
            _settingsContext.SettingGroups.Remove(settingGroup);
           await _settingGroupConverter.Convert(settingGroup);


            return _settingGroupConverter.Convert(settingGroup);
        }

    private async Task<SettingGroup> GetSettingGroupFromContext(string settingGroupId) =>
            await _settingsContext.SettingGroups.Include(sg => sg.Settings).Include(sg => sg.Permissions)
                .SingleAsync(sg => sg.Id == settingGroupId);

     }
