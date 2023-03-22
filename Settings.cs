// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable IdentifierTypo
#pragma warning disable CS8618
namespace Blog.Backend;

using FluentValidation;


public class SettingsValidator : AbstractValidator<Settings>
{
    public SettingsValidator()
    {
        RuleFor(settings => settings.HygraphSecret).NotEmpty();
        RuleFor(settings => settings.HygraphContentApiUrl).NotEmpty();
        RuleFor(settings => settings.HygraphAssetUploadUrl).NotEmpty();
    }
}

public interface IValidatable
{
    void Validate();
}

public class Settings : IValidatable
{
    public string HygraphSecret { get; set; }
    public string HygraphContentApiUrl { get; set; }
    public string HygraphAssetUploadUrl { get; set; }

    public void Validate()
    {
        new SettingsValidator().Validate(this);
    }
}

