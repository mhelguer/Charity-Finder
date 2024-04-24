namespace CharityFinder.Models
{
    public class ThemeModel
    {
        public Themes Themes { get; set; }
    }

    public class Themes
    {
        public List<Theme> Theme { get; set; }
    }

    public class Theme
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

}
