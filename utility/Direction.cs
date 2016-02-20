using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade.utility
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

    public static class ParseDirections
    {
        public static Directions ParseStringToDirections(string input)
        {
            string lowerInput = input.ToLower();
            if (lowerInput == "north" || input == "0")
            {
                return Directions.North;
            }
            else if (lowerInput == "south" || input == "1")
            {
                return Directions.South;
            }
            else if (lowerInput == "east" || input == "2")
            {
                return Directions.East;
            }
            else if (lowerInput == "west" || input == "3")
            {
                return Directions.West;
            }

            else if (lowerInput == "northeast" || input == "4")
            {
                return Directions.NorthEast;
            }
            else if (lowerInput == "northwest" || input == "5")
            {
                return Directions.NorthWest;
            }
            else if (lowerInput == "southeast" || input == "6")
            {
                return Directions.SouthEast;
            }
            else if (lowerInput == "southwest" || input == "7")
            {
                return Directions.SouthWest;
            }
            else return Directions.North;
        }
    }
}
