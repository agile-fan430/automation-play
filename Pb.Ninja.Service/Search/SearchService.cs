using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Ninja.Service.Search
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using OpenQA.Selenium;

    using Pb.Ninja.Service.Playback;

    public class SearchService
    {
        private readonly PlaybackService playbackService;
        public SearchService(PlaybackService playbackService)
        {
            this.playbackService = playbackService;
        }

        /**
         * function to search song by title on chrome driver
         */
        public async Task<IWebDriver> SearchForMusicAsync(IWebDriver driver, string songName, bool playSong, CancellationToken cancellationToken)
        {
            Thread.Sleep(5000);
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            PopulateSearchBox(songName,driver);                     //populate search result box by input song title on search input box

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            if (playSong)
            {
                Console.WriteLine("playing song: " + songName);
                await this.playbackService.PlaySongFromSearchResultsAsync(driver).ConfigureAwait(false);            //play first song on search result page(need to check artist in TO DO)
            }

            return driver;
        }

        /**
         * input song name on search box
         */
        private void PopulateSearchBox(string songName, IWebDriver driver)
        {
            var searchBox = driver.FindElement(By.ClassName("dt-search-box__input"));
            searchBox.Clear();
            searchBox.SendKeys(songName);
            searchBox.SendKeys(Keys.Enter);
            Thread.Sleep(5000);
        }
       
    }
}
