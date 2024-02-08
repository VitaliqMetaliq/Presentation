namespace Storage.Main.Validation;

internal static class ValidationHelper
{
    public static bool IsDatesInValidRange(DateTime dateFrom, DateTime dateTo)
    {
        return dateFrom <= dateTo;
    }
}