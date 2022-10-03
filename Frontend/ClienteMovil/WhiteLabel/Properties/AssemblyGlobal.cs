namespace WhiteLabel
{
    public static class AssemblyGlobal
    {
        public const string Company = "BBVA";

        public const string ProductLine = "OCR";

        public const string Year = "2022";

        public const string Copyright = Company + " - " + Year;

#if DEBUG
        public const string Configuration = "Debug";
#elif RELEASE
        public const string Configuration = "Release";
#else
        public const string Configuration = "Unkown";
#endif
    }
}
