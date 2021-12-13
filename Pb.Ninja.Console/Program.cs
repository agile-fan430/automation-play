// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pb.Ninja.Console
{
    #region

    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    using Pb.Ninja.Console.Extensions;
    using Pb.Ninja.Service.Login;
    using Pb.Ninja.Service.Playback;
    using Pb.Ninja.Service.Playlist;
    using Pb.Ninja.Service.Search;
    using Pb.Ninja.Service.AccountList;

    #endregion

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        
        /**
         * Function for play musics after login
         */
        public static async Task StartPlaybackAsync(
            PlaylistService playlistService,
            SearchService searchService,
            IWebDriver driver)
        {
            var songList = await playlistService.GetSongListAsync().ConfigureAwait(false);          //get song list from local file using playlist service
            
            songList.Shuffle();                                                                     //play musics shuffle(optional)
            foreach (var song in songList)
            {
                var source = new CancellationTokenSource();

                source.CancelAfter(TimeSpan.FromSeconds(300));

                var taswk = Task.Run(() => {
                    searchService.SearchForMusicAsync(driver, song, true, source.Token);            //search and play song by song title from playlist on chromedriver
                });

                taswk.Wait();
            }
        }

        /**
         * function for main process
         */
        private static async void AccountList()
        {

            var loginService = new LoginService();                      // login service to login apple music with login credentials
            var playbackServive = new PlaybackService();                // Play service to play songs with playlist on options of shuffle
            var searchService = new SearchService(playbackServive);     // search service to get a song by song title
            var playlistService = new PlaylistService();                // playlist service which provides song list from local file(Will be change into API service to get music list from server
            var accountlistService = new AccountListService();          // account service to get accounts from local file(will be changed into API service to get account list from server
            
            var accountList = await accountlistService.GetAccountListAsync().ConfigureAwait(false);     //get account list with accountlist service
            accountList.Shuffle();                                                                      //suffle option on account list (optional)
            foreach (var account in accountList)
            {
                Thread thread = new Thread(() =>                                                        //Thread for each account login and play songs
                {
                    //open chromedriver
                    var cService = ChromeDriverService.CreateDefaultService();
                    cService.HideCommandPromptWindow = true;

                    var options = new ChromeOptions();
                    options.AddArguments("--proxy-server=zproxy.lum-superproxy.io:22225");
                    options.AddExtension(@"Data\proxyhelper.zip");
                    //options.AddArguments("headless");
                    options.Proxy = null;

                    string userAgent = account.Item1;

                    options.AddArgument($"--user-agent={userAgent}$PC${"lum-customer-hl_637ef71b-zone-static_res:ujf5m5wq32va"}");

                    IWebDriver driver = new ChromeDriver(cService, options);

                    try
                    {
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                        driver.Url = "https://beta.music.apple.com/";

                        driver = loginService.Login(driver, account);                   //login to apple music using account credentials on chrome driver

                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                        StartPlaybackAsync(playlistService, searchService, driver);     //play music list after login apple music with playlist from playlist service
                    }
                    finally
                    {
                        if (driver != null)
                            driver.Dispose();                                           //after playing or throwing errors, release memory of chrome driver
                    }
                });
                thread.Start();                                                         //thread start
                Thread.Sleep(80000);                                                     // make delay for start each thread with 60s for lazy rendering on server with no VRAM
            }
        }

        /**
         * Project start here
         */
        private static void Main(string[] args)
        {
            AccountList();
        }
    }
}