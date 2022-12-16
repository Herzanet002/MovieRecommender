using MovieRecommender.Models;

namespace MovieRecommender.Services
{
    public class MovieService : IMovieService
    {
        public readonly static int MoviesToRecommend = 6;
        private readonly static int TrendingMoviesCount = 20;
        public Lazy<List<Movie>> Movies = new Lazy<List<Movie>>(LoadMovieData);
        private List<Movie> _trendingMovies = LoadTrendingMovies();
        public readonly static string Modelpath = @"model.zip";
        private readonly IWebHostEnvironment _hostingEnvironment;

        public List<Movie> GetTrendingMovies => LoadTrendingMovies();

        public MovieService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public static List<Movie> LoadTrendingMovies()
        {
            var result = new List<Movie>();

            result.Add(new Movie { Id = 1573, Name = "Face/Off (1997)" });
            result.Add(new Movie { Id = 1721, Name = "Titanic (1997)" });
            result.Add(new Movie { Id = 1703, Name = "Home Alone 3 (1997)" });
            result.Add(new Movie { Id = 49272, Name = "Casino Royale (2006)" });
            result.Add(new Movie { Id = 5816, Name = "Harry Potter and the Chamber of Secrets (2002)" });
            result.Add(new Movie { Id = 3578, Name = "Gladiator (2000)" });
            return result;
        }

        public string GetModelPath()
        {
            return Path.Combine(_hostingEnvironment.ContentRootPath, "Models", Modelpath);
        }

        public IEnumerable<Movie> GetSomeSuggestions()
        {
            var movies = GetRecentMovies().ToArray();

            var rnd = new Random();
            var movieselector = new int[MoviesToRecommend];

            for (var i = 0; i < MoviesToRecommend; i++)
            {
                movieselector[i] = rnd.Next(movies.Length);
            }

            return movieselector.Select(s => movies[s]);
        }

        public IEnumerable<Movie> GetRecentMovies()
        {
            return GetAllMovies()
                .Where(m => m.Name.Contains("20")
                            || m.Name.Contains("198")
                            || m.Name.Contains("199"));
        }

        public Movie Get(int id)
        {
            return Movies.Value.Single(m => m.Id == id);
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return Movies.Value;
        }

        private static List<Movie> LoadMovieData()
        {
            var result = new List<Movie>();

            var fileReader = File.OpenRead("Content/movies.csv");

            var reader = new StreamReader(fileReader);
            try
            {
                var header = true;
                var index = 0;
                var line = "";
                while (!reader.EndOfStream)
                {
                    if (header)
                    {
                        line = reader.ReadLine();
                        header = false;
                    }
                    line = reader.ReadLine();
                    var fields = line.Split(',');
                    var movieId = int.Parse(fields[0].TrimStart(new char[] { '0' }));
                    var movieName = fields[1];
                    result.Add(new Movie() { Id = movieId, Name = movieName });
                    index++;
                }
            }
            finally
            {
                reader?.Dispose();
            }

            return result;
        }
    }
}