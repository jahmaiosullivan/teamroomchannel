using System;

namespace HobbyClue.Common.Helpers
{
    /// <summary>
    /// Sleeper class.
    /// </summary>
    public class Sleeper
    {
        #region "Members"
        private const int CDefaultTimeout = 3; // 3 seconds
        private readonly int m_timeoutInMs;
        private readonly int m_sleepIntervalInMs = CDefaultTimeout;
        private TimeSpan m_totalSleepTimeSpan;
        private readonly SleepingType m_sleepingType = SleepingType.AppendOnlySleepTime;
        private readonly DateTime m_endTime = DateTime.MinValue;

        /// <summary>
        /// Definition of the types of sleep times to track.
        /// </summary>
        public enum SleepingType
        {
            /// <summary>
            /// This will append the time we sleep only; once that time has passed the
            /// timeout, then the sleeper will expire.
            /// 
            /// Ex.
            /// MethodA takes 5 seconds to run and you sleep for 5 on each interation;
            /// then, the sleeper will actually take twice as long to expire.
            /// 
            /// This is the default.
            /// </summary>
            AppendOnlySleepTime,

            /// <summary>
            /// This will calculate the end time before starting (when constructed) and
            /// expire once that end time has been passed.
            /// </summary>
            AppendTotalTime
        }
        #endregion

        #region "Constructors"
        /// <summary>
        /// Constructor taking timespan.
        /// </summary>
        /// <param name="timeout"></param>
        public Sleeper(TimeSpan timeout)
            : this(timeout.Milliseconds)
        {
        }

        /// <summary>
        /// Constructor - this will use SleepingType.AppendOnlySleepTime.
        /// </summary>
        /// <param name="timeoutInMs">Time in seconds for this sleeper will expire</param>
        /// <param name="sleepIntervalInMs">Amount of time to sleep when called</param>
        public Sleeper(int timeoutInMs, int sleepIntervalInMs)
            : this(timeoutInMs, sleepIntervalInMs, SleepingType.AppendOnlySleepTime)
        {
        }

        /// <summary>
        /// Construcotr - this will use SleepingType.AppendOnlySleepTime.  This will also use
        /// the defualt sleep interval of 3 seconds.
        /// </summary>
        /// <param name="timeoutInMs"></param>
        public Sleeper(int timeoutInMs)
            : this(timeoutInMs, CDefaultTimeout, SleepingType.AppendOnlySleepTime)
        {
        }

        /// <summary>
        /// Constructor - this will use SleepingType.AppendOnlySleepTime.
        /// </summary>
        /// <param name="timeoutInMs">Time in seconds for this sleeper will expire</param>
        /// <param name="sleepIntervalInMs">Amount of time to sleep when called</param>
        /// <param name="typeOfSleep"></param>
        public Sleeper(int timeoutInMs, int sleepIntervalInMs, SleepingType typeOfSleep)
        {
            m_timeoutInMs = timeoutInMs;
            m_sleepIntervalInMs = sleepIntervalInMs;
            m_sleepingType = typeOfSleep;

            DateTime mStartTime = DateTime.Now;
            m_endTime = mStartTime.AddMilliseconds(m_timeoutInMs);
        }
        #endregion

        #region "Properties"
        /// <summary>
        /// Return false if the time has not elapsed.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                switch (m_sleepingType)
                {
                    case SleepingType.AppendOnlySleepTime:
                        if (m_totalSleepTimeSpan.TotalMilliseconds >= m_timeoutInMs)
                        {
                            return true;
                        }
                        break;
                    case SleepingType.AppendTotalTime:
                        if (DateTime.Now >= m_endTime)
                        {
                            return true;
                        }
                        break;
                }

                return false;
            }
        }

        /// <summary>
        /// Return true if the time has elapsed.
        /// </summary>
        public bool IsNotExpired
        {
            get
            {
                return !IsExpired;
            }
        }
        #endregion

        #region "Methods"
        /// <summary>
        /// Sleep for a the specified amount of time.
        /// </summary>
        public void Sleep()
        {
            // sleep for specified amount of time
            System.Threading.Thread.Sleep(m_sleepIntervalInMs);

            // add the total sleep time
            m_totalSleepTimeSpan = m_totalSleepTimeSpan.Add(new TimeSpan(0, 0, 0, 0, m_sleepIntervalInMs));
        }

        /// <summary>
        /// Runs the action until the action returns true or timeout.
        /// </summary>
        /// <param name="action">The action to repeat until successful or timeout.</param>
        /// <returns>True if the action succeeded.</returns>
        public bool SleepUntilTrue(Func<bool> action)
        {
            while (IsNotExpired)
            {
                if (action())
                {
                    return true;
                }

                Sleep();
            }

            return false;
        }

        /// <summary>
        /// Just perform a sleep for a specific amount of time.
        /// </summary>
        /// <param name="numberOfMs"></param>
        public static void Delay(int numberOfMs)
        {
            System.Threading.Thread.Sleep(numberOfMs);
        }
        #endregion
    }
}
