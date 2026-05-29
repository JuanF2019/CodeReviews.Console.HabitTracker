using System.Xml.Linq;

namespace HabitTracker.Model
{
    internal class HabitOccurrence (int id, DateTime occurredAt, string? notes, Habit habit)
    {
        public int ID { get; set; } = id;     
        public DateTime OccurredAt { get; set; } = occurredAt;
        public string? Notes { get; set; } = notes;
        public Habit Habit { get; set; } = habit;
        public override string ToString()
        {
            return $"ID = {ID} ; OccurredAt = {OccurredAt} ; Notes = {Notes} ; Habit = {{{Habit}}}";
        }
    }
}
