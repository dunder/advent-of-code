using Xunit;

namespace Y2017.Day07 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {

            var input = new[] {
                "pbga (66)                    ",
                "xhth (57)                    ",
                "ebii (61)                    ",
                "havc (66)                    ",
                "ktlj (57)                    ",
                "fwft (72) -> ktlj, cntj, xhth",
                "qoyq (66)                    ",
                "padx (45) -> pbga, havc, qoyq",
                "tknk (41) -> ugml, padx, fwft",
                "jptl (61)                    ",
                "ugml (68) -> gyxo, ebii, jptl",
                "gyxo (61)                    ",
                "cntj (57)                    "
            };
            var bottom = Towers.BottomDisc(input);

            Assert.Equal("tknk", bottom.Name);
        }

        [Fact]
        public void Problem2_Example1() {
            var input = new[] {
                "pbga (66)                    ",
                "xhth (57)                    ",
                "ebii (61)                    ",
                "havc (66)                    ",
                "ktlj (57)                    ",
                "fwft (72) -> ktlj, cntj, xhth",
                "qoyq (66)                    ",
                "padx (45) -> pbga, havc, qoyq",
                "tknk (41) -> ugml, padx, fwft",
                "jptl (61)                    ",
                "ugml (68) -> gyxo, ebii, jptl",
                "gyxo (61)                    ",
                "cntj (57)                    "
            };
            var unbalanced = Towers.FindUnbalanced(input);

            Assert.Equal(60, unbalanced);
        }
        
    }
}
