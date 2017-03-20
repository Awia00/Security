namespace Common.Models
{
    public class Comment
    {
        public string Text { get; set; }

        // Relations
        public User User { get; set; }
        public Image Image { get; set; }
    }
}
