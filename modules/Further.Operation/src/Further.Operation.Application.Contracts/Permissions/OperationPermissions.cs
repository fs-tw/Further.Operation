using Volo.Abp.Reflection;

namespace Further.Operation.Permissions;

public class OperationPermissions
{
    public const string GroupName = "Operation";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(OperationPermissions));
    }
}
