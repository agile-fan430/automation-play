using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Ninja.Service.Login
{
    using System.Collections.ObjectModel;
    using System.Linq;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public  class LoginService
    {
        /**
         * Function for login to apple music using account credentials on chromedriver)
         */
        public  IWebDriver Login(IWebDriver driver, Tuple<string, string> account)
        {
            // Get the current window handle so you can switch back later.

            string currentHandle = driver.CurrentWindowHandle;
            ReadOnlyCollection<string> originalHandles = driver.WindowHandles;

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Cause the popup to appear
            var loginButton = driver.FindElements(By.ClassName("web-navigation__auth-button--sign-in"))[2];
            loginButton.Click();


            // WebDriverWait.Until<T> waits until the delegate returns
            // a non-null value for object types. We can leverage this
            // behavior to return the popup window handle.
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            string popupWindowHandle = wait.Until<string>((d) =>
            {
                string foundHandle = null;

                // Subtract out the list of known handles. In the case of a single
                // popup, the newHandles list will only have one value.
                List<string> newHandles = driver.WindowHandles.Except(originalHandles).ToList();
                if (newHandles.Count > 0)
                {
                    foundHandle = newHandles[0];
                }

                return foundHandle;
            });
            //get popup handle to login
            driver.SwitchTo().Window(popupWindowHandle);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.SwitchTo().Frame(driver.FindElement(By.Id("aid-auth-widget-iFrame")));

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            //input account email on email input field
            var link = driver.FindElement(By.TagName("input"));
            link.SendKeys(account.Item1);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            //click continue button on right of email input field
            var loginbtn = driver.FindElement(By.TagName("button"));
            loginbtn.Click();
            
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            //input account password on password input field
            var passwordField = driver.FindElement(By.Id("password_text_field"));
            passwordField.SendKeys(account.Item2);

            //click submit(login) button to finalize login
            var loginbtn2 = driver.FindElement(By.TagName("button"));
            loginbtn2.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            //click success button after login
            var successBtn = driver.FindElement(By.TagName("button"));
            successBtn.Click();
            return driver;
        }

    }
}
