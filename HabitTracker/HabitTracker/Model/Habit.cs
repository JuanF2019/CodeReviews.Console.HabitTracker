namespace HabitTracker.Model
{
    internal class Habit (int id, string name, string? description)
    {
        public int ID { get; set; } = id;
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public override string ToString()
        {
            return $"ID = {ID} ; Name = {Name} ; Description = {Description}";
        }
    }
}
