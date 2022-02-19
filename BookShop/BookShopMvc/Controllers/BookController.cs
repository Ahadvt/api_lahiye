using BookShopMvc.Dtos.AuthorDtos;
using BookShopMvc.Dtos.BookDtos;
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
    public class BookController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ListDtos<BookGetDtos> listDtos;
            var endpoint = "https://localhost:44386/api/books";
            using(HttpClient client = new HttpClient())
            {
                var respons = await client.GetAsync(endpoint);
                var responstr = await respons.Content.ReadAsStringAsync();
                if (respons.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    listDtos = JsonConvert.DeserializeObject<ListDtos<BookGetDtos>>(responstr);
                    return View(listDtos);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        public async Task<IActionResult> Create()
        {
            
            ListDtos<AuthorGetDtos> authorGetDtos;
            ListDtos<JanrGetDtos> janrGetDtos;
            var EndpointAuthor = "https://localhost:44386/api/authors";
            var EndpointJanr = "https://localhost:44386/api/janrs";
            using(HttpClient client=new HttpClient())
            {
                var AuthorResponse = await client.GetAsync(EndpointAuthor);
                var authorResponStr = await AuthorResponse.Content.ReadAsStringAsync();
                
                
                var JanrResponse = await client.GetAsync(EndpointJanr);
                var JanrResponStr = await JanrResponse.Content.ReadAsStringAsync();
                if (JanrResponse.StatusCode == System.Net.HttpStatusCode.OK&& AuthorResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    authorGetDtos = JsonConvert.DeserializeObject<ListDtos<AuthorGetDtos>>(authorResponStr);
                    janrGetDtos = JsonConvert.DeserializeObject<ListDtos<JanrGetDtos>>(JanrResponStr);
                    EntitiesDtos entitiesDtos = new EntitiesDtos
                    {
                        Authors=authorGetDtos,
                        Janrs=janrGetDtos
                    };
                    return View(entitiesDtos);
                }
                else
                {
                    return Json(JanrResponse.StatusCode);
                }

            }

         
        }

        [HttpPost]
        public async Task<IActionResult> Create(EntitiesDtos entitiesDtos)
        {
            var Endpoints = "https://localhost:44386/api/books";
            using(HttpClient client= new HttpClient())
            {
                byte[] bytes = null;
                var multipartcontent = new MultipartFormDataContent();
                if (entitiesDtos.postDtos.Imagefile!=null)
                {
                    using(var ms=new MemoryStream())
                    {
                        entitiesDtos.postDtos.Imagefile.CopyTo(ms);
                        bytes = ms.ToArray();
                        var bytearrcontent = new ByteArrayContent(bytes);
                        bytearrcontent.Headers.ContentType = MediaTypeHeaderValue.Parse(entitiesDtos.postDtos.Imagefile.ContentType);
                        multipartcontent.Add(bytearrcontent, "Imagefile", entitiesDtos.postDtos.Imagefile.FileName);
                    }
                }
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.Name), Encoding.UTF8,"application/json"), "name");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.Title), Encoding.UTF8, "application/json"), "title");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.Description), Encoding.UTF8, "application/json"), "description");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.SalePrice), Encoding.UTF8, "application/json"), "saleprice");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.CostPrice), Encoding.UTF8, "application/json"), "costprice");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.AuthorId), Encoding.UTF8, "application/json"), "authorid");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.JanrId), Encoding.UTF8, "application/json"), "janrid");
                using(var Response=await client.PostAsync(Endpoints, multipartcontent))
                {
                    if (Response.StatusCode==System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("index", "book");
                    }
                    else
                    {
                        return Json(Response.StatusCode);
                    }
                }
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            ListDtos<AuthorGetDtos> authorGetDtos;
            ListDtos<JanrGetDtos> janrGetDtos;
            BookPostDtos bookPost;
            var EndpointAuthor = "https://localhost:44386/api/authors";
            var EndpointJanr = "https://localhost:44386/api/janrs";
            var EndpointBook = $"https://localhost:44386/api/books/{id}";
            using (HttpClient client = new HttpClient())
            {
                var AuthorResponse = await client.GetAsync(EndpointAuthor);
                var authorResponStr = await AuthorResponse.Content.ReadAsStringAsync();
                var bookResponse = await client.GetAsync(EndpointBook);
                var bookResponsestr = await bookResponse.Content.ReadAsStringAsync();


                var JanrResponse = await client.GetAsync(EndpointJanr);
                var JanrResponStr = await JanrResponse.Content.ReadAsStringAsync();
                if (JanrResponse.StatusCode == System.Net.HttpStatusCode.OK && AuthorResponse.StatusCode == System.Net.HttpStatusCode.OK&&bookResponse.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    authorGetDtos = JsonConvert.DeserializeObject<ListDtos<AuthorGetDtos>>(authorResponStr);
                    janrGetDtos = JsonConvert.DeserializeObject<ListDtos<JanrGetDtos>>(JanrResponStr);
                    bookPost = JsonConvert.DeserializeObject<BookPostDtos>(bookResponsestr);
                    EntitiesDtos entitiesDtos = new EntitiesDtos
                    {
                        Authors = authorGetDtos,
                        Janrs = janrGetDtos,
                        postDtos=bookPost
                    };
                    return View(entitiesDtos);
                }
                else
                {
                    return Json(bookResponse.StatusCode);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,EntitiesDtos entitiesDtos)
        {
            var endpoints = $"https://localhost:44386/api/books/{id}";
            using(HttpClient client=new HttpClient())
            {
                var multipartcontent = new MultipartFormDataContent();
                byte[] bytes = null;
                if (entitiesDtos.postDtos.Imagefile!=null)
                {
                    using(var ms=new MemoryStream())
                    {
                        entitiesDtos.postDtos.Imagefile.CopyTo(ms);
                        bytes = ms.ToArray();
                        var bytearrcontent = new ByteArrayContent(bytes);
                        bytearrcontent.Headers.ContentType = MediaTypeHeaderValue.Parse(entitiesDtos.postDtos.Imagefile.ContentType);
                        multipartcontent.Add(bytearrcontent, "Imagefile", entitiesDtos.postDtos.Imagefile.FileName);
                    }
                }

                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.Name), Encoding.UTF8, "application/json"), "name");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.Title), Encoding.UTF8, "application/json"), "title");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.Description), Encoding.UTF8, "application/json"), "description");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.SalePrice), Encoding.UTF8, "application/json"), "saleprice");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.CostPrice), Encoding.UTF8, "application/json"), "costprice");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.AuthorId), Encoding.UTF8, "application/json"), "authorid");
                multipartcontent.Add(new StringContent(JsonConvert.SerializeObject(entitiesDtos.postDtos.JanrId), Encoding.UTF8, "application/json"), "janrid");
                
                using(var response=await client.PutAsync(endpoints, multipartcontent))
                {
                    if (response.StatusCode==System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("index", "book");
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
             var uri=  Path.Combine($"https://localhost:44386/api/books/{id}");
                await client.DeleteAsync(uri);
                return RedirectToAction("index", "book");
            }
            
        }
    }
}
