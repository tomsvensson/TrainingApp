namespace TrainingApp.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string Exercise { get; set; } //50 max
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int Weight { get; set; }
    }
}
