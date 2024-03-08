using System;

[Serializable]
public class Data_Storage
{
    public int SlotIndex;
    public int CatIndex;

    public Data_Storage(int slotIndex)
    {
        SlotIndex = slotIndex;
        CatIndex = -1;
    }
}