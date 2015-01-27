using System.IO;

namespace Tests
{
    public static class Fixture
    {
        public static string Json(string filename)
        {
            return File.ReadAllText(Path.Combine("fixtures", filename + ".json"));
        }
         
    }
}