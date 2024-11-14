using Further.Operation.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Further.Operation.Permissions;

public class OperationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OperationPermissions.GroupName, L("Permission:Operation"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OperationResource>(name);
    }
}
