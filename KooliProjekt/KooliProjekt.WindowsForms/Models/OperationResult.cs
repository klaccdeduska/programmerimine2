namespace KooliProjekt.WindowsForms.Models
{
    public class OperationResult
    {
        public List<string> Errors { get; set; } = new();
        public Dictionary<string, List<string>> PropertyErrors { get; set; } = new();

        public bool HasErrors => Errors.Any() || PropertyErrors.Any();
        public bool Success => !HasErrors;

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddPropertyError(string propertyName, string error)
        {
            if (!PropertyErrors.ContainsKey(propertyName))
            {
                PropertyErrors[propertyName] = new List<string>();
            }

            PropertyErrors[propertyName].Add(error);
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Value { get; set; }
    }
}