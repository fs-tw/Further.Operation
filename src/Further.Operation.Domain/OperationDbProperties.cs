namespace Further.Operation;

public static class OperationDbProperties
{
    public static string DbTablePrefix { get; set; } = "Operation";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Operation";
}
