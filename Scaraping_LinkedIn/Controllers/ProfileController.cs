using HtmlAgilityPack;
using Intercom.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Scaraping_LinkedIn.Models;

namespace Scaraping_LinkedIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BasicInfo>> CallUrl()
        {
            string profileUrl = "https://www.linkedin.com/in/pavan-kumar-vadde-66146b261/";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage httpResponse = await client.GetAsync(profileUrl);

                if (httpResponse.IsSuccessStatusCode)
                {
                    httpResponse.EnsureSuccessStatusCode();
                    string content = await httpResponse.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(content);

                    HtmlNode nameNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"ember27\"]/div[2]/div[2]/div[1]/div[1]/h1");
                    string name = nameNode.InnerText.Trim();
                    HtmlNode genitiveNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"ember27\"]/div[2]/div[2]/div[1]/div[1]/span");
                    string genitive = genitiveNode.InnerText.Trim();
                    HtmlNode roleNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"ember27\"]/div[2]/div[2]/div[1]/div[2]");
                    string role = roleNode.InnerText.Trim();
                    HtmlNode jobNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"ember27\"]/div[2]/div[2]/ul/li[1]/button/span/div");
                    string job = jobNode.InnerText.Trim();
                    HtmlNode collegeNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"ember27\"]/div[2]/div[2]/ul/li[2]/button/span/div");
                    string college = collegeNode.InnerText.Trim();
                    HtmlNode addressNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"ember27\"]/div[2]/div[2]/ul/li[2]/button/span/div");
                    string address = addressNode.InnerText.Trim();

                    string[] regions = address.Split(',');

                    LocationData locationData = new LocationData();
                    if (regions.Length >= 3)
                    {
                        locationData.city_name = regions[0].Trim();
                        locationData.region_name = regions[1].Trim();
                        locationData.country_name = regions[2].Trim();
                    }

                    BasicInfo basicInfo = new BasicInfo();
                    basicInfo.Name = name;
                    basicInfo.Pronouns = genitive;
                    basicInfo.HeadLine = role;
                    basicInfo.Current_Position = job;
                    basicInfo.College = college;
                    basicInfo.City = locationData.city_name;
                    basicInfo.State = locationData.region_name;
                    basicInfo.Country = locationData.country_name;

                    return basicInfo;
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
