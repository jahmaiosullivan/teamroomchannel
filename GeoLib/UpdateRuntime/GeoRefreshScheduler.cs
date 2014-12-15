using System;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.IT.Geo.UpdateRuntime
{
    //=====================================================================
    //  Class: GeoRefreshScheduler
    //
    /// <summary>
    /// Component responsible for managing the scheduling of the Geo Data
    /// </summary>
    //=====================================================================

    public sealed class GeoRefreshScheduler : IDisposable
    {
        private Timer refreshTimer;

        private GeoRefreshConfiguration configuration;

        private Action<object> refreshCallbackAction;

        private DateTime initialRefreshTime;

        private bool disposed;

        //=====================================================================
        //  Method: GeoRefreshScheduler
        //
        /// <summary>
        /// Default constructor
        /// </summary>
        //=====================================================================

        private GeoRefreshScheduler()
        { 
        }

        //=====================================================================
        //  Method: GeoRefreshScheduler
        //
        /// <summary>
        /// Creates an instance of the GeoRefreshScheduler using configuration
        /// from the app.config file
        /// </summary>
        //=====================================================================

        public static GeoRefreshScheduler Create(
            Action<object> refreshCallback
            )
        {
            GeoRefreshConfiguration configuration = GeoRefreshConfiguration.CreateConfiguration();

            return GeoRefreshScheduler.Create(
                refreshCallback,
                configuration
                );
        }

        //=====================================================================
        //  Method: GeoRefreshScheduler
        //
        /// <summary>
        /// Creates an instance of the GeoRefreshScheduler using specified
        /// configuration options
        /// </summary>
        //=====================================================================

        public static GeoRefreshScheduler Create(
            Action<object> refreshCallback,
            GeoRefreshConfiguration configuration
            )
        {
            GeoRefreshScheduler scheduler = null;

            if (configuration != null)
            {
                if (refreshCallback != null)
                {
                    scheduler = new GeoRefreshScheduler();

                    scheduler.configuration = configuration;

                    scheduler.Initialize(
                        refreshCallback
                        );
                }
                else
                {
                    throw new ArgumentNullException(
                        "refreshCallback",
                        "Cannot initialize Geo Refresh component using a null processing method."
                        );
                }
            }
            else
            {
                throw new ArgumentNullException(
                    "configuration",
                    "Cannot initialize Geo Refresh component using a null configuration."
                    );
            }

            return scheduler;
        }

        //=====================================================================
        //  Method: Start
        //
        /// <summary>
        /// This method starts the scheduler
        /// </summary>
        //=====================================================================

        public void Start()
        {
            this.InitializeRefreshTimer();
        }

        //=====================================================================
        //  Method: Stop
        //
        /// <summary>
        /// This method stops the scheduler
        /// </summary>
        //=====================================================================

        public void Stop()
        {
            this.refreshTimer = null;
        }

        //=====================================================================
        //  Method: Initialize
        //
        /// <summary>
        /// Initialize the internal state of the scheduler
        /// </summary>
        //=====================================================================

        private void Initialize(
            Action<object> refreshCallback
            )
        {
            Debug.Assert(this.configuration != null);

            this.refreshCallbackAction = refreshCallback;
        }

        //=====================================================================
        //  Method: InitializeRefreshTimer
        //
        /// <summary>
        /// Initialize the internal timer used to trigger the refresh procedure
        /// </summary>
        //=====================================================================

        private void InitializeRefreshTimer()
        {
            Debug.Assert(this.configuration != null);

            Debug.Assert(this.refreshCallbackAction != null);

            DateTime currentUtcTime = DateTime.UtcNow;

            DateTime scheduledRefreshUtcTime = this.CalculateRefreshTime(
                currentUtcTime,
                this.configuration.GeoDataRefreshUtcTime
                );

            this.initialRefreshTime = scheduledRefreshUtcTime;

            TimerCallback timerCallback = new TimerCallback(
                this.refreshCallbackAction
                );

            this.refreshTimer = new Timer(
                timerCallback,
                null,
                scheduledRefreshUtcTime - currentUtcTime,
                this.configuration.GeoDataRefreshInterval
                );
        }

        private DateTime CalculateRefreshTime(
            DateTime currentUtcTime,
            DateTime baseScheduledRefreshTime
            )
        {
            Debug.Assert(currentUtcTime.Kind == DateTimeKind.Utc);

            //
            //  we take the Time part of the configured
            //  DateTime information
            //
            DateTime scheduledRefreshUtcTime = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day,
                baseScheduledRefreshTime.Hour,
                baseScheduledRefreshTime.Minute,
                baseScheduledRefreshTime.Second,
                baseScheduledRefreshTime.Millisecond
                );

            int intervalMilliseconds = 0;
            int multiplyFactor = 0;

            if (currentUtcTime >= scheduledRefreshUtcTime)
            {
                //
                //  The refresh time already passed for the current day
                //  so we take that into account, so we schedule for 
                //  a future time by adding the refresh interval to the scheduled
                //  time, past the current time
                //

                TimeSpan timeDifference = currentUtcTime - scheduledRefreshUtcTime;

                intervalMilliseconds = (int)this.configuration.GeoDataRefreshInterval.TotalMilliseconds;

                multiplyFactor = ((int)timeDifference.TotalMilliseconds) / intervalMilliseconds;

                multiplyFactor = multiplyFactor + 1;

                timeDifference = new TimeSpan(
                    0,
                    0,
                    0,
                    0,
                    intervalMilliseconds * multiplyFactor
                    );

                scheduledRefreshUtcTime = scheduledRefreshUtcTime + timeDifference;
            }
            else if (currentUtcTime + this.configuration.GeoDataRefreshInterval < scheduledRefreshUtcTime)
            {
                //
                //  The refresh time is past the current time, but the refresh interval
                //  needs to be taken into account, as another scheduler might have already been started
                //  and it is scheduled to run based on interval too, thus being possible to run
                //  between now and the next scheduled time 
                //  For example:
                //      Refresh1 scheduled for time T1 (13:00) with interval I1 (1 hour)
                //      Refresh1 starts at T1 and runs for k (=20) I1 intervals
                //      time T2 arrives = T1 + k1 * I1 (9:00 AM)
                //      The next scheduled time for Refresh1 to happen is T1 + (k +1) * I1 = T2 + I1 = 10:00 AM
                //      Refresh2 starts at T2, but is supposed to be scheduled for T1 (same config as Refresh1)
                //      but that would not make it to run at the same time with T1 which
                //      will run next at T2 + I1 = 10:00 AM
                //      we need to hande this scenario

                TimeSpan timeDifference = scheduledRefreshUtcTime - currentUtcTime;

                intervalMilliseconds = (int)this.configuration.GeoDataRefreshInterval.TotalMilliseconds;

                multiplyFactor = ((int)timeDifference.TotalMilliseconds) / intervalMilliseconds;

                timeDifference = new TimeSpan(
                    0,
                    0,
                    0,
                    0,
                    intervalMilliseconds * multiplyFactor
                    );

                scheduledRefreshUtcTime = scheduledRefreshUtcTime - timeDifference;
            }

            return scheduledRefreshUtcTime;
        }

        //=====================================================================
        //  Method: RetrieveNextRefreshTime
        //
        /// <summary>
        /// Calculates the time of the next refresh action
        /// </summary>
        //=====================================================================

        public DateTime? RetrieveNextRefreshTime()
        {
            //
            //  We return something only if the Start has been called
            //  which is equivalent to the refreshTimer being initialized
            //

            DateTime? nextRefreshTime = null;

            if (this.refreshTimer != null)
            {
                Debug.Assert(this.configuration != null);

                Debug.Assert(this.refreshCallbackAction != null);

                DateTime currentUtcTime = DateTime.UtcNow;

                nextRefreshTime = this.CalculateRefreshTime(
                    currentUtcTime,
                    this.initialRefreshTime
                    );
            }

            return nextRefreshTime;
        }

        //=====================================================================
        //  Method: InitializeRefreshTimer
        //
        /// <summary>
        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
        //=====================================================================

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        //=====================================================================
        //  Method: InitializeRefreshTimer
        //
        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        //=====================================================================
        
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.

                    if (this.refreshTimer != null)
                    {
                        this.refreshTimer.Dispose();
                    }
                }

                // Note disposing has been done.
                disposed = true;

            }
        }


    }
}
