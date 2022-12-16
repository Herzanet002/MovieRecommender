using MovieRecommender.Models;

namespace MovieRecommender.Services;

public interface IProfileService
{
    List<Profile> GetProfiles { get; }
    Profile GetProfileById(int id);

    List<(int movieId, int movieRating)> GetProfileWatchedMovies(int id);
}