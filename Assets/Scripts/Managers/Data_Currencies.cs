using System;

[Serializable]
public class Data_Currencies
{
    public int Index;
    public int Amount;

    public Data_Currencies(Currency currency)
    {
        Index = (int)currency;
        Amount = 0;
    }
}