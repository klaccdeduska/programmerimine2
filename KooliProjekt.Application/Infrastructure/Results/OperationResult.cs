using System.Collections.Generic;

namespace KooliProjekt.Application.Infrastructure.Results
{
    public class OperationResult
    {
        /// <summary>
        /// Есть ли ошибки
        /// </summary>
        public bool HasErrors => Errors.Count > 0;

        /// <summary>
        /// Успешно ли выполнено (нет ошибок)
        /// </summary>
        public bool Success => !HasErrors;

        /// <summary>
        /// Список ошибок (простым текстом, можно и property: message)
        /// </summary>
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
