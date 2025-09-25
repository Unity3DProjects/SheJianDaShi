namespace _ADD
{
    public static class Tools
    {
        public static string T_Replace(string origin, string clear)
        {
            if (origin.Contains(clear))
            {
                return origin.Replace(clear, "");
            }
            return origin;
        }

    }
}