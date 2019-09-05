using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FootballShare.Web.Validators
{
    public class BetValidator : IValidatableObject
    {
        /// <summary>
        /// Dollar denomination
        /// </summary>
        public double Denomination { get; set; }
        /// <summary>
        /// Minimum allowed bet
        /// </summary>
        public double MinBet { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            throw new NotImplementedException();
        }
    }
}
