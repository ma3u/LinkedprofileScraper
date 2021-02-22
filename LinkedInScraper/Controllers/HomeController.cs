using LinkedInScraper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.IO;
using Microsoft.Extensions.Options;

namespace LinkedInScraper.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    // @Zaid call a parameter linkedIn profile ID
    // 

    public IActionResult Index()
    {
      var profileId = "mbuchhorn";
      string url = "https://www.linkedin.com/in/"+ profileId+"/";
      var response = CallUrl(url).Result;

      var list = ParseHTML(response);

      // Options:
      // generate and return JSON (REST API Profile API)
      // store in database (MongoDB) => JSON
      return View();

    }

    // done
    private static async Task<string> CallUrl(string fullUrl)
    {
      HttpClient client = new HttpClient();
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
      client.DefaultRequestHeaders.Accept.Clear();
      var response = client.GetStringAsync(fullUrl);
      return await response;
    }

    /**
     *  @Zaid keys
     *  <summary>Parse specific HTML tags and return a key value list</summary>
     * <returns>
     * <code> 
     *  name :
     *  about : span class=lt-line-clamp__line
     *  expierence :
     *  profile pic : 
     *  
     * </code>
     *</returns>
     */
    //
    // 

    private KeyValuePair<string, string> ParseHTML(string html)
    {

      var profile = new KeyValuePair<string, string> { };

      HtmlDocument htmlDoc = new HtmlDocument();
      htmlDoc.LoadHtml(html);
      var about = htmlDoc.DocumentNode.Descendants("span")
              .Where(node => !node.GetAttributeValue("class", "").Contains("lt-line-clamp__line")).ToList();

      KeyValuePair.Create("about", about);

      // profilepic
       
      // expierence

      return profile;
    }


    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
