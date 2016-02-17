using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade
{
    /// <summary>
    /// The 4 cardinal directions;
    /// North, south, east, west.
    /// </summary>
    public enum DirectionCardinal
    {
        North,
        South,
        East,
        West
    }

    /// <summary>
    /// The 4 intercardinal directions (Directions between cardinal directions);
    /// Northeast, Southeast, Northwest, Southwest.
    /// </summary>
    public enum DirectionIntercardinal
    {
        NorthEast,
        SouthEast,
        NorthWest,
        SouthWest
    }

    /// <summary>
    /// The cardinal AND intercardinal drections.
    /// </summary>
    public enum Directions
    {
        North,
        South,
        East,
        West,
        NorthEast,
        SouthEast,
        NorthWest,
        SouthWest
    }

    /// <summary>
    /// The axis drections;
    /// Horizontal, Vertical.
    /// </summary>
    public enum DirectionAxis
    {
        Horizontal,
        Vertical
    }

    public enum Orentation
    {
        Top,
        Bottom,
        Left,
        Right
    }
    
}
