using UnityEngine;

public enum CatColor
{
    White, Black, Grey, Blue, Lilac, Cream, Red, Chocolate
}

public enum ProjectColor
{
    Yellow, Green, Violet, Pink, DarkViolet
}

public static class ColorPalette
{
    public static Color GetColor(CatColor color)
    {
        return color switch
        {
            CatColor.White => Color.white,
            CatColor.Black => Color.black,
            CatColor.Grey => new Color32(165, 170, 204, 255),
            CatColor.Blue => new Color32(115, 122, 170, 255),
            CatColor.Lilac => new Color32(204, 165, 168, 255),
            CatColor.Cream => new Color32(255, 236, 201, 255),
            CatColor.Red => new Color32(255, 218, 201, 255),
            CatColor.Chocolate => new Color32(102, 42, 49, 255),
            _ => Color.clear,
        };
    }

    public static Color GetColor(ProjectColor color)
    {
        return color switch
        {
            ProjectColor.Yellow => new Color32(255, 242, 201, 255),
            ProjectColor.Green => new Color32(201, 255, 213, 255),
            ProjectColor.Violet => new Color32(201, 209, 255, 255),
            ProjectColor.Pink => new Color32(255, 201, 208, 255),
            ProjectColor.DarkViolet => new Color32(116, 124, 171, 255),
            _ => Color.clear,
        };
    }
}