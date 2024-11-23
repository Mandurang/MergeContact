using FuzzySharp;

namespace MergeContact.Interefaces
{
    public class FuzzyComparer : IFuzzyComparer
    {
        public bool AreSimilar(string value1, string value2, int threshold)
        {
            if (string.IsNullOrEmpty(value1) || string.IsNullOrEmpty(value2))
                return false;

            int similarity = Fuzz.Ratio(value1, value2);
            return similarity >= threshold;
        }
    }
}
