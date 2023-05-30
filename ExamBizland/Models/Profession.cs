namespace ExamBizland.Models
{
    public class Profession
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
    }
}
