//
// Define a public API for both SharpDisplay client and server to use.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections;
using System.Drawing;
using System.Runtime.Serialization;


namespace SharpDisplay
{
    /// <summary>
    /// TextField can be send to our server to be displayed on the screen.
    /// </summary>
    [DataContract]
    public class TableLayout
    {
        public TableLayout()
        {
            ColumnCount = 0;
            RowCount = 0;
            //Alignment = ContentAlignment.MiddleLeft;
        }

        public TableLayout(int aColumnCount, int aRowCount)
        {
            ColumnCount = aColumnCount;
            RowCount = aRowCount;
        }

        [DataMember]
        public int ColumnCount { get; set; }

        [DataMember]
        public int RowCount { get; set; }

        [DataMember]
        public List<DataField> Cells { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class DataField
    {
        [DataMember]
        public int Column { get; set; }

        [DataMember]
        public int Row { get; set; }

        [DataMember]
        public int ColumnSpan { get; set; }

        [DataMember]
        public int RowSpan { get; set; }

    }


    /// <summary>
    /// TextField can be send to our server to be displayed on the screen.
    /// </summary>
    [DataContract]
    public class TextField : DataField
    {
        public TextField()
        {
            Index = 0;
            Text = "";
            Alignment = ContentAlignment.MiddleLeft;
        }

        public TextField(int aIndex, string aText = "", ContentAlignment aAlignment = ContentAlignment.MiddleLeft)
        {
            Index = aIndex;
            Text = aText;
            Alignment = aAlignment;
        }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public ContentAlignment Alignment { get; set; }
    }

    /// <summary>
    /// Define our SharpDisplay service.
    /// Clients and servers must implement it to communicate with one another.
    /// Through this service clients can send requests to a server.
    /// Through this service a server session can receive requests from a client.
    /// </summary>
    [ServiceContract(   CallbackContract = typeof(ICallback), SessionMode = SessionMode.Required)]
    public interface IService
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
        /// </summary>
        /// <param name="aLayout"></param>
        [OperationContract(IsOneWay = true)]
        void SetLayout(TableLayout aLayout);

        /// <summary>
        /// Put the given text in the given field on your display.
        /// Fields are often just lines of text.
        /// </summary>
        /// <param name="aTextFieldIndex"></param>
        [OperationContract(IsOneWay = true)]
        void SetText(TextField aTextField);

        /// <summary>
        /// Allows a client to set multiple text fields at once.
        /// </summary>
        /// <param name="aTexts"></param>
        [OperationContract(IsOneWay = true)]
        void SetTexts(System.Collections.Generic.IList<TextField> aTextFields);

        /// <summary>
        /// Provides the number of clients currently connected
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        int ClientCount();

    }

    /// <summary>
    /// SharDisplay callback provides a means for a server to notify its clients.
    /// </summary>
    public interface ICallback
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
