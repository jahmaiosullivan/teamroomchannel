using System;
using System.ServiceModel;

namespace Microsoft.IT.Geo.Hosting
{

    public class ClientProxy
    {
        //=========================================================================
        //  Method: ClientProxy
        //
        /// <summary>
        /// Default constructor
        /// </summary>
        //=========================================================================

        private ClientProxy()
        {
        }

        //=====================================================================
        //  Method: OpenProxy
        //
        /// <summary>
        /// Opens a client proxy of the specificed service contract
        /// </summary>  
        //=====================================================================

        public static void OpenProxy<TChannel>(System.ServiceModel.ClientBase<TChannel> clientBase)
            where TChannel : class
        {
            if (clientBase != null)
            {

                if (clientBase.ChannelFactory.State != CommunicationState.Opened)
                {
                    try
                    {
                        clientBase.Open();
                    }
                    catch (Exception)
                    {
                        if (clientBase.State == CommunicationState.Opened)
                        {
                            clientBase.Close();
                        }
                        else if (clientBase.State == CommunicationState.Faulted)
                        {
                            clientBase.Abort();
                        }

                        throw;
                    }
                }
            }
        }


        //=====================================================================
        //  Method: CloseProxy
        //
        /// <summary>
        /// Colses a client proxy of the specificed service contract
        /// </summary>  
        //=====================================================================

        public static void CloseProxy<TChannel>(ClientBase<TChannel> clientBase)
            where TChannel : class
        {
            if (clientBase != null)
            {
                if (clientBase.State == CommunicationState.Opened)
                {
                    //
                    //  this call could fail, as the channel might be timed out
                    //  so we catch the communication exception and abort
                    //  if that happens
                    //

                    try
                    {
                        clientBase.Close();
                    }
                    catch (CommunicationException)
                    {
                        clientBase.Abort();
                    }
                    catch (TimeoutException)
                    {
                        clientBase.Abort();
                    }
                    catch (Exception)
                    {
                        clientBase.Abort();
                        throw;
                    }
                }
                else if (clientBase.State == CommunicationState.Faulted)
                {
                    clientBase.Abort();
                }
            }
        }
    }
}
