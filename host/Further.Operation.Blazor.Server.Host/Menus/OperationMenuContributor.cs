using System.Threading.Tasks;
using Further.Operation.Localization;
using Volo.Abp.Identity.Pro.Blazor.Navigation;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.UI.Navigation;
using Volo.Saas.Host.Blazor.Navigation;

namespace Further.Operation.Blazor.Server.Host.Menus;

public class OperationMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<OperationResource>();

        context.Menu.SetSubItemOrder(SaasHostMenus.GroupName, 2);


        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 4;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityProMenus.GroupName, 1);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 2);

        return Task.CompletedTask;
    }
}
