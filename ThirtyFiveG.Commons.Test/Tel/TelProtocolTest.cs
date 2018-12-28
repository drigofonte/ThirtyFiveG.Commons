using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ThirtyFiveG.Commons.Tel
{
    [TestClass]
    public class TelProtocolTest
    {
        [TestMethod]
        public void GetUri()
        {
            TelProtocol protocol = new TelProtocol(string.Empty, "44");

            string plus = "+";
            string telephone = "1234567890";
            string number = plus + telephone;
            Uri actual = protocol.GetUri(number);

            Uri expected = new Uri("tel:" + protocol.DefaultInternationalPrefix + telephone);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUri_with_country_code_plus_symbol_with_prefix()
        {
            string prefix = "9";
            TelProtocol protocol = new TelProtocol(prefix, "00");

            string plus = "+";
            string countryCode = "44";
            string telephone = "01234567890";
            string number = plus + countryCode + telephone;
            Uri actual = protocol.GetUri(number);

            Uri expected = new Uri("tel:" + prefix + protocol.DefaultInternationalPrefix + countryCode + telephone.Substring(1));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUri_with_country_code_plus_symbol_without_prefix()
        {
            TelProtocol protocol = new TelProtocol(string.Empty, "00");

            string plus = "+";
            string countryCode = "44";
            string telephone = "01234567890";
            string number = plus + countryCode + telephone;
            Uri actual = protocol.GetUri(number);

            Uri expected = new Uri("tel:" + protocol.DefaultInternationalPrefix + countryCode + telephone.Substring(1));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUri_with_country_code_double_zero_symbol_with_prefix()
        {
            string prefix = "9";
            TelProtocol protocol = new TelProtocol(prefix, "00");

            string countryCode = "0044";
            string telephone = "01234567890";
            string number = countryCode + telephone;
            Uri actual = protocol.GetUri(number);

            Uri expected = new Uri("tel:" + prefix + countryCode + telephone.Substring(1));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUri_with_country_code_double_zero_without_prefix()
        {
            TelProtocol protocol = new TelProtocol(string.Empty, "00");

            string countryCode = "0044";
            string telephone = "01234567890";
            string number = countryCode + telephone;
            Uri actual = protocol.GetUri(number);

            Uri expected = new Uri("tel:" + countryCode + telephone.Substring(1));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUri_without_country_code_with_prefix()
        {
            string prefix = "9";
            TelProtocol protocol = new TelProtocol(prefix, "00");

            string number = "01234567890";
            Uri actual = protocol.GetUri(number);

            Uri expected = new Uri("tel:" + prefix + protocol.DefaultInternationalPrefix + protocol.DefaultCountryCode + number.Substring(1));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUri_without_country_code_without_prefix()
        {
            TelProtocol protocol = new TelProtocol(string.Empty, "00");

            string number = "01234567890";
            Uri actual = protocol.GetUri(number);

            Uri expected = new Uri("tel:" + protocol.DefaultInternationalPrefix + protocol.DefaultCountryCode + number.Substring(1));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUri_without_country_code_without_prefix_without_leading_zero()
        {
            TelProtocol protocol = new TelProtocol(string.Empty, "00");

            string number = "1234567890";
            Uri actual = protocol.GetUri(number);

            Uri expected = new Uri("tel:" + protocol.DefaultInternationalPrefix + protocol.DefaultCountryCode + number);
            Assert.AreEqual(expected, actual);
        }
    }
}
