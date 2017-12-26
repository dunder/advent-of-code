using Xunit;

namespace Y2017.Day25 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {

            string[] input = {
                "Begin in state A.",
                "Perform a diagnostic checksum after 6 steps.",
                "",
                "In state A:",
                "If the current value is 0:",
                "-Write the value 1.",
                "",
                "- Move one slot to the right.",
                "",
                "- Continue with state B.",
                "If the current value is 1:",
                "-Write the value 0.",
                "",
                "- Move one slot to the left.",
                "",
                "- Continue with state B.",
                "",
                "In state B:",
                "If the current value is 0:",
                "-Write the value 1.",
                "",
                "- Move one slot to the left.",
                "",
                "- Continue with state A.",
                "If the current value is 1:",
                "-Write the value 1.",
                "",
                "- Move one slot to the right.",
                "",
                "- Continue with state A."
            };

            var result = TheHaltingProblem.DiagnosticChecksum(input);

            Assert.Equal(3, result);
        }
    }
}
