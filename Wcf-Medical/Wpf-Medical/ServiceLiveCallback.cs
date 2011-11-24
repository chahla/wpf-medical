using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Wpf_Medical
{
    public class ServiceLiveCallback : Wpf_Medical.ServiceLive.IServiceLiveCallback
    {
        private static ObservableCollection<KeyValuePair<string, double>> _heartlist;
        private static ObservableCollection<KeyValuePair<string, double>> _templist;

        public static ObservableCollection<KeyValuePair<string, double>> Heartlist
        {
            get { return ServiceLiveCallback._heartlist; }
            set { ServiceLiveCallback._heartlist = value; }
        }

        public static ObservableCollection<KeyValuePair<string, double>> Templist
        {
            get { return ServiceLiveCallback._templist; }
            set { ServiceLiveCallback._templist = value; }
        }

        public ServiceLiveCallback()
        {
            _heartlist = new ObservableCollection<KeyValuePair<string, double>>();
            _templist = new ObservableCollection<KeyValuePair<string, double>>();
        }

        public void PushDataHeart(double requestData)
        {
            Debug.WriteLine(requestData);
            string time = DateTime.Now.TimeOfDay.Hours + ":" + DateTime.Now.TimeOfDay.Minutes + ":" + DateTime.Now.TimeOfDay.Seconds;
            _heartlist.Add(new KeyValuePair<string, double>(time, requestData));
        }

        public void PushDataTemp(double requestData)
        {
            Debug.WriteLine(requestData);
            string time = DateTime.Now.TimeOfDay.Hours + ":" + DateTime.Now.TimeOfDay.Minutes + ":" + DateTime.Now.TimeOfDay.Seconds;
            _templist.Add(new KeyValuePair<string, double>(time, requestData));
        }

    }
}
