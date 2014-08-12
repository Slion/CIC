using System;
using System.ServiceModel;

namespace SharpDisplayManager
{
    [ServiceContract]
    public interface IDisplayService
    {
        [OperationContract]
        void SetText(int aLineIndex, string aText);
    }
}
