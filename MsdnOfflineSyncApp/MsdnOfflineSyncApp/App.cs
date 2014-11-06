using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MsdnOfflineSyncApp
{
	public class App
	{
        static SensorModel _model;
        public static SensorModel Model
        {
            get
            {
                if (_model == null)
                {
                    _model = new SensorModel();
                }
                return _model;
            }
            set
            {
                _model = value;
            }
        }


       public static Page GetMainPage()
		{
            return new SamplePage();
		}
	}
}
