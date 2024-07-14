namespace Bilingual.Compiler.Extensions
{
    public static class ExtensionMethods
    {
        /// <summary>Gets items in list that are not in list a.</summary>
        public static List<T> GetAdded<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // https://stackoverflow.com/a/39001823
            return b.Except(a).ToList();
        }

        /// <summary>Get the items removed from list a.</summary>
        public static List<T> GetRemoved<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // https://stackoverflow.com/a/39001823
            return a.Except(b).ToList();
        }
    }
}
