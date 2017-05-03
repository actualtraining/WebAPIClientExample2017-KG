using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using BO;
using System.Threading.Tasks;

namespace WebClient.Controllers
{
    public class KategoriController : Controller
    {
        private HttpClient client; 

        public KategoriController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50456/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Kategori
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync("api/Kategori");
            List<Kategori> results;
            if (response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsAsync<List<Kategori>>();
            }
            else
            {
                results = new List<Kategori>();
            }
            return View(results);
        }

        public async Task<Kategori> GetKategoriById(string id)
        {
            var response = await client.GetAsync("api/Kategori/" + id);
            Kategori currKat = null;
            if (response.IsSuccessStatusCode)
            {
                currKat = await response.Content.ReadAsAsync<Kategori>();
            }
            return currKat;
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> InsertPost(Kategori kategori)
        {
            var newKategori = new Kategori
            {
                NamaKategori = kategori.NamaKategori
            };

            var response = await client.PostAsJsonAsync("api/Kategori", newKategori);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "<span class='alert alert-danger'>Data Gagal Ditambah !</span>";
                return View();
            }
        }

        public async Task<ActionResult> Update(string id)
        {
            var editKat = await GetKategoriById(id);
            if(editKat!=null)
            {
                return View(editKat);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePost(Kategori kategori)
        {
            var response = await client.PutAsJsonAsync("api/Kategori", kategori);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "<span class='alert alert-danger'>Data Gagal Diedit !</span>";
                return View();
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            var deleteKat = await GetKategoriById(id);
            if (deleteKat != null)
            {
                return View(deleteKat);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> DeletePost(string KategoriID)
        {
            var response = await client.DeleteAsync("api/Kategori/" + KategoriID);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "<span class='alert alert-danger'>Data Gagal didelete !</span>";
                return View();
            }
        }
    }
}