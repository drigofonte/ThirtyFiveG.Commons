namespace ThirtyFiveG.Commons.Tel
{
    public class DialingCode
    {
        #region Constructor
        public DialingCode(string code, string isoAlpha2Code, string isoAlpha3Code)
        {
            Code = code;
            ISOAlpha2Code = isoAlpha2Code;
            ISOAlpha3Code = isoAlpha3Code;
        }
        #endregion

        #region Public properties
        public string Code { get; private set; }
        public string ISOAlpha2Code { get; private set; }
        public string ISOAlpha3Code { get; private set; }
        #endregion

        #region Public methods
        public bool Matches(string isoAlphaCode)
        {
            return !string.IsNullOrEmpty(isoAlphaCode)
                && ((isoAlphaCode.Length == 2 && isoAlphaCode.Equals(ISOAlpha2Code))
                    || isoAlphaCode.Length == 3 && isoAlphaCode.Equals(ISOAlpha3Code));
        }
        #endregion
    }
}
