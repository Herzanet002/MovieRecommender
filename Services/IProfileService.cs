using MovieRecommender.Models;

namespace MovieRecommender.Services
{
    public interface IProfileService
    {
        Profile GetProfileById(int id);

        List<(int movieId, int movieRating)> GetProfileWatchedMovies(int id);

        List<Profile> GetProfiles { get; }
    }
}