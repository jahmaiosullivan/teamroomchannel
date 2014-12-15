using System;
using System.Threading;

namespace HobbyClue.Common.Helpers
{
    /// <summary>
    /// This class handles smart retry logic.
    /// </summary>
    public static class RetryHelper
    {
        // Date time is only accurate to between 10-15ms, we use this to avoid getting stuck looping.
        private static readonly TimeSpan s_marginOfError = TimeSpan.FromMilliseconds(30);

        /// <summary>
        /// Retries the action until the action returns true or we exhaust the retry count.
        /// </summary>
        /// <param name="maxAttempts">The number of times to try.</param>
        /// <param name="sleepInterval">The interval to sleep between each iteration.</param>
        /// <param name="action">The action to repeat until it returns true or we exhaust the retry count.</param>
        /// <param name="failureAction">The optional action to call if all of the retries fail.</param>
        /// <param name="handleExceptions">If true, exceptions will be caught and only thrown on the last iteration.</param>
        /// <returns>True if the action succeeded, false if retry count is exhausted.</returns>
        /// <exception cref="ArgumentException">Throws if retryCount is less than 2.</exception>
        /// <exception cref="ArgumentNullException">Throws if action is null.</exception>
        /// <remarks>
        /// Will timeout after 1 hour not matter what.
        /// If handleExceptions is true and the last iteration throws, your failure action will be called before we throw the final exception.
        /// </remarks>
        public static bool Retry(int maxAttempts, TimeSpan sleepInterval, Func<bool> action, Action failureAction = null, bool handleExceptions = true)
        {
            return Retry(TimeSpan.FromHours(1), sleepInterval, action, failureAction, handleExceptions, maxAttempts);
        }

        /// <summary>
        /// Repeats the action until the action returns true, timeout, or retries are exhausted.  If an exception is
        /// thrown on the last iteration, this method will throw that exception.
        /// </summary>
        /// <param name="timeout">The total length of time before we timeout (TimeSpan.Zero is infinite).</param>
        /// <param name="sleepInterval">The interval to sleep between each iteration.</param>
        /// <param name="action">The action to repeat until it returns true.</param>
        /// <param name="failureAction">The optional action to call if all of the retries fail.</param>
        /// <param name="handleExceptions">If true, exceptions will be caught and only thrown on the last iteration.</param>
        /// <returns>True if the action succeeded, false if retry count is exhausted.</returns>
        /// <exception cref="ArgumentException">Throws if retryCount is less than 2.</exception>
        /// <exception cref="ArgumentNullException">Throws if action is null.</exception>
        /// <remarks>
        /// If handleExceptions is true and the last iteration throws, your failure action will be called before we throw the final exception.
        /// </remarks>
        public static bool Retry(TimeSpan timeout, TimeSpan sleepInterval, Func<bool> action, Action failureAction = null, bool handleExceptions = true, int maxAttempts = int.MaxValue)
        {
            if(timeout <= TimeSpan.Zero)
            {
                throw new ArgumentException("Timeout value must be positive timespan.", "timeout");
            }

            if(sleepInterval < TimeSpan.Zero)
            {
                throw new ArgumentException("SleepInterval value must be >= zero.", "sleepInterval");
            }

            if (maxAttempts < 2)
            {
                throw new ArgumentException("Max attempts must be larger than 1.", "maxAttempts");
            }

            if (action == null)
            {
                throw new ArgumentNullException("Action must not be null.", "action");
            }

            DateTime start = DateTime.Now;
            DateTime endTime = start.Add(timeout);
            int attemptsRemaining = maxAttempts;

            while (true) // We aren't checking anything here so we can do specific work on the last iteration.
            {
                // Track any exception for this iteration.
                Exception exception = null;
                --attemptsRemaining;

                try
                {
                    if (action())
                    {
                        // Action reported success.
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // Save for later.
                    exception = ex;
                }

                TimeSpan timeLeft = endTime - DateTime.Now;
                
                // Check if it is done now, this avoids sleeping on the last iteration and avoids logging the final exception twice.
                if (attemptsRemaining == 0 || // Used the last attempt.
                    timeLeft <= s_marginOfError || // Past the end time.
                    (!handleExceptions && exception != null)) // Not handling exceptions and we got an exception.
                {
                    // Report failure.
                    if (failureAction != null)
                    {
                        failureAction();
                    }

                    // If an exception was handled this iteration, throw it again.
                    if (exception != null)
                    {
                        throw exception;
                    }

                    return false;
                }

                // Sleep if we are supposed to.
                if (sleepInterval > TimeSpan.Zero && timeLeft > TimeSpan.Zero)
                {
                    // Don't sleep longer than timeLeft.
                    TimeSpan sleepTime = sleepInterval < timeLeft ? sleepInterval : timeLeft;
                    Thread.Sleep(sleepTime);
                }
            }
        }

    }
}
