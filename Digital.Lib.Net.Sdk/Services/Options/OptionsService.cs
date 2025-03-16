using Digital.Lib.Net.Core.Extensions.ConfigurationUtilities;
using Digital.Lib.Net.Core.Extensions.EnumUtilities;
using Digital.Lib.Net.Core.Extensions.TypeUtilities;
using Digital.Lib.Net.Entities.Context;
using Digital.Lib.Net.Entities.Models.ApplicationOptions;
using Digital.Lib.Net.Entities.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Digital.Lib.Net.Sdk.Services.Options;

public class OptionsService(
    ILogger<OptionsService> logger,
    IConfiguration configuration,
    IRepository<ApplicationOption, DigitalContext> appOptionRepository
) : IOptionsService
{
    public void SettingsInit()
    {
        foreach (var (appSettingsAccessor, appOptionAccessor, defaultValue) in ApplicationDefaults.Settings)
        {
            var optionAccessor = appOptionAccessor.GetDisplayName();
            var stored = appOptionRepository
                .Get(o => o.Id == optionAccessor)
                .FirstOrDefault();

            if (stored is not null)
                continue;

            var value = configuration.Get<string>(appSettingsAccessor) ?? defaultValue;
            appOptionRepository.CreateAndSave(
                new ApplicationOption
                {
                    Id = optionAccessor,
                    Value = value
                });

            logger.LogInformation($"Setting {optionAccessor} has been saved in database.");
        }
    }

    public T Get<T>(OptionAccessor optionAccessor) where T : notnull
    {
        var stored = appOptionRepository.Get(o => o.Id == optionAccessor.GetDisplayName());
        if (stored is null)
            throw new InvalidOperationException($"Option {optionAccessor} could not be found");

        return TypeConverter.Convert<T>(stored.First().Value);
    }
}