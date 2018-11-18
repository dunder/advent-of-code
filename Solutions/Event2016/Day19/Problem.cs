using System.Linq;

namespace Solutions.Event2016.Day19
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day19;

        private const int InitialNumberOfElves = 3014387;

        public override string FirstStar()
        {
            var result = ElfGame(InitialNumberOfElves);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var result = ElfGameWithOpposites(InitialNumberOfElves);
            return result.ToString();
        }

        public class Elf
        {
            public int Id { get; }
            public Elf Left { get; set; }
            public Elf Right { get; set; }

            public Elf(int id)
            {
                Id = id;
            }

            public override string ToString()
            {
                return $"Elf {Id}";
            }
        }

        public static int ElfGame(int numberOfElves)
        {
            var elves = Enumerable.Range(1, numberOfElves).Select(id => new Elf(id)).ToList();

            for (int i = 0; i < elves.Count - 1; i++)
            {
                elves[i].Left = elves[i + 1];
            }

            elves.Last().Left = elves.First();

            Elf elf = elves.First();
            while (elf.Left != elf)
            {
                elf.Left = elf.Left.Left;
                elf = elf.Left;
            }

            return elf.Id;
        }

        public static int ElfGameWithOpposites(int numberOfElves)
        {
            var elves = Enumerable.Range(1, numberOfElves).Select(id => new Elf(id)).ToList();

            for (int i = 0; i < elves.Count - 1; i++)
            {
                elves[i].Left = elves[i + 1];
                elves[i + 1].Right = elves[i];
            }

            elves.Last().Left = elves.First();
            elves.First().Right = elves.Last();

            var elvesLeft = numberOfElves;

            int DistanceToOpposite()
            {
                return elvesLeft % 2 == 0 ? elvesLeft / 2 - 1 : elvesLeft / 2;
            }

            Elf elfToSteal = elves.First();
            Elf currentOpposite = elves[DistanceToOpposite()];
            while (elfToSteal.Left != elfToSteal)
            {
                currentOpposite.Right.Left = currentOpposite.Left;
                currentOpposite.Left.Right = currentOpposite.Right;

                elfToSteal = elfToSteal.Left;
                currentOpposite = elvesLeft % 2 == 0 ? currentOpposite.Left : currentOpposite.Left.Left;
                elvesLeft--;
            }

            return elfToSteal.Id;
        }
    }
}