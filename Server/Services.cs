using System;
using System.ServiceModel;
using System.Collections;

namespace SharpDisplayManager
{
    [ServiceContract]
    public interface IDisplayService
    {
        [OperationContract]
        void SetText(int aLineIndex, string aText);

        [OperationContract]
        void SetTexts(System.Collections.Generic.IList<string> aTexts);
    }
}
