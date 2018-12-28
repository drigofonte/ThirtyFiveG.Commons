using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThirtyFiveG.Commons.Tel
{
    public class TelProtocol
    {
        private const string _defaultInternationalPrefix = "00";

        private string _prefix;
        private string[] _internationalPrefixes = new string[] { "+", _defaultInternationalPrefix };
        private DialingCodeCollection _dialingCodes;

        #region Constructor
        public TelProtocol(string defaultInternationalPrefix = _defaultInternationalPrefix) : this(string.Empty, defaultInternationalPrefix) { }
        public TelProtocol(string prefix, string defaultInternationalPrefix = _defaultInternationalPrefix) : this(prefix, string.Empty, defaultInternationalPrefix)
        {
            DialingCode code = _dialingCodes.DialingCodes.SingleOrDefault(c => c.Matches(RegionInfo.CurrentRegion.TwoLetterISORegionName));
            if (code != null)
                DefaultCountryCode = code.Code;
        }
        public TelProtocol(string prefix, string defaultCountryCode, string defaultInternationalPrefix = _defaultInternationalPrefix)
        {
            Initialise(prefix, defaultCountryCode, defaultInternationalPrefix);
        }
        #endregion

        #region Public properties
        public string DefaultInternationalPrefix { get; private set; }
        public string DefaultCountryCode { get; private set; }
        #endregion

        #region Private methods
        private void Initialise(string prefix, string defaultCountryCode, string defaultInternationalPrefix)
        {
            _prefix = prefix;
            DefaultCountryCode = defaultCountryCode;
            DefaultInternationalPrefix = defaultInternationalPrefix;
            _dialingCodes = new DialingCodeCollection();
        }

        private string RemoveLeadingZero(string number)
        {
            string numberWithNoLeadingZero = number;
            if (number.StartsWith("0"))
                numberWithNoLeadingZero = number.Substring(1);
            return numberWithNoLeadingZero;
        }
        #endregion

        #region Public methods
        public Uri GetUri(string number)
        {
            return new Uri("tel:" + GetNumber(number));
        }

        public string GetNumber(string number)
        {
            if (!_internationalPrefixes.Any(p => number.StartsWith(p)))
            {
                // The number does not start with an international prefix. Prepend the prefixes as needed
                number = _prefix + DefaultInternationalPrefix + DefaultCountryCode + RemoveLeadingZero(number);
            }
            else
            {
                // The number starts with an international prefix. Extract the international country dialing code, and prepend with the given prefix as needed.
                string internationalPrefix = _internationalPrefixes.Single(p => number.StartsWith(p));
                string localNumber = number.Substring(internationalPrefix.Length);
                DialingCode code = _dialingCodes.MatchDialingCode(localNumber);
                string dialingCode = string.Empty;
                if (code != null)
                    dialingCode = code.Code;
                number = _prefix + DefaultInternationalPrefix + dialingCode + RemoveLeadingZero(localNumber.Substring(dialingCode.Length));
            }

            return Regex.Replace(number, @"\s+", "");
        }
        #endregion
    }
}
