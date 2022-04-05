namespace Posts
{
    internal class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }

        public override string ToString()
        {
            return $"UserId: {UserId}\nId: {Id}\nTitle: {Title}\nBody: {Body}\n";
        }
    }
}