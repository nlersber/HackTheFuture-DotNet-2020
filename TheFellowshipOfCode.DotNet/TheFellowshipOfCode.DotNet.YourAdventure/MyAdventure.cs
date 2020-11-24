using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTF2020.Contracts;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Models.Adventurers;
using HTF2020.Contracts.Models.Party;
using HTF2020.Contracts.Requests;
using HTF2020.Contracts.Models.Enemies;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class MyAdventure : IAdventure
    {
        private readonly Random _random = new Random();

        private readonly MovementTracker movetracker = new MovementTracker();
        private readonly IntersectionTracker intertracker = new IntersectionTracker();

        public Task<Party> CreateParty(CreatePartyRequest request)
        {
            var party = new Party
            {
                Name = "My Party",
                Members = new List<PartyMember>()
            };

            for (var i = 0; i < request.MembersCount; i++)
            {

                if (i % 2 == 0)
                    party.Members.Add(new Fighter()
                    {
                        Id = i,
                        Name = $"FighterMan {i + 1}",
                        Constitution = 11,
                        Strength = 15,
                        Intelligence = 8
                    });
                else
                    party.Members.Add(new Wizard()
                    {
                        Id = i,
                        Name = $"Tim {i + 1}",
                        Constitution = 11,
                        Strength = 8,
                        Intelligence = 15
                    });
            }

            return Task.FromResult(party);
        }

        public Task<Turn> PlayTurn(PlayTurnRequest request)
        {

            return Strategic();
            //return PlayToEnd();

            //Task<Turn> PlayToEnd()
            //{
            //    return Task.FromResult(request.PossibleActions.Contains(TurnAction.WalkSouth) ? new Turn(TurnAction.WalkSouth) : new Turn(request.PossibleActions[_random.Next(request.PossibleActions.Length)]));
            //}

            Task<Turn> Strategic()
            {

                if (HealthChecker.NeedsToChug(request.PartyMember) && request.PossibleActions.Contains(TurnAction.DrinkPotion))
                    return Task.FromResult(new Turn(TurnAction.DrinkPotion));

                return request.IsCombat ? StrategicCombat() : StrategicNonCombat();

            }

            Task<Turn> StrategicCombat()
            {
                return Task.FromResult(new Turn(TurnAction.Attack, Targeter.GetPriorityTarget(request.PossibleTargets)));
            }

            Task<Turn> StrategicNonCombat()
            {
                if (request.PossibleActions.Contains(TurnAction.Loot))
                    return Task.FromResult(new Turn(TurnAction.Loot));

                if (request.PossibleActions.Contains(TurnAction.Open))
                    return Task.FromResult(new Turn(TurnAction.Open));

                return Task.FromResult(new Turn(TurnAction.WalkSouth));
            }
        }

        class Targeter
        {
            public static Enemy GetPriorityTarget(Enemy[] targets)
            {
                //TODO get them best target to BRUTALLY MURDER
                return targets.FirstOrDefault();
            }
        }

        class HealthChecker
        {

            public int PotionsCount { get; set; }

            public bool IsOutOfPotions()
            {
                return PotionsCount == 0;
            }
            public static bool NeedsToChug(PartyMember member)
            {
                return member.CurrentHealthPoints + 10 <= member.HealthPoints ||
                       member.CurrentHealthPoints <= (member.HealthPoints / 4);
            }
        }

        class MovementTracker
        {
            public Stack<TurnAction> Actions { get; } = new Stack<TurnAction>();


            public void RegisterMove(TurnAction action)
            {
                Actions.Push(action);
            }

            public TurnAction AlwaysGoLeft()
            {
                switch (Actions.Peek())
                {
                    default: return TurnAction.WalkSouth;
                }
            }


        }

        class IntersectionTracker
        {
            public Stack<Location> Intersections { get; } = new Stack<Location>();

            public void RegisterIntersection(Location loc)
            {
                Intersections.Push(loc);
            }

            public Location RemoveIntersection()
            {
                return Intersections.Pop();
            }

            public Location Peek()
            {
                return Intersections.Peek();
            }
        }






    }
}





//const double goingEastBias = 0.35;
//const double goingSouthBias = 0.25;
//if (request.PossibleActions.Contains(TurnAction.Loot))
//{
//    return Task.FromResult(new Turn(TurnAction.Loot));
//}

//if (request.PossibleActions.Contains(TurnAction.Attack))
//{
//    return Task.FromResult(new Turn(TurnAction.Attack));
//}

//if (request.PossibleActions.Contains(TurnAction.WalkEast) && _random.NextDouble() > (1 - goingEastBias))
//{
//    return Task.FromResult(new Turn(TurnAction.WalkEast));
//}

//if (request.PossibleActions.Contains(TurnAction.WalkSouth) && _random.NextDouble() > (1 - goingSouthBias))
//{
//    return Task.FromResult(new Turn(TurnAction.WalkSouth));
//}

//return Task.FromResult(new Turn(request.PossibleActions[_random.Next(request.PossibleActions.Length)]));