using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Hashcode2015.Core;
using Hashcode2015.Core.Model;
using HashCode2015.Model;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity.Query;

namespace Hashcode2015.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHostingEnvironment _env;

		public HomeController(IHostingEnvironment env)
		{
			_env = env;
		}

		public IActionResult Index(string error)
		{
			return View("Index", error);
		}

		public IActionResult Result(int inputFile, IList<IFormFile> outputFile)
		{

			if (inputFile != 1 && inputFile != 2)
				return RedirectToAction("Index", "Home", new { Error = "Invalide input file !" });

			if (outputFile.Count != 1)
				return RedirectToAction("Index", "Home", new { Error = "Please upload your result file" });

			if (outputFile[0].ContentType != "text/plain")
				return RedirectToAction("Index", "Home", new { Error = "invalid submition !" });

			// compute here !
			var outputfile = outputFile[0];

			var outputStream = outputfile.OpenReadStream();

			var rowsCount = 0;
			var slotsCount = 0;
			var poolCount = 0;

			List<Server> servers;
			List<Point> deadSlots;

			var inputFilename = "dc.in";

			if (inputFile == 1)
				inputFilename = "sample.in";

			var filePath = Path.Combine("inputFiles", inputFilename);

			InputReader.Parse(_env.WebRootFileProvider.GetFileInfo(filePath).CreateReadStream(), ref rowsCount, ref slotsCount, ref poolCount, out deadSlots, out servers);

			var datacenter = new DataCenter(rowsCount, slotsCount, deadSlots, poolCount, servers);

			try
			{
				// ### read output file
				OutputReader.Parse(outputStream, datacenter);
			}
			catch (Exception e)
			{
				return RedirectToAction("Index", "Home", new { Error = e.Message });
			}
			

			return View("Result", datacenter);

		}

		public IActionResult Error()
		{
			return View("~/Views/Shared/Error.cshtml");
		}
	}
}