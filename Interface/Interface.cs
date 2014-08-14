using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections;


namespace SharpDisplayInterface
{

    [ServiceContract(CallbackContract = typeof(IDisplayServiceCallback))]
    public interface IDisplayService
    {
        [OperationContract(IsOneWay = true)]
        void Connect(string aClientName);

        [OperationContract(IsOneWay = true)]
        void SetText(int aLineIndex, string aText);

        [OperationContract(IsOneWay = true)]
        void SetTexts(System.Collections.Generic.IList<string> aTexts);
    }


    public interface IDisplayServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnConnected();

        [OperationContract(IsOneWay = true)]
        void OnServerClosing();
    }



}
