using System;
using System.Collections.Generic;
using System.Text;
using HTF2020.Contracts.Models.Adventurers;
using HTF2020.Contracts.Models.Party;

namespace TheFellowshipOfCode.DotNet.YourAdventure.Tools
{
    public enum DamageType
    {
        Physical,
        Magical,
        Null
    }

    public class DamageTypeConverter
    {
        public static DamageType FromChar(PartyMember member)
        {
            switch (member)
            {
                case Fighter _:
                    return DamageType.Physical;
                case Wizard _:
                    return DamageType.Magical;
                default:
                    return DamageType.Null;
            }
        }
    }
}
