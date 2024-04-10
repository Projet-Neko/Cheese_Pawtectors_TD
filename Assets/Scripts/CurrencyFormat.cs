public static class CurrencyFormat
{
    public static string Get(double currency)
    {
        string[] suffixes = { "", "k", "m", "b", "t" }; // Suffixes pour les milliers, millions, milliards, billions, etc.

        int index = 0;
        while (currency >= 1e3 && index < suffixes.Length - 1)
        {
            currency /= 1e3;
            index++;
        }

        string formattedCurrency = currency.ToString("0.#");

        return $"{formattedCurrency}{suffixes[index]}";
    }
}