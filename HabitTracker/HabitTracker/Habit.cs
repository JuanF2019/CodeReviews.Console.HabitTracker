namespace HabitTracker
{
    internal class Habit (int id, string name, DateTime dateAndTime)
    {
        public int ID { get; set; } = id;
        public string Name { get; set; } = name;
        public DateTime DateAndTime { get; set; } = dateAndTime;

        public override string ToString()
        {
            return $"id = {ID} ; Name = {Name} ; DateAndTime = {DateAndTime.ToString()}";
        }
    }
}
