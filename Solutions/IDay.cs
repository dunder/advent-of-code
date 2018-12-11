namespace Solutions
{
    public interface IDay
    {
        Event Event { get; }
        Day Day { get; }
        string Name { get; }

        string FirstStar();
        string SecondStar();
    }
}
