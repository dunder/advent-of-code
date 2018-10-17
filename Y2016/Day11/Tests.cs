using Xunit;

namespace Y2016.Day11 {
    public class Tests {

        [Fact]
        public void Assembly_Moves()
        {
            var floor1 = new Problems.Assembly()
                .WithChip(Problems.Element.Hydrogen)
                .WithChip(Problems.Element.Lithium)
                .WithUpperFloor(new Problems.Assembly());

            

            var moves = floor1.MovableAssemblies();

            
        }
    }
}
