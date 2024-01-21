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
            CatColor.Grey => new Color(165, 170, 204),
            CatColor.Blue => new Color(115, 122, 170),
            CatColor.Lilac => new Color(204, 165, 168),
            CatColor.Cream => new Color(255, 236, 201),
            CatColor.Red => new Color(255, 218, 201),
            CatColor.Chocolate => new Color(102, 42, 49),
            _ => Color.clear,
        };
    }

    public static Color GetColor(ProjectColor color)
    {
        return color switch
        {
            ProjectColor.Yellow => new Color(255, 242, 201),
            ProjectColor.Green => new Color(201, 255, 213),
            ProjectColor.Violet => new Color(201, 209, 255),
            ProjectColor.Pink => new Color(255, 201, 208),
            ProjectColor.DarkViolet => new Color(116, 124, 171),
            _ => Color.clear,
        };
    }
}