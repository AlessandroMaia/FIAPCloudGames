namespace SharedKernel;

public static class ConstraintEnumExtension
{
    public  static string GetEnumValuesForCheckConstraint<TEnum>() where TEnum : Enum
    {
        return string.Join(", ", Enum.GetNames(typeof(TEnum)).Select(name => $"'{name}'"));
    }
}
