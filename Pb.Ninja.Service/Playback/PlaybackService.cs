// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaybackService.cs" company="">
//   
// </copyright>
// <summary>
//   The playback service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pb.Ninja.Service.Playback
{
    #region

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using OpenQA.Selenium;

    #endregion

    /**
     * Service to get play songs
     */
    public class PlaybackService
    {
        
        /**
         * function to perform play song after search song by title
         */
        public async Task<IWebDriver> PlaySongFromSearchResultsAsync(
            IWebDriver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            ClickPlay(driver);                      //click play button on song logo

            // check if song is finished playing by credentials of player
            while (true)
            {
                var playControl = driver.FindElement(By.ClassName("web-chrome-playback-lcd__scrub"));
                var currentStep = playControl.GetAttribute("aria-valuenow");
                var maxStep = playControl.GetAttribute("aria-valuemax");
                if (Int16.Parse(currentStep) == (Int16.Parse(maxStep) - 1))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }

            return driver;
        }

        /**
         * function for click play button on song logo
         */
        private void ClickPlay(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.ClassName("play-button")).Click();
        }
    }
}