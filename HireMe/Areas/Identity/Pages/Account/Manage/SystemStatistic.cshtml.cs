using HireMe.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = "RequireAdminRole")]
    public class SystemStatisticsModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public SystemStatisticsModel(UserManager<User> userManager)
        {
            _userManager = userManager;
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


            //ViewData["Labels"] = labels;
            //ViewData["Data"] = labels;
            return Page();
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
