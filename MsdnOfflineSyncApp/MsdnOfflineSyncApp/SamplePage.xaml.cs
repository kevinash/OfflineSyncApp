using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnOfflineSyncApp
{
	public partial class SamplePage
	{
		public SamplePage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await App.Model.InitStoreAsync();
        }

        async void add_Click(object sender, EventArgs e)
        {
            // insert a new item into local store
            SensorDataItem item = new SensorDataItem {
                text = todo.Text,
                latitude = GetRandomNumber(-90,90),
                longitude = GetRandomNumber(-180, 180),
                distance = GetRandomNumber(-0, 20),
                speed = GetRandomNumber(-0, 100)
            };
            await App.Model.SaveAsync(item);
            todo.Text = "";
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        async void push_Click(object sender, EventArgs e)
        {
            // push data to sync
            await App.Model.PushAsync();
        }

        async void pull_Click(object sender, EventArgs e)
        {
            // pull data to sync
            await App.Model.PullAsync();
        }

        async void purge_Click(object sender, EventArgs e)
        {
            // purge data
            await App.Model.PurgeAsync();
        }
	}
}
