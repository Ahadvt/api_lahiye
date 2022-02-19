using BookShopMvc.Dtos.JanrDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookShopMvc.Controllers
{
    public class JanrController : Controller
    {
        public async  Task<IActionResult> Index()
        {
            ListDtos<JanrGetDtos> janrgetDtos;
          
            using(HttpClient client=new HttpClient())
            {
                var respons = await client.GetAsync("https://localhost:44386/api/janrs");
                var responsstr = await respons.Content.ReadAsStringAsync();
                if (respons.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    janrgetDtos = JsonConvert.DeserializeObject<ListDtos<JanrGetDtos>>(responsstr);
                   
                    return View(janrgetDtos);
                }
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JanrPostDtos janrPost)
        {
            string EndPoints = "https://localhost:44386/api/janrs";

            using (HttpClient client=new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(janrPost), Encoding.UTF8, "application/json");
               
                using (var respons = await client.PostAsync(EndPoints, content))
                {
                    if (respons.StatusCode==System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("index", "janr");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
           
        }

        public async Task<IActionResult> Edit(int id)
        {
           JanrPostDtos janrPost;
            using (HttpClient client=new HttpClient())
            {
                var respons = await client.GetAsync($"https://localhost:44386/api/janrs/{id}");
                var responStr = await respons.Content.ReadAsStringAsync();

                if (respons.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    janrPost = JsonConvert.DeserializeObject<JanrPostDtos>(responStr);
                    return View(janrPost);
                }

            }
            return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JanrPostDtos janrPost)
        {
            string endpoints = $"https://localhost:44386/api/janrs/{janrPost.id}";
            using (HttpClient client=new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(janrPost), Encoding.UTF8, "application/json");
                using (var response = await client.PutAsync(endpoints, content))
                {
                    if (response.StatusCode==System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("index", "janr");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            using(HttpClient client=new HttpClient())
            {
                var uri = Path.Combine($"https://localhost:44386/api/janrs/{id}");
                var response = await client.DeleteAsync(uri);
                return RedirectToAction("index","janr")
;           }
          
        }
    }

    
}
