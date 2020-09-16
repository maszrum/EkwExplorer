using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace EkwClicker
{
	internal class SeleniumClicker : IClicker
	{
		private readonly ChromeDriver _driver = new ChromeDriver();
		private readonly string _homeUrl;

		public SeleniumClicker(string homeUrl)
		{
			_homeUrl = homeUrl;
		}

		public void GotoHome()
		{
			if (string.IsNullOrEmpty(_homeUrl))
			{
				throw new NullReferenceException(
					$"property {nameof(_homeUrl)} cannot be null");
			}
			
			_driver.Navigate().GoToUrl(_homeUrl);
		}
		
		public void FillTextbox(string textboxId, string text)
		{
			_driver
				.FindElement(By.Id(textboxId))
				.SendKeys(text);
		}

		public void ClickButtonById(string buttonId)
		{
			var button = _driver.FindElement(By.Id(buttonId));
			ClickButton(button);
		}

		public void ClickButtonByName(string buttonName)
		{
			var button = _driver.FindElement(By.Name(buttonName));
			ClickButton(button);
		}

		private void ClickButton(IWebElement button)
		{
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

		public void CloseCookiesInfo()
		{
			_driver
				.FindElement(By.XPath("//*[@id=\"cookies\"]/div/span/span"))
				.Click();
		}

		public IReadOnlyList<string> GetPropertyNumbers()
		{
			var contentElement = _driver.FindElement(By.Id("contentDzialu"));
			var tableLines = contentElement
				.FindElements(By.TagName("table"))
				.Select(t => t.FindElements(By.TagName("tbody")).FirstOrDefault() ?? t)
				.SelectMany(t => t.FindElements(By.TagName("tr")))
				.ToArray();

			var result = new List<string>();

			foreach (var line in tableLines)
			{
				var cells = line.FindElements(By.TagName("td"));

				if (cells.Count >= 2)
				{
					var firstCellText = cells[0].Text.ToLower();
					var isValidLine = firstCellText == "numer działki";

					if (isValidLine)
					{
						result.Add(cells[1].Text);
					}
				}
			}

			return result;
		}

		public void Dispose()
		{
			_driver.Dispose();
		}

		private static IWebElement GetElementOrDefault(ISearchContext element, By by)
			=> element.FindElements(by).FirstOrDefault();
	}
}
