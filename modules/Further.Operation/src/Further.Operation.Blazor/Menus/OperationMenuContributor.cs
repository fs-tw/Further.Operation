using System.Threading.Tasks;
using Further.Operation.Localization;
using Volo.Abp.UI.Navigation;

namespace Further.Operation.Blazor.Menus;

public class OperationMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        var l = context.GetLocalizer<OperationResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            OperationMenus.Prefix,
            displayName: l["Menu:Operation"], 
            "/Operation",
            icon: "fa fa-globe"));

        await Task.CompletedTask;
    }
}
