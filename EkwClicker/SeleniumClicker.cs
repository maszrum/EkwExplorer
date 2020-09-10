using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace EkwClicker
{
	internal class SeleniumClicker : IDisposable
	{
		private readonly ChromeDriver _driver;

		public SeleniumClicker(string url)
		{
			_driver = new ChromeDriver();
			_driver.Navigate().GoToUrl(url);
		}

		public void FillTextbox(string textboxId, string text)
		{
			_driver
				.FindElement(By.Id(textboxId))
				.SendKeys(text);
		}

		public void ClickButton(string buttonId)
		{
			var button = _driver.FindElement(By.Id(buttonId));
			var actions = new Actions(_driver);

			actions
				.MoveToElement(button)
				.Click()
				.Perform();
		}

		public string GetValueFromTable(string rowCaption)
		{
			var elements = _driver.FindElements(By.ClassName("form-row"));

			foreach (var element in elements)
			{
				var captionDiv = GetElementOrDefault(element, By.ClassName("label-column-50"));
				if (captionDiv == null)
					continue;

				var captionLabel = GetElementOrDefault(captionDiv, By.TagName("label"));
				if (captionLabel == null)
					continue;

				if (captionLabel.Text.Contains(rowCaption))
				{
					var contentDiv = element.FindElement(By.ClassName("content-column-50"));
					var contentDivInner = contentDiv.FindElement(By.TagName("div"));

					return contentDivInner.Text;
				}
			}

			return null;
		}

		public bool CheckIfAnyError()
		{
			var container = _driver
				.FindElements(By.Id("recaptchaResponseErrors"))
				.FirstOrDefault();

			if (container == null)
			{
				return false;
			}

			var errorsElements = container.FindElements(By.TagName("span"));

			return errorsElements.Count > 0;
		}

		internal void CloseCookiesInfo(string xPath)
		{
			_driver
				.FindElement(By.XPath(xPath))
				.Click();
		}

		public void Dispose()
		{
			_driver.Dispose();
		}

		private static IWebElement GetElementOrDefault(ISearchContext element, By by)
			=> element.FindElements(by).FirstOrDefault();
	}
}
