using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Benchmark;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = "RequireAdminRole")]
    public class SystemStatisticsModel : PageModel
    {
        private readonly IBaseService _baseService;
        private readonly UserManager<User> _userManager;
        private readonly IBenchmarkService _benchmarkService;

        public SystemStatisticsModel(
            IBaseService baseService,
            UserManager<User> userManager, 
            IBenchmarkService benchmarkService)
        {
            _baseService = baseService;
            _userManager = userManager;
            _benchmarkService = benchmarkService;
        }

        public int Threads { get; set; }
        public int Ports { get; set; }

        public int MaxThreads { get; set; }
        public int MaxPorts { get; set; }

        public int MinThreads { get; set; }
        public int MinPorts { get; set; }
        public int TotalProcesses  { get; set; }

        public double CPU { get; set; }
        public int RAM { get; set; }

        public Process[] ProcessList = Process.GetProcesses();

        public string WebDir { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            public int count { get; set; }
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            int threads, ports, maxthreads, maxports, minthreads, minports;
            int processcount = Environment.ProcessorCount;

            ThreadPool.GetMinThreads(out minthreads, out minports);
            ThreadPool.GetMaxThreads(out maxthreads, out maxports);
            ThreadPool.GetAvailableThreads(out threads, out ports);                    

            Threads = threads;
            Ports = ports;

            MaxThreads = maxthreads;
            MaxPorts = maxports;

            MinThreads = minthreads;
            MinPorts = minports;

            TotalProcesses = processcount;

            var proc = Process.GetCurrentProcess();
            decimal mem = (int)proc.WorkingSet64;
            RAM = (int)Math.Floor(mem);
            CPU = await GetCpuUsageForProcess();

           /* WebDir = Directory.GetCurrentDirectory();*/


            //ViewData["Labels"] = labels;
            //ViewData["Data"] = labels;
            return Page();
        }

        // Jobs
        public async Task<IActionResult> OnPostSeedJobs1Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time =_benchmarkService.SeedJobs(Input.count, user.Id);
            _baseService.ToastNotify(ToastMessageState.Info, "Result", $"Jobs seeding executed for {time} ms. ( Normal operation! )", 10000);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSeedJobs2Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = await _benchmarkService.SeedJobsAsync(Input.count, user.Id);
            _baseService.ToastNotify(ToastMessageState.Success, "Result", $"Jobs seeding executed for {time} ms. ( Asynchronous operation! )", 10000);

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteJobs1Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = _benchmarkService.RemoveJobs(user);
            _baseService.ToastNotify(ToastMessageState.Info, "Result", $"Jobs removal was executed for {time} ms. ( Normal operation! )", 10000);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteJobs2Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = await _benchmarkService.RemoveJobsAsync(user);
            _baseService.ToastNotify(ToastMessageState.Success, "Result", $"Jobs removal was executed for {time} ms. ( Asynchronous operation! )", 10000);

            return RedirectToPage();
        }



        // Contestants
        public async Task<IActionResult> OnPostSeedContestants1Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = _benchmarkService.SeedContestants(Input.count, user.Id);
            _baseService.ToastNotify(ToastMessageState.Info, "Result", $"Contestants seeding executed for {time} ms. ( Normal operation! )", 10000);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSeedContestants2Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = await _benchmarkService.SeedContestantsAsync(Input.count, user.Id);
            _baseService.ToastNotify(ToastMessageState.Success, "Result", $"Contestants seeding executed for {time} ms. ( Asynchronous operation! )", 10000);

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteContestants1Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = _benchmarkService.RemoveContestants(user);
            _baseService.ToastNotify(ToastMessageState.Info, "Result", $"Contestants removal was executed for {time} ms. ( Normal operation! )", 10000);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteContestants2Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = await _benchmarkService.RemoveContestantsAsync(user);
            _baseService.ToastNotify(ToastMessageState.Success, "Result", $"Contestants removal was executed for {time} ms. ( Asynchronous operation! )", 10000);

            return RedirectToPage();
        }


        // Company
        public async Task<IActionResult> OnPostSeedCompanies1Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = _benchmarkService.SeedCompany(Input.count, user.Id);
            _baseService.ToastNotify(ToastMessageState.Info, "Result", $"Companies seeding executed for {time} ms. ( Normal operation! )", 10000);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSeedCompanies2Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = await _benchmarkService.SeedCompanyAsync(Input.count, user.Id);
            _baseService.ToastNotify(ToastMessageState.Success, "Result", $"Companies seeding executed for {time} ms. ( Asynchronous operation! )", 10000);

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteCompanies1Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = _benchmarkService.RemoveCompanies(user);
            _baseService.ToastNotify(ToastMessageState.Info, "Result", $"Companies removal was executed for {time} ms. ( Normal operation! )", 10000);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteCompanies2Async()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            var time = await _benchmarkService.RemoveContestantsAsync(user);
            _baseService.ToastNotify(ToastMessageState.Success, "Result", $"Companies removal was executed for {time} ms. ( Asynchronous operation! )", 10000);

            return RedirectToPage();
        }

        private async Task<double> GetCpuUsageForProcess()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime; 
            await Task.Delay(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime; 
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            return Math.Floor(cpuUsageTotal * 100);
        }




    }
}
