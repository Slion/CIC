using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections;


namespace SharpDisplayInterface
{
    
    [ServiceContract(   CallbackContract = typeof(IDisplayServiceCallback),
                        SessionMode = SessionMode.Required)]
    public interface IDisplayService
    {
        /// <summary>
        /// Set the name of this client.
        /// Name is a convenient way to recognize your client.
        /// Naming you client is not mandatory.
        /// In the absence of a name the session ID is often used instead.
        /// </summary>
        /// <param name="aClientName"></param>
        [OperationContract(IsOneWay = true)]
        void SetName(string aClientName);

        /// <summary>
        /// Put the given text in the given field on your display.
        /// Fields are often just lines of text.
        /// </summary>
        /// <param name="aFieldIndex"></param>
        /// <param name="aText"></param>
        [OperationContract(IsOneWay = true)]
        void SetText(int aFieldIndex, string aText);

        /// <summary>
        /// Allows a client to set multiple text fields at once.
        /// First text in the list is set into field index 0.
        /// Last text in the list is set into field index N-1.
        /// </summary>
        /// <param name="aTexts"></param>
        [OperationContract(IsOneWay = true)]
        void SetTexts(System.Collections.Generic.IList<string> aTexts);

        /// <summary>
        /// Provides the number of clients currently connected
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        int ClientCount();

    }


    public interface IDisplayServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnConnected();

        /// <summary>
        /// Tell our client to close its connection.
        /// Notably sent when the server is shutting down.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void OnCloseOrder();
    }



}
