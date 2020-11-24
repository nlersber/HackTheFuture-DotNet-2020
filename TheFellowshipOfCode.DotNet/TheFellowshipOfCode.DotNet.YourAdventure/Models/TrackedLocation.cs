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
        public TurnAction Action { get; set; }

        public TrackedLocation(Location loc, TurnAction ac)
        {
            Location = loc;

            Action = ac;
        }
    }
}
