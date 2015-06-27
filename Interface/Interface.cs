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
using System.Windows.Forms;


namespace SharpDisplay
{
    /// <summary>
    /// For client to specify a specific layout.
    /// A table layout is sent from client to server and defines data fields layout on our display.
    /// </summary>
    [DataContract]
    public class TableLayout
    {
        public TableLayout()
        {
            Columns = new List<ColumnStyle>();
            Rows = new List<RowStyle>();
        }

        /// <summary>
        /// Construct our table layout.
        /// </summary>
        /// <param name="aColumnCount">Number of column in our table.</param>
        /// <param name="aRowCount">Number of rows in our table.</param>
        public TableLayout(int aColumnCount, int aRowCount)
        {
            Columns = new List<ColumnStyle>();
            Rows = new List<RowStyle>();

            for (int i = 0; i < aColumnCount; i++)
            {
                Columns.Add(new ColumnStyle(SizeType.Percent, 100 / aColumnCount));
            }

            for (int i = 0; i < aRowCount; i++)
            {
                Rows.Add(new RowStyle(SizeType.Percent, 100 / aRowCount));
            }
        }

        /// <summary>
        /// Compare two TableLayout object.
        /// </summary>
        /// <returns>Tells whether both layout are identical.</returns>
        public bool IsSameAs(TableLayout aTableLayout)
        {
            //Check rows and columns counts
            if (Columns.Count != aTableLayout.Columns.Count || Rows.Count != aTableLayout.Rows.Count)
            {
                return false;
            }

            //Compare each columns
            for (int i=0;i<Columns.Count;i++)
            {
                if (Columns[i].SizeType != aTableLayout.Columns[i].SizeType)
                {
                    return false;
                }

                if (Columns[i].Width != aTableLayout.Columns[i].Width)
                {
                    return false;
                }
            }

            //Compare each columns
            for (int i = 0; i < Rows.Count; i++)
            {
                if (Rows[i].SizeType != aTableLayout.Rows[i].SizeType)
                {
                    return false;
                }

                if (Rows[i].Height != aTableLayout.Rows[i].Height)
                {
                    return false;
                }
            }

            //Both rows and columns have the same content.
            return true;
        }

        [DataMember]
        public List<ColumnStyle> Columns { get; set; }

        [DataMember]
        public List<RowStyle> Rows { get; set; }
    }

    /// <summary>
    /// Define a data field on our display.
    /// Data field can be either text or bitmap.
    /// </summary>
    [DataContract]
    public class DataField
    {
        public DataField()
        {
            Index = 0;
            ColumnSpan = 1;
            RowSpan = 1;
            //Text
            Text = "";
            Alignment = ContentAlignment.MiddleLeft;
            //Bitmap
            Bitmap = null;
        }

        //Text constructor
        public DataField(int aIndex, string aText = "", ContentAlignment aAlignment = ContentAlignment.MiddleLeft)
        {
            ColumnSpan = 1;
            RowSpan = 1;
            Index = aIndex;
            Text = aText;
            Alignment = aAlignment;
            //
            Bitmap = null;
        }

        //Bitmap constructor
        public DataField(int aIndex, Bitmap aBitmap)
        {
            ColumnSpan = 1;
            RowSpan = 1;
            Index = aIndex;
            Bitmap = aBitmap;
            //Text
            Text = "";
            Alignment = ContentAlignment.MiddleLeft;
        }


        //Generic layout properties
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int Column { get; set; }

        [DataMember]
        public int Row { get; set; }

        [DataMember]
        public int ColumnSpan { get; set; }

        [DataMember]
        public int RowSpan { get; set; }

        //Text properties
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public ContentAlignment Alignment { get; set; }

        //Bitmap properties
        [DataMember]
        public Bitmap Bitmap { get; set; }

        //
        public bool IsBitmap { get{ return Bitmap!=null;} }
        //
        public bool IsText { get { return Bitmap == null; } }
        //
        public bool IsSameLayout(DataField aField)
        {
            return (aField.ColumnSpan == ColumnSpan && aField.RowSpan == RowSpan);
        }
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
        /// Set the given field on your display.
        /// Fields are often just lines of text or bitmaps.
        /// </summary>
        /// <param name="aTextFieldIndex"></param>
        [OperationContract(IsOneWay = true)]
        void SetField(DataField aField);

        /// <summary>
        /// Allows a client to set multiple fields at once.
        /// </summary>
        /// <param name="aFields"></param>
        [OperationContract(IsOneWay = true)]
        void SetFields(System.Collections.Generic.IList<DataField> aFields);

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
