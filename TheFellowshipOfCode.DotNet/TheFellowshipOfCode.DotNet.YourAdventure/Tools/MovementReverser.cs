using System;
using System.Collections.Generic;
using System.Text;
using HTF2020.Contracts.Enums;

namespace TheFellowshipOfCode.DotNet.YourAdventure.Tools
{
    public class MovementReverser
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
    }
}
