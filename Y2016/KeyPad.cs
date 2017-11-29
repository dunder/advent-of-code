using System.Collections.Generic;
using System.Linq;

namespace Y2016 {
    public class KeyPad {
        public static string Crack(List<string> input) {
            var key = NumericKey.K5;
            var code = "";
            foreach (var line in input) {
                var movements = line.Select(c => c.ToMovement());
                foreach (var movement in movements) {
                    key = key.Move(movement);
                }
                code = code + ((int) key);
            }
            return code;
        }
    }
}