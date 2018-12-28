namespace ThirtyFiveG.Commons.Extensions
{
    public static class BoolExtensions
    {
        public static string ToString(this bool b, string @true, string @false)
        {
            if (b)
                return @true;
            else
                return @false;
        }
    }
}
