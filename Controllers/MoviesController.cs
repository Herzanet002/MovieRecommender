using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using MovieRecommender.DataStructures;
using MovieRecommender.Models;
using MovieRecommender.Services;
using Newtonsoft.Json;

namespace movierecommender.Controllers;

public class MoviesController : Controller
{
    private readonly PredictionEnginePool<MovieRating, MovieRatingPrediction> _model;
    private readonly IMovieService _movieService;
    private readonly IProfileService _profileService;

    public MoviesController(PredictionEnginePool<MovieRating, MovieRatingPrediction> model,
        IMovieService movieService,
        IProfileService profileService)
    {
        _movieService = movieService;
        _profileService = profileService;
        _model = model;
    }

    public ActionResult Choose()
    {
        return View(_movieService.GetSomeSuggestions());
    }

    public ActionResult Recommend(int id)
    {
        var activeprofile = _profileService.GetProfileById(id);

        // 1. Create the ML.NET environment and load the already trained model

        var ratings = new List<(int movieId, float normalizedScore)>();
        var movieRatings = _profileService.GetProfileWatchedMovies(id);
        var watchedMovies = new List<Movie>();

        foreach (var (movieId, _) in movieRatings) watchedMovies.Add(_movieService.Get(movieId));

        foreach (var movie in _movieService.GetTrendingMovies)
        {
            // Call the Rating Prediction for each movie prediction
            var prediction = _model.Predict(new MovieRating
            {
                userId = id.ToString(),
                movieId = movie.Id.ToString()
            });

            // Normalize the prediction scores for the "ratings" b/w 0 - 100
            var normalizedscore = Sigmoid(prediction.Score);

            // Add the score for recommendation of each movie in the trending movie list
            ratings.Add((movie.Id, normalizedscore));
        }

        //3. Provide rating predictions to the view to be displayed
        ViewData["watchedmovies"] = watchedMovies;
        ViewData["ratings"] = ratings;
        ViewData["trendingmovies"] = _movieService.GetTrendingMovies;
        return View(activeprofile);
    }

    public float Sigmoid(float x)
    {
        return (float) (100 / (1 + Math.Exp(-x)));
    }

    public ActionResult Watch()
    {
        return View();
    }

    public ActionResult Profiles()
    {
        var profiles = _profileService.GetProfiles;
        return View(profiles);
    }

    public ActionResult Watched(int id)
    {
        var activeprofile = _profileService.GetProfileById(id);
        var movieRatings = _profileService.GetProfileWatchedMovies(id);
        var watchedMovies = new List<Movie>();

        foreach ((var movieId, float _) in movieRatings) watchedMovies.Add(_movieService.Get(movieId));

        ViewData["watchedmovies"] = watchedMovies;
        ViewData["trendingmovies"] = _movieService.GetTrendingMovies;
        return View(activeprofile);
    }

    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        {
        }
    }
}