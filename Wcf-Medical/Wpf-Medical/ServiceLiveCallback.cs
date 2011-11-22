using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Wpf_Medical
{
    class ServiceLiveCallback : Wpf_Medical.ServiceLive.IServiceLiveCallback
    {
        public ServiceLiveCallback()
        {
        }

        public void PushDataHeart(double requestData)
        {
            Debug.WriteLine(requestData);
        }

        public void PushDataTemp(double requestData)
        {
            Debug.WriteLine(requestData);
        }

    }
}
