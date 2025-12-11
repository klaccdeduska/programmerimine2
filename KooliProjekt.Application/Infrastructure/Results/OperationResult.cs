using System.Collections.Generic;

namespace KooliProjekt.Application.Infrastructure.Results
{
    public class OperationResult
    {

        public bool HasErrors => Errors.Count > 0;


        public bool Success => !HasErrors;


        public List<string> Errors { get; set; } = new();

        public OperationResult AddError(string error)
        {
            Errors.Add(error);
            return this;
        }

        public OperationResult AddPropertyError(string propertyName, string error)
        {
            Errors.Add($"{propertyName}: {error}");
            return this;
        }
    }
}
