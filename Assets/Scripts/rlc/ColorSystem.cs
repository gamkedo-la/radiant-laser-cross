using System.Collections;

namespace rlc
{
    // Families of colors, used to specify if things matches or not, colorwise.
    public enum ColorFamily
    {
        Blacks,
        Whites,
        Greys,
        Reds,
        Yellows,
        Blues,
        Greens,
        Purples,
        Oranges,
    }

    // Functions helping with color matching rules.
    static class ColorSystem
    {
        // Returns true if the colors matches in the game rules, false otherwise.
        static public bool colors_matches(ColorFamily left_color, ColorFamily right_color)
        {
            // Whites matches all colors
            if (left_color == ColorFamily.Whites || right_color == ColorFamily.Whites)
                return true;

            // Blacks matches with no colors, not even blacks.
            if (left_color == ColorFamily.Blacks || right_color == ColorFamily.Blacks)
                return false;


            // Other colors only matches their own familly
            return left_color == right_color;
        }
    }

    // Represent the different groups in the game
    public enum Clan
    {
        none,
        enemy,
        player,
    }


}
