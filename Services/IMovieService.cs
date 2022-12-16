using MovieRecommender.Models;

namespace MovieRecommender.Services;

public interface IMovieService
{
    List<Movie> GetTrendingMovies { get; }
    Movie Get(int id);
    IEnumerable<Movie> GetAllMovies();
    string GetModelPath();
    IEnumerable<Movie> GetRecentMovies();
    IEnumerable<Movie> GetSomeSuggestions();
}