using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// The unique identifier for the prize.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The numeric identifier for the place.
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// The name for the place
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// The fixed amount this place earns (zero if not used).
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// The number that represents the percentage (fraction of 1) of the overall take (zero if not used).
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            int.TryParse(placeNumber, out int placeNumberValue);
            PlaceNumber = placeNumberValue;

            decimal.TryParse(prizeAmount, out decimal prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            double.TryParse(prizePercentage, out double prizePercentageValue);
            PrizePercentage = prizePercentageValue;
        }
    }
}
