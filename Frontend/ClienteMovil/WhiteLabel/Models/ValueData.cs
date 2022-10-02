namespace WhiteLabel.Models
{
    public class ValueData
    {
        public ValueData(string label, string value, bool loaderEnabled = false, bool isCompleted = true)
        {
            Label = label;
            Value = value;
            LoaderEnabled = loaderEnabled;
            IsCompleted = isCompleted;
        }

        public string Label { get; }
        public string Value { get; }
        public bool LoaderEnabled { get; set; }
        public bool IsCompleted { get; set; }
    }
}
