using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace Microsoft.IT.Geo.Legacy.GeoCommon
{
	/// <summary>
	/// Provides helper methods, such as method to read various types of value from string.
	/// </summary>
	/// <remarks>Moved from GeoLocationAPI to GeoCommon.</remarks>
	public static class Helper
	{
		/// <summary>
		/// This helper method reads a string and assigns its int value 
		/// or null to a nullable int type
		/// </summary>
		/// <param name="inValue"></param>
		/// <returns></returns>
		public static int? ReadIntValue(string inValue)
		{
			if (string.IsNullOrEmpty(inValue))
				return null;
			else
				return Convert.ToInt32(inValue, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// This helper method reads a string and assigns its int value 
		/// or null to a nullable int type
		/// </summary>
		/// <param name="inValue"></param>
		/// <returns></returns>
		public static double? ReadDoubleValue(string inValue)
		{
			if (string.IsNullOrEmpty(inValue))
				return null;
			else
				return Convert.ToDouble(inValue, CultureInfo.InvariantCulture);
		}


		/// <summary>
		/// This helper method reads a string and assigns its int value 
		/// or null to a nullable int type
		/// </summary>
		/// <param name="inValue"></param>
		/// <returns></returns>
		public static DateTime? ReadDateTimeValue(string inValue)
		{
			if (string.IsNullOrEmpty(inValue))
				return null;
			else
                return Convert.ToDateTime(inValue, CultureInfo.InvariantCulture);

		}


		/// <summary>
		/// This helper funtion substitutes string.Empty for null or trims a string.
		/// </summary>
		/// <param name="inValue"></param>
		/// <returns></returns>
		public static string TrimParameter(string inValue)
		{
			if (inValue == null)
				return string.Empty;
			else
				return inValue.Trim();

		}

        /// <summary>
        /// Compute hash for a given file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ComputerHash(string fileName)
        {
            FileStream fs1 = null;
            MD5CryptoServiceProvider md5 = null;
            try
            {
                fs1 = GeoDataFeedBase.SafeOpenFileForRead(fileName);

                byte[] hash = null;
                //Create the crypto provider
                using (md5 = new MD5CryptoServiceProvider())
                {
                    //Compute the hash for the streams
                    hash = md5.ComputeHash(fs1);
                }
                // The files are not different
                if (hash != null && hash.Length == 0)
                    return null;
                else
                    return hash;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (fs1 != null) fs1.Close();
                if (md5 != null) md5.Clear();
            }
        }

        /// <summary>
        /// Compare two hashs
        /// </summary>
        /// <param name="hash1"></param>
        /// <param name="hash2"></param>
        /// <returns></returns>
        public static bool IsHashSame(byte[] hash1, byte[] hash2)
        {
            //Compare the byte arrays
            if (hash1 == null || hash2 == null)
                return false;
            if (hash1.Length != hash2.Length)
                return false;
            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// /// Get int configure from app.config.
        /// Note: it is not supposed to read configure from geosocket.mscom.config
        /// </summary>
        /// <param name="configureName">Configure name</param>
        /// <param name="max">max value we allow</param>
        /// <param name="min">min value we allow</param>
        /// <param name="defaultValue">default value if it does not appear in configure file. Could be null</param>
        /// <returns></returns>
        public static int GetConfigureInteger(string configureName, int min, int max, int defaultValue)
        {
            string configureValue = ConfigurationManager.AppSettings[configureName];
            return Helper.GetSafeConfigureInteger(configureName, configureValue, min, max, defaultValue);
        }

        /// <summary>
        /// Get string configure from app.config.
        /// Note: it is not supposed to read configure from geosocket.mscom.config
        /// </summary>
        /// <param name="configureName">configure name</param>
        /// <param name="defaultValue">default value if it does not appear in configure file. Could be null</param>
        /// <returns></returns>
        public static string GetConfigureString(string configureName, string defaultValue)
        {
            string configureValue = ConfigurationManager.AppSettings[configureName];
            if (configureValue == null)
                return defaultValue;
            else
                return configureValue;
        }

        /// <summary>
        /// Get safe integer by ensure the min and max range
        /// </summary>
        /// <param name="configureName"></param>
        /// <param name="configureValue"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetSafeConfigureInteger(string configureName, string configureValue, int min, int max, int defaultValue)
        {
            if (String.IsNullOrEmpty(configureValue))
                return defaultValue;
            else
            {
                try
                {
                    int val = Int32.Parse(configureValue, CultureInfo.InvariantCulture);
                    if (val < min || val > max)
                    {
                       //EventLogWriter.WriteEntry(EventType.Configuration, "Helper:Configure entry " + configureName + " is not a valid value. We will use default value.", System.Diagnostics.EventLogEntryType.Error);
                        return defaultValue;
                    }
                    else
                        return val;
                }
                catch (Exception)
                {
                   //EventLogWriter.WriteException(EventType.Configuration, "Helper:Configure entry " + configureName + " is not a valid integer. We will use defaultValue", e);
                    return defaultValue;
                }
            }
        }

	}
}
