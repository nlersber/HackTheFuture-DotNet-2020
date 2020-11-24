using System;
using System.Collections.Generic;
using System.Text;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;

namespace TheFellowshipOfCode.DotNet.YourAdventure.Models
{
    public class TrackedLocation
    {
        public Location Location { get; set; }
        public bool IsIntersection { get; set; }
        public TurnAction Action { get; set; }

        public TrackedLocation(Location loc, bool inter, TurnAction ac)
        {
            Location = loc;
            IsIntersection = inter;
            Action = ac;
        }
    }
}
