using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnOfflineSyncApp
{
	public partial class SamplePage
	{
        TransferQueue queue = new TransferQueue();
        const string imageUrl = @"http://upload.wikimedia.org/wikipedia/commons/thumb/c/c2/Golden_Gate_Bridge%2C_SF_%28cropped%29.jpg/800px-Golden_Gate_Bridge%2C_SF_%28cropped%29.jpg";

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

        async void download_Click(object sender, EventArgs e)
        {
            var ok = await queue.AddInProcessAsync(new Job { Id = 1, Url = imageUrl, LocalFile = String.Format("image{0}.jpg", 1)});
            await DisplayAlert("Download Complete", "Result " + ok, "OK", "Cancel");
        }

        async void download10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                queue.AddOutProcess(new Job { Id = i, Url = imageUrl, LocalFile = String.Format("image{0}.jpg", i) });
            }
            await DisplayAlert("Downloads", "Submitted", "OK", "Cancel");
        }
	}
}
