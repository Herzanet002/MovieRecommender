using MovieRecommender.Models;

namespace MovieRecommender.Services;

public class ProfileService : IProfileService
{
    public int Activeprofileid = -1;
    public List<Profile> GetProfiles { get; } = new(LoadProfileData());

    public List<(int movieId, int movieRating)> GetProfileWatchedMovies(int id)
    {
        foreach (var profile in GetProfiles)
            if (id == profile.Id)
                return profile.ProfileMovieRatings;

        return null;
    }

    public Profile GetProfileById(int id)
    {
        foreach (var profile in GetProfiles)
            if (id == profile.Id)
                return profile;

        return null;
    }

    private static List<Profile> LoadProfileData()
    {
        var result = new List<Profile>();

        var fileReader = File.OpenRead("Content/Profiles.csv");
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
                var profileId = int.Parse(fields[0].TrimStart(new[] {'0'}));
                var profileImageName = fields[1];
                var profileName = fields[2];

                var ratings = new List<(int movieId, int movieRating)>();

                for (var i = 3; i < fields.Length; i += 2)
                    ratings.Add((int.Parse(fields[i]), int.Parse(fields[i + 1])));
                result.Add(new Profile
                {
                    Id = profileId,
                    ImageName = profileImageName,
                    Name = profileName,
                    ProfileMovieRatings = ratings
                });
                index++;
            }
        }
        finally
        {
            if (reader != null) reader.Dispose();
        }

        return result;
    }
}