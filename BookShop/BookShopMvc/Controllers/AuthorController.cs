using BookShopMvc.Dtos.AuthorDtos;
using BookShopMvc.Dtos.JanrDtos;
using Microsoft.AspNetCore.Http;
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
    public class AuthorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var endpoints = "https://localhost:44386/api/authors";
            ListDtos<AuthorGetDtos> listDtos;
            using(HttpClient client=new HttpClient())
            {
            
                var response = await client.GetAsync(endpoints);
                var responsestr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    listDtos = JsonConvert.DeserializeObject<ListDtos<AuthorGetDtos>>(responsestr);
                    return View(listDtos);
                }
                else
                {
                    return Json(response.StatusCode);
                }
            }
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorPostDtos postDtos)
        {
            var Endpoints = "https://localhost:44386/api/authors";
           using(HttpClient client=new HttpClient())
            {
                var multipartcontent = new MultipartFormDataContent();
                byte[] bytes = null;
               
                if (postDtos.ImageFile!=null)
                {
                   using(var ms=new MemoryStream())
                    {
                        postDtos.ImageFile.CopyTo(ms);
                        bytes = ms.ToArray();
                        var bytearrcontent = new ByteArrayContent(bytes);
                        bytearrcontent.Headers.ContentType = MediaTypeHeaderValue.Parse(postDtos.ImageFile.ContentType);
                        multipartcontent.Add(bytearrcontent, "ImageFile", postDtos.ImageFile.FileName);
                    }
                }

                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(postDtos.FullName), Encoding.UTF8, "application/json"), "FullName");
                using (var response = await client.PostAsync(Endpoints, multipartcontent))
                {
                    if (response.StatusCode==System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("index", "author");
                    }
                    else
                    {
                        return Json(response.StatusCode);
                    }
                }
            }
            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Endpoints = $"https://localhost:44386/api/authors/{id}";
               AuthorEditDtos EditDtos ;
            using(HttpClient client =new HttpClient())
            {
                var response = await client.GetAsync(Endpoints);
                var responstr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    EditDtos = JsonConvert.DeserializeObject<AuthorEditDtos>(responstr);

                    return View(EditDtos);
                }
                else
                {
                    return NotFound();
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorEditDtos authorEdit)
        {
            var Endpoint = $"https://localhost:44386/api/authors/{authorEdit.Id}";
         
            using (HttpClient client=new HttpClient())
            {
                var multipartcontent = new MultipartFormDataContent();

                byte[] bytes = null;
                if (authorEdit.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        authorEdit.ImageFile.CopyTo(ms);
                        bytes = ms.ToArray();
                        var bytearrcontent = new ByteArrayContent(bytes);
                        bytearrcontent.Headers.ContentType = MediaTypeHeaderValue.Parse(authorEdit.ImageFile.ContentType);
                        multipartcontent.Add(bytearrcontent, "ImageFile", authorEdit.ImageFile.FileName);
                    }
                }
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(authorEdit.FullName), Encoding.UTF8, "application/json"),"FullName");
                using (var response = await client.PutAsync(Endpoint, multipartcontent))
                {
                    if (response.StatusCode==System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("index", "author");
                    }
                    else
                    {
                        return Json(response.StatusCode);
                    }
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            using(HttpClient client=new HttpClient())
            {
                var uri = Path.Combine($"https://localhost:44386/api/authors/{id}");
                await client.DeleteAsync(uri);
                return RedirectToAction("index", "author");
            }
        }
    }
}
