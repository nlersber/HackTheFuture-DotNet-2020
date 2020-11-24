﻿using System;
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
using TheFellowshipOfCode.DotNet.YourAdventure.Models;
using TheFellowshipOfCode.DotNet.YourAdventure.Tools;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class MyAdventure : IAdventure
    {
        private readonly Random _random = new Random();

        private readonly MovementTracker movetracker = new MovementTracker();

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
                return request.IsCombat ? StrategicCombat() : StrategicNonCombat();
            }

            Task<Turn> StrategicCombat()
            {
                if (HealthChecker.NeedsToChug(request.PartyMember) && request.PossibleActions.Contains(TurnAction.DrinkPotion))
                    return Task.FromResult(new Turn(TurnAction.DrinkPotion));

                return Task.FromResult(new Turn(TurnAction.Attack, Targeter.GetPriorityTarget(request.PossibleTargets)));
            }

            Task<Turn> StrategicNonCombat()
            {
                var movements = MovementTracker.GetMovementActions();
                if (request.PossibleActions.Contains(TurnAction.Loot))
                    return Task.FromResult(new Turn(TurnAction.Loot));

                if (request.PossibleActions.Contains(TurnAction.Open))
                    return Task.FromResult(new Turn(TurnAction.Open));

                var movementoptions =
                    request.PossibleActions.Where(s => movements.Contains(s));

                var direction = movetracker.GetNextDirection(movementoptions.ToList(), request.PartyLocation);
                movetracker.RegisterMove(request.PartyLocation, movementoptions.Count() > 1, direction);


                return Task.FromResult(new Turn(direction));
            }
        }

        class Targeter
        {
            public static Enemy GetPriorityTarget(Enemy[] targets)
            {
                return targets.OrderBy(e => e.CurrentHealthPoints).FirstOrDefault();
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
            public Stack<TrackedLocation> LocationsTracker { get; } = new Stack<TrackedLocation>();
            public Stack<TurnAction> ActionTracker { get; set; } = new Stack<TurnAction>();


            public bool BeenToTarget(Location currentLocation, TurnAction action)
            {
                return LocationsTracker.Any(s => s.Location == MovementTool.ApplyMovement(currentLocation, action));
            }

            public void RegisterMove(Location loc, bool isInter, TurnAction ac)
            {
                LocationsTracker.Push(new TrackedLocation(loc, isInter, ac));
            }

            public static IList<TurnAction> GetMovementActions()
            {
                return new List<TurnAction>() { TurnAction.WalkSouth, TurnAction.WalkNorth, TurnAction.WalkWest, TurnAction.WalkEast };
            }

            public TurnAction GetNextDirection(IList<TurnAction> movements, Location loc)
            {

                TurnAction previous;
                try
                {
                    previous = LocationsTracker.Peek().Action;
                }
                catch (Exception)
                {
                    return movements[new Random().Next(movements.Count)];
                }

                if (movements.Count == 2) //Only going on and going back
                    return previous;//Keep going

                var trimHasBeen = movements.Where(s => !BeenToTarget(loc, s));


                TurnAction dirToGoBias = MovementTool.Leftify(previous);
                if (trimHasBeen.Contains(dirToGoBias))
                    return dirToGoBias;
                do
                {
                    dirToGoBias = MovementTool.Rightify(dirToGoBias);
                } while (!trimHasBeen.Contains(dirToGoBias) && dirToGoBias != previous);

                return dirToGoBias == previous ? MovementTool.Reverse(previous) : dirToGoBias;

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