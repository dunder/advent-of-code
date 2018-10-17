using System.Collections.Generic;
using System.IO;

namespace Solutions
{
    public abstract class ProblemBase {
        public abstract Event Event { get; }
        public abstract Day Day { get; }

        public virtual string FirstStar()
        {
            return "Not implemented yet";

        }

        public virtual string SecondStar()
        {
            return "Not implemented yet";
        }

        private string InputPath => $@".\{Event}\{Day}\input.txt";

        protected string ReadInput()
        {
            return File.ReadAllText(InputPath);
        }

        protected IList<string> ReadLineInput()
        {
            return File.ReadAllLines(InputPath);
        }
    }
}