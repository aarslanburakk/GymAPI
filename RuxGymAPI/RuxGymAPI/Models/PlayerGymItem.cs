namespace RuxGymAPI.Models
{
    public class PlayerGymItem
    {
        public Guid Id { get; set; }
        public int DumbbellPressItem { get; set; }
        public int AbsItem { get; set; }
        public int SquatItem { get; set; }
        public int DeadLiftItem { get; set; }
        public int BenchPressItem{ get; set; }
        public Guid UserID { get; set; }

    }
}
