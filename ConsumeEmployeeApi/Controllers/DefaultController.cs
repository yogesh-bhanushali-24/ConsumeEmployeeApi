using ConsumeEmployeeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumeEmployeeApi.Controllers
{
    public class DefaultController : Controller
    {
        private readonly HttpClient client = null;
        private string employeeApiUrl = "";

        public DefaultController(HttpClient client,IConfiguration config)
        {
            this.client = client;
            employeeApiUrl = config.GetValue<string>("AppSettings:EmployeeApiUrl");
        }

        
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(employeeApiUrl);
            string empdata = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
            List<Emp> data = JsonSerializer.Deserialize<List<Emp>>(empdata, options);
            return View(data);
        }


        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{employeeApiUrl}/{id}");
           string stringData= await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Emp obj = JsonSerializer.Deserialize<Emp>(stringData, options);
            return View(obj);
        }

        [HttpGet]
        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> create(Emp obj)
        {
            if (ModelState.IsValid)
            {
                string stringData = JsonSerializer.Serialize(obj);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(employeeApiUrl,contentData);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.message = "Employee Inserted SuccessFully";
                }
                else
                {
                    ViewBag.message = "Error when call api";
                }

                return RedirectToAction("Index");
            }

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{employeeApiUrl}/{id}");
            string stringData= await response.Content.ReadAsStringAsync();
            var options= new JsonSerializerOptions
            {
              PropertyNameCaseInsensitive  = true
            };

            Emp obj = JsonSerializer.Deserialize<Emp>(stringData, options);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Emp obj)
        {
            if (ModelState.IsValid)
            {
                string stringData = JsonSerializer.Serialize(obj);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"{employeeApiUrl}/{obj.Empid}", contentData);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.message = "Employee Updated Successfully";
                }
                else
                {
                    ViewBag.message = "Error while calling web api";
                }

            }
            return RedirectToAction("Index");  
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{employeeApiUrl}/{id}");
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Emp obj = JsonSerializer.Deserialize<Emp>(stringData, options);
            return View(obj);
        }


        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteRec(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{employeeApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.message = "Employee Delete Successfully";
            }
            else
            {
                ViewBag.message = "Error While Calling Wrb api";
            }

            return RedirectToAction("Index");
           
        }

    }
}
