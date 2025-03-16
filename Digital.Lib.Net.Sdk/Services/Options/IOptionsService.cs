namespace Digital.Lib.Net.Sdk.Services.Options;

public interface IOptionsService
{
    public void SettingsInit();
    public T Get<T>(OptionAccessor optionAccessor) where T : notnull;
}