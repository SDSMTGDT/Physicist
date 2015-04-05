namespace Physicist.Extensions
{
    using System;
    using Microsoft.Xna.Framework;

    public enum PhysicistColor
    {
        AliceBlue,
        AntiqueWhite,
        Aqua,
        Aquamarine,
        Azure,
        Beige,
        Bisque,
        Black,
        BlanchedAlmond,
        Blue,
        BlueViolet,
        Brown,
        BurlyWood,
        CadetBlue,
        Chartreuse,
        Chocolate,
        Coral,
        CornflowerBlue,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cornsilk", Justification = "Xna Spelling")]
        Cornsilk,
        Crimson,
        Cyan,
        DarkBlue,
        DarkCyan,
        DarkGoldenrod,
        DarkGray,
        DarkGreen,
        DarkKhaki,
        DarkMagenta,
        DarkOliveGreen,
        DarkOrange,
        DarkOrchid,
        DarkRed,
        DarkSalmon,
        DarkSeaGreen,
        DarkSlateBlue,
        DarkSlateGray,
        DarkTurquoise,
        DarkViolet,
        DeepPink,
        DeepSkyBlue,
        DimGray,
        DodgerBlue,
        Firebrick,
        FloralWhite,
        ForestGreen,
        Fuchsia,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gainsboro", Justification = "Xna Spelling")]
        Gainsboro,
        GhostWhite,
        Gold,
        Goldenrod,
        Gray,
        Green,
        GreenYellow,
        Honeydew,
        HotPink,
        IndianRed,
        Indigo,
        Ivory,
        Khaki,
        Lavender,
        LavenderBlush,
        LawnGreen,
        LemonChiffon,
        LightBlue,
        LightCoral,
        LightCyan,
        LightGoldenrodYellow,
        LightGray,
        LightGreen,
        LightPink,
        LightSalmon,
        LightSeaGreen,
        LightSkyBlue,
        LightSlateGray,
        LightSteelBlue,
        LightYellow,
        Lime,
        LimeGreen,
        Linen,
        Magenta,
        Maroon,
        MediumAquamarine,
        MediumBlue,
        MediumOrchid,
        MediumPurple,
        MediumSeaGreen,
        MediumSlateBlue,
        MediumSpringGreen,
        MediumTurquoise,
        MediumVioletRed,
        MidnightBlue,
        MintCream,
        MistyRose,
        Moccasin,
        NavajoWhite,
        Navy,
        OldLace,
        Olive,
        OliveDrab,
        Orange,
        OrangeRed,
        Orchid,
        PaleGoldenrod,
        PaleGreen,
        PaleTurquoise,
        PaleVioletRed,
        PapayaWhip,
        PeachPuff,
        Peru,
        Pink,
        Plum,
        PowderBlue,
        Purple,
        Red,
        RosyBrown,
        RoyalBlue,
        SaddleBrown,
        Salmon,
        SandyBrown,
        SeaGreen,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SeaShell", Justification = "Xna Spelling")]
        SeaShell,
        Sienna,
        Silver,
        SkyBlue,
        SlateBlue,
        SlateGray,
        Snow,
        SpringGreen,
        SteelBlue,
        Tan,
        Teal,
        Thistle,
        Tomato,
        Transparent,
        TransparentBlack,
        Turquoise,
        Violet,
        Wheat,
        White,
        WhiteSmoke,
        Yellow,
        YellowGreen,
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = "Xna Color System Provides no casting support")]
    public static class ColorHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Xna Color System Provides no casting support")] 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = "Xna Color System Provides no casting support")]
        public static PhysicistColor ToPhysicistColor(this Color color)
        {
            PhysicistColor value = PhysicistColor.White;
            if (color == Color.AliceBlue)
            {
               value = PhysicistColor.AliceBlue;
            }
            else if (color == Color.AntiqueWhite)
            {
               value = PhysicistColor.AntiqueWhite;
            }
            else if (color == Color.Aqua)
            {
               value = PhysicistColor.Aqua;
            }
            else if (color == Color.Aquamarine)
            {
               value = PhysicistColor.Aquamarine;
            }
            else if (color == Color.Azure)
            {
               value = PhysicistColor.Azure;
            }
            else if (color == Color.Beige)
            {
               value = PhysicistColor.Beige;
            }
            else if (color == Color.Bisque)
            {
               value = PhysicistColor.Bisque;
            }
            else if (color == Color.Black)
            {
               value = PhysicistColor.Black;
            }
            else if (color == Color.BlanchedAlmond)
            {
               value = PhysicistColor.BlanchedAlmond;
            }
            else if (color == Color.Blue)
            {
               value = PhysicistColor.Blue;
            }
            else if (color == Color.BlueViolet)
            {
               value = PhysicistColor.BlueViolet;
            }
            else if (color == Color.Brown)
            {
               value = PhysicistColor.Brown;
            }
            else if (color == Color.BurlyWood)
            {
               value = PhysicistColor.BurlyWood;
            }
            else if (color == Color.CadetBlue)
            {
               value = PhysicistColor.CadetBlue;
            }
            else if (color == Color.Chartreuse)
            {
               value = PhysicistColor.Chartreuse;
            }
            else if (color == Color.Chocolate)
            {
               value = PhysicistColor.Chocolate;
            }
            else if (color == Color.Coral)
            {
               value = PhysicistColor.Coral;
            }
            else if (color == Color.CornflowerBlue)
            {
               value = PhysicistColor.CornflowerBlue;
            }
            else if (color == Color.Cornsilk)
            {
               value = PhysicistColor.Cornsilk;
            }
            else if (color == Color.Crimson)
            {
               value = PhysicistColor.Crimson;
            }
            else if (color == Color.Cyan)
            {
               value = PhysicistColor.Cyan;
            }
            else if (color == Color.DarkBlue)
            {
               value = PhysicistColor.DarkBlue;
            }
            else if (color == Color.DarkCyan)
            {
               value = PhysicistColor.DarkCyan;
            }
            else if (color == Color.DarkGoldenrod)
            {
               value = PhysicistColor.DarkGoldenrod;
            }
            else if (color == Color.DarkGray)
            {
               value = PhysicistColor.DarkGray;
            }
            else if (color == Color.DarkGreen)
            {
               value = PhysicistColor.DarkGreen;
            }
            else if (color == Color.DarkKhaki)
            {
               value = PhysicistColor.DarkKhaki;
            }
            else if (color == Color.DarkMagenta)
            {
               value = PhysicistColor.DarkMagenta;
            }
            else if (color == Color.DarkOliveGreen)
            {
               value = PhysicistColor.DarkOliveGreen;
            }
            else if (color == Color.DarkOrange)
            {
               value = PhysicistColor.DarkOrange;
            }
            else if (color == Color.DarkOrchid)
            {
               value = PhysicistColor.DarkOrchid;
            }
            else if (color == Color.DarkRed)
            {
               value = PhysicistColor.DarkRed;
            }
            else if (color == Color.DarkSalmon)
            {
               value = PhysicistColor.DarkSalmon;
            }
            else if (color == Color.DarkSeaGreen)
            {
               value = PhysicistColor.DarkSeaGreen;
            }
            else if (color == Color.DarkSlateBlue)
            {
               value = PhysicistColor.DarkSlateBlue;
            }
            else if (color == Color.DarkSlateGray)
            {
               value = PhysicistColor.DarkSlateGray;
            }
            else if (color == Color.DarkTurquoise)
            {
               value = PhysicistColor.DarkTurquoise;
            }
            else if (color == Color.DarkViolet)
            {
               value = PhysicistColor.DarkViolet;
            }
            else if (color == Color.DeepPink)
            {
               value = PhysicistColor.DeepPink;
            }
            else if (color == Color.DeepSkyBlue)
            {
               value = PhysicistColor.DeepSkyBlue;
            }
            else if (color == Color.DimGray)
            {
               value = PhysicistColor.DimGray;
            }
            else if (color == Color.DodgerBlue)
            {
               value = PhysicistColor.DodgerBlue;
            }
            else if (color == Color.Firebrick)
            {
               value = PhysicistColor.Firebrick;
            }
            else if (color == Color.FloralWhite)
            {
               value = PhysicistColor.FloralWhite;
            }
            else if (color == Color.ForestGreen)
            {
               value = PhysicistColor.ForestGreen;
            }
            else if (color == Color.Fuchsia)
            {
               value = PhysicistColor.Fuchsia;
            }
            else if (color == Color.Gainsboro)
            {
               value = PhysicistColor.Gainsboro;
            }
            else if (color == Color.GhostWhite)
            {
               value = PhysicistColor.GhostWhite;
            }
            else if (color == Color.Gold)
            {
               value = PhysicistColor.Gold;
            }
            else if (color == Color.Goldenrod)
            {
               value = PhysicistColor.Goldenrod;
            }
            else if (color == Color.Gray)
            {
               value = PhysicistColor.Gray;
            }
            else if (color == Color.Green)
            {
               value = PhysicistColor.Green;
            }
            else if (color == Color.GreenYellow)
            {
               value = PhysicistColor.GreenYellow;
            }
            else if (color == Color.Honeydew)
            {
               value = PhysicistColor.Honeydew;
            }
            else if (color == Color.HotPink)
            {
               value = PhysicistColor.HotPink;
            }
            else if (color == Color.IndianRed)
            {
               value = PhysicistColor.IndianRed;
            }
            else if (color == Color.Indigo)
            {
               value = PhysicistColor.Indigo;
            }
            else if (color == Color.Ivory)
            {
               value = PhysicistColor.Ivory;
            }
            else if (color == Color.Khaki)
            {
               value = PhysicistColor.Khaki;
            }
            else if (color == Color.Lavender)
            {
               value = PhysicistColor.Lavender;
            }
            else if (color == Color.LavenderBlush)
            {
               value = PhysicistColor.LavenderBlush;
            }
            else if (color == Color.LawnGreen)
            {
               value = PhysicistColor.LawnGreen;
            }
            else if (color == Color.LemonChiffon)
            {
               value = PhysicistColor.LemonChiffon;
            }
            else if (color == Color.LightBlue)
            {
               value = PhysicistColor.LightBlue;
            }
            else if (color == Color.LightCoral)
            {
               value = PhysicistColor.LightCoral;
            }
            else if (color == Color.LightCyan)
            {
               value = PhysicistColor.LightCyan;
            }
            else if (color == Color.LightGoldenrodYellow)
            {
               value = PhysicistColor.LightGoldenrodYellow;
            }
            else if (color == Color.LightGray)
            {
               value = PhysicistColor.LightGray;
            }
            else if (color == Color.LightGreen)
            {
               value = PhysicistColor.LightGreen;
            }
            else if (color == Color.LightPink)
            {
               value = PhysicistColor.LightPink;
            }
            else if (color == Color.LightSalmon)
            {
               value = PhysicistColor.LightSalmon;
            }
            else if (color == Color.LightSeaGreen)
            {
               value = PhysicistColor.LightSeaGreen;
            }
            else if (color == Color.LightSkyBlue)
            {
               value = PhysicistColor.LightSkyBlue;
            }
            else if (color == Color.LightSlateGray)
            {
               value = PhysicistColor.LightSlateGray;
            }
            else if (color == Color.LightSteelBlue)
            {
               value = PhysicistColor.LightSteelBlue;
            }
            else if (color == Color.LightYellow)
            {
               value = PhysicistColor.LightYellow;
            }
            else if (color == Color.Lime)
            {
               value = PhysicistColor.Lime;
            }
            else if (color == Color.LimeGreen)
            {
               value = PhysicistColor.LimeGreen;
            }
            else if (color == Color.Linen)
            {
               value = PhysicistColor.Linen;
            }
            else if (color == Color.Magenta)
            {
               value = PhysicistColor.Magenta;
            }
            else if (color == Color.Maroon)
            {
               value = PhysicistColor.Maroon;
            }
            else if (color == Color.MediumAquamarine)
            {
               value = PhysicistColor.MediumAquamarine;
            }
            else if (color == Color.MediumBlue)
            {
               value = PhysicistColor.MediumBlue;
            }
            else if (color == Color.MediumOrchid)
            {
               value = PhysicistColor.MediumOrchid;
            }
            else if (color == Color.MediumPurple)
            {
               value = PhysicistColor.MediumPurple;
            }
            else if (color == Color.MediumSeaGreen)
            {
               value = PhysicistColor.MediumSeaGreen;
            }
            else if (color == Color.MediumSlateBlue)
            {
               value = PhysicistColor.MediumSlateBlue;
            }
            else if (color == Color.MediumSpringGreen)
            {
               value = PhysicistColor.MediumSpringGreen;
            }
            else if (color == Color.MediumTurquoise)
            {
               value = PhysicistColor.MediumTurquoise;
            }
            else if (color == Color.MediumVioletRed)
            {
               value = PhysicistColor.MediumVioletRed;
            }
            else if (color == Color.MidnightBlue)
            {
               value = PhysicistColor.MidnightBlue;
            }
            else if (color == Color.MintCream)
            {
               value = PhysicistColor.MintCream;
            }
            else if (color == Color.MistyRose)
            {
               value = PhysicistColor.MistyRose;
            }
            else if (color == Color.Moccasin)
            {
               value = PhysicistColor.Moccasin;
            }
            else if (color == Color.NavajoWhite)
            {
               value = PhysicistColor.NavajoWhite;
            }
            else if (color == Color.Navy)
            {
               value = PhysicistColor.Navy;
            }
            else if (color == Color.OldLace)
            {
               value = PhysicistColor.OldLace;
            }
            else if (color == Color.Olive)
            {
               value = PhysicistColor.Olive;
            }
            else if (color == Color.OliveDrab)
            {
               value = PhysicistColor.OliveDrab;
            }
            else if (color == Color.Orange)
            {
               value = PhysicistColor.Orange;
            }
            else if (color == Color.OrangeRed)
            {
               value = PhysicistColor.OrangeRed;
            }
            else if (color == Color.Orchid)
            {
               value = PhysicistColor.Orchid;
            }
            else if (color == Color.PaleGoldenrod)
            {
               value = PhysicistColor.PaleGoldenrod;
            }
            else if (color == Color.PaleGreen)
            {
               value = PhysicistColor.PaleGreen;
            }
            else if (color == Color.PaleTurquoise)
            {
               value = PhysicistColor.PaleTurquoise;
            }
            else if (color == Color.PaleVioletRed)
            {
               value = PhysicistColor.PaleVioletRed;
            }
            else if (color == Color.PapayaWhip)
            {
               value = PhysicistColor.PapayaWhip;
            }
            else if (color == Color.PeachPuff)
            {
               value = PhysicistColor.PeachPuff;
            }
            else if (color == Color.Peru)
            {
               value = PhysicistColor.Peru;
            }
            else if (color == Color.Pink)
            {
               value = PhysicistColor.Pink;
            }
            else if (color == Color.Plum)
            {
               value = PhysicistColor.Plum;
            }
            else if (color == Color.PowderBlue)
            {
               value = PhysicistColor.PowderBlue;
            }
            else if (color == Color.Purple)
            {
               value = PhysicistColor.Purple;
            }
            else if (color == Color.Red)
            {
               value = PhysicistColor.Red;
            }
            else if (color == Color.RosyBrown)
            {
               value = PhysicistColor.RosyBrown;
            }
            else if (color == Color.RoyalBlue)
            {
               value = PhysicistColor.RoyalBlue;
            }
            else if (color == Color.SaddleBrown)
            {
               value = PhysicistColor.SaddleBrown;
            }
            else if (color == Color.Salmon)
            {
               value = PhysicistColor.Salmon;
            }
            else if (color == Color.SandyBrown)
            {
               value = PhysicistColor.SandyBrown;
            }
            else if (color == Color.SeaGreen)
            {
               value = PhysicistColor.SeaGreen;
            }
            else if (color == Color.SeaShell)
            {
               value = PhysicistColor.SeaShell;
            }
            else if (color == Color.Sienna)
            {
               value = PhysicistColor.Sienna;
            }
            else if (color == Color.Silver)
            {
               value = PhysicistColor.Silver;
            }
            else if (color == Color.SkyBlue)
            {
               value = PhysicistColor.SkyBlue;
            }
            else if (color == Color.SlateBlue)
            {
               value = PhysicistColor.SlateBlue;
            }
            else if (color == Color.SlateGray)
            {
               value = PhysicistColor.SlateGray;
            }
            else if (color == Color.Snow)
            {
               value = PhysicistColor.Snow;
            }
            else if (color == Color.SpringGreen)
            {
               value = PhysicistColor.SpringGreen;
            }
            else if (color == Color.SteelBlue)
            {
               value = PhysicistColor.SteelBlue;
            }
            else if (color == Color.Tan)
            {
               value = PhysicistColor.Tan;
            }
            else if (color == Color.Teal)
            {
               value = PhysicistColor.Teal;
            }
            else if (color == Color.Thistle)
            {
               value = PhysicistColor.Thistle;
            }
            else if (color == Color.Tomato)
            {
               value = PhysicistColor.Tomato;
            }
            else if (color == Color.Transparent)
            {
               value = PhysicistColor.Transparent;
            }
            else if (color == Color.TransparentBlack)
            {
               value = PhysicistColor.TransparentBlack;
            }
            else if (color == Color.Turquoise)
            {
               value = PhysicistColor.Turquoise;
            }
            else if (color == Color.Violet)
            {
               value = PhysicistColor.Violet;
            }
            else if (color == Color.Wheat)
            {
               value = PhysicistColor.Wheat;
            }
            else if (color == Color.White)
            {
               value = PhysicistColor.White;
            }
            else if (color == Color.WhiteSmoke)
            {
               value = PhysicistColor.WhiteSmoke;
            }
            else if (color == Color.Yellow)
            {
               value = PhysicistColor.Yellow;
            }
            else if (color == Color.YellowGreen)
            {
               value = PhysicistColor.YellowGreen;
            }

            return value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Xna Color System Provides no casting support")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = "Xna Color System Provides no casting support")]
        public static Color ToXnaColor(this PhysicistColor color)
        {
            Color value = Color.White;
            switch (color)
            {
               case PhysicistColor.AliceBlue:
                    value = Color.AliceBlue;
                    break;
               case PhysicistColor.AntiqueWhite:
                    value = Color.AntiqueWhite;
                    break;
               case PhysicistColor.Aqua:
                    value = Color.Aqua;
                    break;
               case PhysicistColor.Aquamarine:
                    value = Color.Aquamarine;
                    break;
               case PhysicistColor.Azure:
                    value = Color.Azure;
                    break;
               case PhysicistColor.Beige:
                    value = Color.Beige;
                    break;
               case PhysicistColor.Bisque:
                    value = Color.Bisque;
                    break;
               case PhysicistColor.Black:
                    value = Color.Black;
                    break;
               case PhysicistColor.BlanchedAlmond:
                    value = Color.BlanchedAlmond;
                    break;
               case PhysicistColor.Blue:
                    value = Color.Blue;
                    break;
               case PhysicistColor.BlueViolet:
                    value = Color.BlueViolet;
                    break;
               case PhysicistColor.Brown:
                    value = Color.Brown;
                    break;
               case PhysicistColor.BurlyWood:
                    value = Color.BurlyWood;
                    break;
               case PhysicistColor.CadetBlue:
                    value = Color.CadetBlue;
                    break;
               case PhysicistColor.Chartreuse:
                    value = Color.Chartreuse;
                    break;
               case PhysicistColor.Chocolate:
                    value = Color.Chocolate;
                    break;
               case PhysicistColor.Coral:
                    value = Color.Coral;
                    break;
               case PhysicistColor.CornflowerBlue:
                    value = Color.CornflowerBlue;
                    break;
               case PhysicistColor.Cornsilk:
                    value = Color.Cornsilk;
                    break;
               case PhysicistColor.Crimson:
                    value = Color.Crimson;
                    break;
               case PhysicistColor.Cyan:
                    value = Color.Cyan;
                    break;
               case PhysicistColor.DarkBlue:
                    value = Color.DarkBlue;
                    break;
               case PhysicistColor.DarkCyan:
                    value = Color.DarkCyan;
                    break;
               case PhysicistColor.DarkGoldenrod:
                    value = Color.DarkGoldenrod;
                    break;
               case PhysicistColor.DarkGray:
                    value = Color.DarkGray;
                    break;
               case PhysicistColor.DarkGreen:
                    value = Color.DarkGreen;
                    break;
               case PhysicistColor.DarkKhaki:
                    value = Color.DarkKhaki;
                    break;
               case PhysicistColor.DarkMagenta:
                    value = Color.DarkMagenta;
                    break;
               case PhysicistColor.DarkOliveGreen:
                    value = Color.DarkOliveGreen;
                    break;
               case PhysicistColor.DarkOrange:
                    value = Color.DarkOrange;
                    break;
               case PhysicistColor.DarkOrchid:
                    value = Color.DarkOrchid;
                    break;
               case PhysicistColor.DarkRed:
                    value = Color.DarkRed;
                    break;
               case PhysicistColor.DarkSalmon:
                    value = Color.DarkSalmon;
                    break;
               case PhysicistColor.DarkSeaGreen:
                    value = Color.DarkSeaGreen;
                    break;
               case PhysicistColor.DarkSlateBlue:
                    value = Color.DarkSlateBlue;
                    break;
               case PhysicistColor.DarkSlateGray:
                    value = Color.DarkSlateGray;
                    break;
               case PhysicistColor.DarkTurquoise:
                    value = Color.DarkTurquoise;
                    break;
               case PhysicistColor.DarkViolet:
                    value = Color.DarkViolet;
                    break;
               case PhysicistColor.DeepPink:
                    value = Color.DeepPink;
                    break;
               case PhysicistColor.DeepSkyBlue:
                    value = Color.DeepSkyBlue;
                    break;
               case PhysicistColor.DimGray:
                    value = Color.DimGray;
                    break;
               case PhysicistColor.DodgerBlue:
                    value = Color.DodgerBlue;
                    break;
               case PhysicistColor.Firebrick:
                    value = Color.Firebrick;
                    break;
               case PhysicistColor.FloralWhite:
                    value = Color.FloralWhite;
                    break;
               case PhysicistColor.ForestGreen:
                    value = Color.ForestGreen;
                    break;
               case PhysicistColor.Fuchsia:
                    value = Color.Fuchsia;
                    break;
               case PhysicistColor.Gainsboro:
                    value = Color.Gainsboro;
                    break;
               case PhysicistColor.GhostWhite:
                    value = Color.GhostWhite;
                    break;
               case PhysicistColor.Gold:
                    value = Color.Gold;
                    break;
               case PhysicistColor.Goldenrod:
                    value = Color.Goldenrod;
                    break;
               case PhysicistColor.Gray:
                    value = Color.Gray;
                    break;
               case PhysicistColor.Green:
                    value = Color.Green;
                    break;
               case PhysicistColor.GreenYellow:
                    value = Color.GreenYellow;
                    break;
               case PhysicistColor.Honeydew:
                    value = Color.Honeydew;
                    break;
               case PhysicistColor.HotPink:
                    value = Color.HotPink;
                    break;
               case PhysicistColor.IndianRed:
                    value = Color.IndianRed;
                    break;
               case PhysicistColor.Indigo:
                    value = Color.Indigo;
                    break;
               case PhysicistColor.Ivory:
                    value = Color.Ivory;
                    break;
               case PhysicistColor.Khaki:
                    value = Color.Khaki;
                    break;
               case PhysicistColor.Lavender:
                    value = Color.Lavender;
                    break;
               case PhysicistColor.LavenderBlush:
                    value = Color.LavenderBlush;
                    break;
               case PhysicistColor.LawnGreen:
                    value = Color.LawnGreen;
                    break;
               case PhysicistColor.LemonChiffon:
                    value = Color.LemonChiffon;
                    break;
               case PhysicistColor.LightBlue:
                    value = Color.LightBlue;
                    break;
               case PhysicistColor.LightCoral:
                    value = Color.LightCoral;
                    break;
               case PhysicistColor.LightCyan:
                    value = Color.LightCyan;
                    break;
               case PhysicistColor.LightGoldenrodYellow:
                    value = Color.LightGoldenrodYellow;
                    break;
               case PhysicistColor.LightGray:
                    value = Color.LightGray;
                    break;
               case PhysicistColor.LightGreen:
                    value = Color.LightGreen;
                    break;
               case PhysicistColor.LightPink:
                    value = Color.LightPink;
                    break;
               case PhysicistColor.LightSalmon:
                    value = Color.LightSalmon;
                    break;
               case PhysicistColor.LightSeaGreen:
                    value = Color.LightSeaGreen;
                    break;
               case PhysicistColor.LightSkyBlue:
                    value = Color.LightSkyBlue;
                    break;
               case PhysicistColor.LightSlateGray:
                    value = Color.LightSlateGray;
                    break;
               case PhysicistColor.LightSteelBlue:
                    value = Color.LightSteelBlue;
                    break;
               case PhysicistColor.LightYellow:
                    value = Color.LightYellow;
                    break;
               case PhysicistColor.Lime:
                    value = Color.Lime;
                    break;
               case PhysicistColor.LimeGreen:
                    value = Color.LimeGreen;
                    break;
               case PhysicistColor.Linen:
                    value = Color.Linen;
                    break;
               case PhysicistColor.Magenta:
                    value = Color.Magenta;
                    break;
               case PhysicistColor.Maroon:
                    value = Color.Maroon;
                    break;
               case PhysicistColor.MediumAquamarine:
                    value = Color.MediumAquamarine;
                    break;
               case PhysicistColor.MediumBlue:
                    value = Color.MediumBlue;
                    break;
               case PhysicistColor.MediumOrchid:
                    value = Color.MediumOrchid;
                    break;
               case PhysicistColor.MediumPurple:
                    value = Color.MediumPurple;
                    break;
               case PhysicistColor.MediumSeaGreen:
                    value = Color.MediumSeaGreen;
                    break;
               case PhysicistColor.MediumSlateBlue:
                    value = Color.MediumSlateBlue;
                    break;
               case PhysicistColor.MediumSpringGreen:
                    value = Color.MediumSpringGreen;
                    break;
               case PhysicistColor.MediumTurquoise:
                    value = Color.MediumTurquoise;
                    break;
               case PhysicistColor.MediumVioletRed:
                    value = Color.MediumVioletRed;
                    break;
               case PhysicistColor.MidnightBlue:
                    value = Color.MidnightBlue;
                    break;
               case PhysicistColor.MintCream:
                    value = Color.MintCream;
                    break;
               case PhysicistColor.MistyRose:
                    value = Color.MistyRose;
                    break;
               case PhysicistColor.Moccasin:
                    value = Color.Moccasin;
                    break;
               case PhysicistColor.NavajoWhite:
                    value = Color.NavajoWhite;
                    break;
               case PhysicistColor.Navy:
                    value = Color.Navy;
                    break;
               case PhysicistColor.OldLace:
                    value = Color.OldLace;
                    break;
               case PhysicistColor.Olive:
                    value = Color.Olive;
                    break;
               case PhysicistColor.OliveDrab:
                    value = Color.OliveDrab;
                    break;
               case PhysicistColor.Orange:
                    value = Color.Orange;
                    break;
               case PhysicistColor.OrangeRed:
                    value = Color.OrangeRed;
                    break;
               case PhysicistColor.Orchid:
                    value = Color.Orchid;
                    break;
               case PhysicistColor.PaleGoldenrod:
                    value = Color.PaleGoldenrod;
                    break;
               case PhysicistColor.PaleGreen:
                    value = Color.PaleGreen;
                    break;
               case PhysicistColor.PaleTurquoise:
                    value = Color.PaleTurquoise;
                    break;
               case PhysicistColor.PaleVioletRed:
                    value = Color.PaleVioletRed;
                    break;
               case PhysicistColor.PapayaWhip:
                    value = Color.PapayaWhip;
                    break;
               case PhysicistColor.PeachPuff:
                    value = Color.PeachPuff;
                    break;
               case PhysicistColor.Peru:
                    value = Color.Peru;
                    break;
               case PhysicistColor.Pink:
                    value = Color.Pink;
                    break;
               case PhysicistColor.Plum:
                    value = Color.Plum;
                    break;
               case PhysicistColor.PowderBlue:
                    value = Color.PowderBlue;
                    break;
               case PhysicistColor.Purple:
                    value = Color.Purple;
                    break;
               case PhysicistColor.Red:
                    value = Color.Red;
                    break;
               case PhysicistColor.RosyBrown:
                    value = Color.RosyBrown;
                    break;
               case PhysicistColor.RoyalBlue:
                    value = Color.RoyalBlue;
                    break;
               case PhysicistColor.SaddleBrown:
                    value = Color.SaddleBrown;
                    break;
               case PhysicistColor.Salmon:
                    value = Color.Salmon;
                    break;
               case PhysicistColor.SandyBrown:
                    value = Color.SandyBrown;
                    break;
               case PhysicistColor.SeaGreen:
                    value = Color.SeaGreen;
                    break;
               case PhysicistColor.SeaShell:
                    value = Color.SeaShell;
                    break;
               case PhysicistColor.Sienna:
                    value = Color.Sienna;
                    break;
               case PhysicistColor.Silver:
                    value = Color.Silver;
                    break;
               case PhysicistColor.SkyBlue:
                    value = Color.SkyBlue;
                    break;
               case PhysicistColor.SlateBlue:
                    value = Color.SlateBlue;
                    break;
               case PhysicistColor.SlateGray:
                    value = Color.SlateGray;
                    break;
               case PhysicistColor.Snow:
                    value = Color.Snow;
                    break;
               case PhysicistColor.SpringGreen:
                    value = Color.SpringGreen;
                    break;
               case PhysicistColor.SteelBlue:
                    value = Color.SteelBlue;
                    break;
               case PhysicistColor.Tan:
                    value = Color.Tan;
                    break;
               case PhysicistColor.Teal:
                    value = Color.Teal;
                    break;
               case PhysicistColor.Thistle:
                    value = Color.Thistle;
                    break;
               case PhysicistColor.Tomato:
                    value = Color.Tomato;
                    break;
               case PhysicistColor.Transparent:
                    value = Color.Transparent;
                    break;
               case PhysicistColor.TransparentBlack:
                    value = Color.TransparentBlack;
                    break;
               case PhysicistColor.Turquoise:
                    value = Color.Turquoise;
                    break;
               case PhysicistColor.Violet:
                    value = Color.Violet;
                    break;
               case PhysicistColor.Wheat:
                    value = Color.Wheat;
                    break;
               case PhysicistColor.White:
                    value = Color.White;
                    break;
               case PhysicistColor.WhiteSmoke:
                    value = Color.WhiteSmoke;
                    break;
               case PhysicistColor.Yellow:
                    value = Color.Yellow;
                    break;
               case PhysicistColor.YellowGreen:
                    value = Color.YellowGreen;
                    break;
            }

            return value;
        }
    }
}
