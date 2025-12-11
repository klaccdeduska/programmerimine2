using System.Collections.Generic;

namespace KooliProjekt.Application.Infrastructure.Results
{
    public class OperationResult<T>
    {
        public bool Success => Errors.Count == 0;
        public List<string> Errors { get; set; } = new();
        public T Value { get; set; }
        }
        }

        public OperationResult AddError(string error)
        {
            if (Errors == null)
            {
                Errors = new List<string>();
            }

            Errors.Add(error);

            return this;
        }

        public OperationResult AddPropertyError(string property, string error)
        {
            if (PropertyErrors == null)
            {
                PropertyErrors = new Dictionary<string, string>();
            }
            }

            PropertyErrors.Add(property, error);

            return this;
        }
    }
}