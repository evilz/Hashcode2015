using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HashCode2015.Model;

namespace Hashcode2015.Core
{
	public static class OutputReader
	{
		// 0 1 0 Serveur 0 placé rangée 0, emplacement 1 et affecté au groupe 0.
		// 1 0 1 Serveur 1 placé rangée 1, emplacement 0 et affecté au groupe 1.
		// 1 3 0 Serveur 2 placé rangée 1, emplacement 3 et affecté au groupe 0.
		// 0 4 1 Serveur 3 placé rangée 0, emplacement 4 et affecté au groupe 1.
		// x     Serveur 4 non utilisé.

		public static void Parse(Stream outputStream, DataCenter dataCenter)
		{
			var reader = new StreamReader(outputStream);

			foreach (var server in dataCenter.AllServers)
			{
				// parse first line to get Matrix Size
				reader.ExtractServerValues(server, dataCenter);

			}
		}

		public static void Parse(string fileName, DataCenter dataCenter)
		{
			using (var stream = File.OpenRead(fileName))
			{
				Parse(stream, dataCenter);
			}

		}

		private static void ExtractServerValues(this StreamReader reader, Server server, DataCenter dataCenter)
		{
			var line = reader.ReadLine();

			if (line == "x")
			{
				server.IsUsed = false;
				server.Pool = null;
				server.Row = -1;
				server.Slot = -1;
			}

			else
			{
				int[] data = null;
				try
				{
					data = line.ExtractValues<int>().ToArray();
				}
				catch (Exception)
				{
					throw new Exception("Invalid file, can't parse " + line);
				}
				
				try
				{
					
					var pool = dataCenter.Pools.First(p => p.Index == data[2]);
					server.Pool = pool;
					pool.Servers.Add(server);

					dataCenter.PutServerAt(server, data[0], data[1]);
				}
				catch (Exception)
				{
					throw new Exception("Pool " + data[2] + " doesn't exist ! Line:" + line);
				}
				
			}

		}

	}
}
