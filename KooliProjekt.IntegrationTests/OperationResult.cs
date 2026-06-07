using System.Collections.Generic;

namespace KooliProjekt.IntegrationTests
{
    public class OperationResult
    {
        public List<string> Errors { get; set; } = new();
        public Dictionary<string, List<string>> PropertyErrors { get; set; } = new();

        public bool HasErrors => Errors.Count > 0 || PropertyErrors.Count > 0;
    }

    public class OperationResult<T> : OperationResult
    {
        public T Value { get; set; }
    }
}