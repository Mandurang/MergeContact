namespace MergeContact.Interefaces
{
    public interface IFuzzyComparer
    {
        bool AreSimilar(string value1, string value2, int threshold);
    }
}
