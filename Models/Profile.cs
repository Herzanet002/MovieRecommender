namespace MovieRecommender.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Name { get; set; }
        public List<(int movieId, int movieRating)> ProfileMovieRatings { get; set; }
    }
}
