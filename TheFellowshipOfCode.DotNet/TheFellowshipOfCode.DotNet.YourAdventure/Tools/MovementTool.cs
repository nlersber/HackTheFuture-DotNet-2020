﻿using System;
using System.Collections.Generic;
using System.Text;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;

namespace TheFellowshipOfCode.DotNet.YourAdventure.Tools
{
    public class MovementTool
    {
        public static TurnAction Reverse(TurnAction ac)
        {
            switch (ac)
            {
                case TurnAction.WalkSouth: return TurnAction.WalkNorth;
                case TurnAction.WalkNorth: return TurnAction.WalkSouth;
                case TurnAction.WalkEast: return TurnAction.WalkWest;
                case TurnAction.WalkWest: return TurnAction.WalkEast;
                default: return TurnAction.Pass;
            }
        }

        public static Location ApplyMovement(Location old, TurnAction ac)
        {
            var x = old.X;
            var y = old.Y;

            switch (ac)
            {
                case TurnAction.WalkNorth:
                    y++;
                    break;
                case TurnAction.WalkSouth:
                    y--;
                    break;
                case TurnAction.WalkEast:
                    x++;
                    break;
                case TurnAction.WalkWest:
                    x--;
                    break;

            }

            return new Location(x, y);
        }
    }
}
