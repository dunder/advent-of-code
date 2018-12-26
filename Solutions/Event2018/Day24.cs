using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day24 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day24;
        public string Name => "Immune System Simulator 20XX";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = UnitsInWinningArmy(input, 1, 10, 13, 10);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = UnitsInWinningArmyWithBoost(input, 1, 10, 13, 10);
            return result.ToString();
        }

        public static int UnitsInWinningArmy(IList<string> input, int skip1, int take1, int skip2, int take2)
        {
            var immuneSystem = ParseGroups(input, skip1, take1);
            var infection = ParseGroups(input, skip2, take2);

            var fight = new Fight(immuneSystem, infection);

            return fight.Start();
        }


        public static int UnitsInWinningArmyWithBoost(IList<string> input, int skip1, int take1, int skip2, int take2)
        {
            var boost = 70;

            int units = 0;
            var immuneSystemWins = false;
            while (!immuneSystemWins)
            {
                var immuneSystem = ParseGroups(input, skip1, take1);
                var infection = ParseGroups(input, skip2, take2);

                foreach (var group in immuneSystem)
                {
                    group.Attack.Damage = group.Attack.Damage + boost;
                }
                var fight = new Fight(immuneSystem, infection);
                units = fight.Start();
                immuneSystemWins = fight.ImmuneSystem.Any(g => !g.Removed);
                boost++;
            }

            return units;
        }

        public static List<Group> ParseGroups(IList<string> input, int skip, int take)
        {
            var groups = input.Skip(skip).Take(take).ToList();

            return groups.Select(Group.Parse).ToList();
        }

        public class Group
        {
            private static int groupId = 0;

            public Group(int units, int hitPoints, HashSet<string> weaknesses, HashSet<string> immuneTo, Attack attack)
            {
                Id = groupId++;
                Units = units;
                HitPoints = hitPoints;
                Weaknesses = weaknesses;
                ImmuneTo = immuneTo;
                Attack = attack;
            }

            public static Group Parse(string input)
            {
                var expression = new Regex(@"(\d+) units each with (\d+) hit points(.*)with an attack that does (\d+) (\w+) damage at initiative (\d+)");
                var immuneToExpression = new Regex(@"immune to (.*)(; weak)|immune to (.*)\)");
                var weaknessesExpression = new Regex(@"weak to (.*)(; immune)|weak to (.*)\)");

                var match = expression.Match(input);

                var units = int.Parse(match.Groups[1].Value);
                var hitPoints = int.Parse(match.Groups[2].Value);
                var damage = int.Parse(match.Groups[4].Value);
                var attackType = match.Groups[5].Value;
                var initiative = int.Parse(match.Groups[6].Value);

                var weaknesses = new HashSet<string>();
                if (weaknessesExpression.IsMatch(input))
                {
                    var weaknessesMatch = weaknessesExpression.Match(input);
                    var matchString = weaknessesMatch.Groups[1].Success
                        ? weaknessesMatch.Groups[1]
                        : weaknessesMatch.Groups[3];
                    weaknesses = matchString.Value.Replace(" ", "").Split(",").ToHashSet();
                }

                var immuneTo = new HashSet<string>();

                if (immuneToExpression.IsMatch(input))
                {
                    var immuneToMatch = immuneToExpression.Match(input);
                    var matchString = immuneToMatch.Groups[1].Success
                        ? immuneToMatch.Groups[1]
                        : immuneToMatch.Groups[3];
                    immuneTo = matchString.Value.Replace(" ", "").Split(',').ToHashSet();
                }

                return new Group(units, hitPoints, weaknesses, immuneTo, new Attack(attackType, damage, initiative));
            }

            public int Id { get; set; }
            public Attack Attack { get; }
            public int Units { get; set; }
            public int HitPoints { get; set; }
            public HashSet<string> Weaknesses { get; }
            public HashSet<string> ImmuneTo { get; }
            public int EffectivePower => Units * Attack.Damage;
            public bool Removed => Units <= 0;
            public List<Group> Enemies { get; set; }
            public Group Target { get; set; }

            public void AttackTarget()
            {
                if (!Removed)
                {
                    Target?.TakeAttack(this);
                }
            }

            public Group SelectTarget(HashSet<int> alreadySelected)
            {
                Group target = null;

                var potentialTargets = Enemies
                        .Where(g => !alreadySelected.Contains(g.Id))
                        .Where(g => !g.Removed)
                        .OrderByDescending(g => g.DamageReceivedOnAttack(this))
                        .ThenByDescending(g => g.EffectivePower)
                        .ThenByDescending(g => g.Attack.Initiative)
                        .ToList();

                if (potentialTargets.Any(g => g.DamageReceivedOnAttack(this) > 0))
                {
                    target = potentialTargets.First();
                }

                Target = target;

                return Target;
            }

            public int DamageReceivedOnAttack(Group attackingGroup)
            {

                if (ImmuneTo.Contains(attackingGroup.Attack.Type))
                {
                    return 0;
                }

                var damage = attackingGroup.EffectivePower;

                if (Weaknesses.Contains(attackingGroup.Attack.Type))
                {
                    damage = damage * 2;
                }

                return damage;
            }

            public void TakeAttack(Group attackingGroup)
            {
                if (Removed) return;

                int damage = DamageReceivedOnAttack(attackingGroup);

                if (damage == 0) return;

                int unitsKilled = damage / HitPoints;

                Units = Units - unitsKilled;
            }

            public override string ToString()
            {
                return $"Units: {Units}, Hit points: {HitPoints}, Weaknesses: {string.Join(", ", Weaknesses)} Immune to: {string.Join(", ", ImmuneTo)} Attack: {Attack}";
            }
        }

        public class Attack
        {
            public Attack(string type, int damage, int initiative)
            {
                Type = type;
                Damage = damage;
                Initiative = initiative;
            }

            public string Type { get; set; }
            public int Damage { get; set; }
            public int Initiative { get; set; }

            public override string ToString()
            {
                return $"Type: {Type} Damage: {Damage} Initiative: {Initiative}";
            }
        }

        public class Fight
        {
            private readonly List<Group> immuneSystem;
            private readonly List<Group> infection;

            public Fight(List<Group> immuneSystem, List<Group> infection)
            {
                this.immuneSystem = immuneSystem;
                this.infection = infection;

                foreach (var group in immuneSystem)
                {
                    group.Enemies = infection;
                }

                foreach (var group in infection)
                {
                    group.Enemies = immuneSystem;
                }
            }

            public List<Group> ImmuneSystem => immuneSystem;
            public List<Group> Infection => infection;

            public List<Group> GroupsInTargetSelectionOrder => immuneSystem
                .Concat(infection)
                .Where(g => !g.Removed)
                .OrderByDescending(g => g.EffectivePower)
                .ThenByDescending(g => g.Attack.Initiative)
                .ToList();

            public List<Group> GroupsInAttackingOrder => immuneSystem
                .Concat(infection)
                .Where(g => !g.Removed)
                .OrderByDescending(g => g.Attack.Initiative)
                .ToList();

            public int Start()
            {
                while (immuneSystem.Any(g => !g.Removed) && infection.Any(g => !g.Removed))
                {
                    var alreadySelected = new HashSet<int>();
                    foreach (var group in GroupsInTargetSelectionOrder)
                    {
                        var target = group.SelectTarget(alreadySelected);
                        if (target != null)
                        {
                            alreadySelected.Add(target.Id);
                        }
                    }

                    foreach (var group in GroupsInAttackingOrder)
                    {
                        group.AttackTarget();
                    }
                }

                var immuneSystemPoints = immuneSystem.Where(g => g.Units >= 0).Sum(g => g.Units);
                var infectionPoints = infection.Where(g => g.Units >= 0).Sum(g => g.Units);

                return Math.Max(immuneSystemPoints, infectionPoints);
            }
        }

        [Fact]
        public void FirstStarParseImmuneSystemTest()
        {
            var input = new[]
            {
                "Immune System:",
                "17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2",
                "989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3",
                "",
                "Infection:",
                "801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1",
                "4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4"
            };

            var immuneSystem = ParseGroups(input, 1, 2);
            var infection = ParseGroups(input, 5, 2);

            Assert.Equal(2, immuneSystem.Count);
            Assert.Equal(2, infection.Count);
        }

        [Fact]
        public void FirstStarParseExample1()
        {
            var group = Group.Parse("17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2");

            Assert.Equal(17, group.Units);
            Assert.Equal(5390, group.HitPoints);
            Assert.Contains("radiation", group.Weaknesses);
            Assert.Contains("bludgeoning", group.Weaknesses);
            Assert.Empty(group.ImmuneTo);
            Assert.Equal(4507, group.Attack.Damage);
            Assert.Equal("fire", group.Attack.Type);
            Assert.Equal(2, group.Attack.Initiative);

        }

        [Fact]
        public void FirstStarParseExample2()
        {
            var group = Group.Parse("989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3");

            Assert.Equal(989, group.Units);
            Assert.Equal(1274, group.HitPoints);
            Assert.Contains("bludgeoning", group.Weaknesses);
            Assert.Contains("slashing", group.Weaknesses);
            Assert.Contains("fire", group.ImmuneTo);
            Assert.Equal(25, group.Attack.Damage);
            Assert.Equal("slashing", group.Attack.Type);
            Assert.Equal(3, group.Attack.Initiative);

        }

        [Fact]
        public void FirstStarParseExample3()
        {
            var group = Group.Parse("4033 units each with 10164 hit points (immune to slashing) with an attack that does 22 radiation damage at initiative 5");

            Assert.Equal(4033, group.Units);
            Assert.Equal(10164, group.HitPoints);
            Assert.Empty(group.Weaknesses);
            Assert.Contains("slashing", group.ImmuneTo);
            Assert.Equal(22, group.Attack.Damage);
            Assert.Equal("radiation", group.Attack.Type);
            Assert.Equal(5, group.Attack.Initiative);
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new[]
            {
                "Immune System:",
                "17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2",
                "989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3",
                "",
                "Infection:",
                "801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1",
                "4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4"
            };

            var units = UnitsInWinningArmy(input, 1, 2, 5, 2);

            Assert.Equal(5216, units);
        }
        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("14854", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("3467", actual);
        }
    }
}