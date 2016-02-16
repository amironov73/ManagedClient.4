using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ManagedClient
{
	class Program
	{
		static void Main(string[] args)
		{
			using (ManagedClient64 client = new ManagedClient64())
			{
				//client.AllowHexadecimalDump = true;
				//client.DebugWriter = Console.Out;

				client.Connect();

				IrbisDatabaseInfo[] databases = client.ListDatabases();
				foreach (IrbisDatabaseInfo database in databases)
				{
					Console.WriteLine(database);
				}

				client.GetServerStat();

				//Console.WriteLine(client.GetDatabaseInfo());

			    //Console.WriteLine("Max MFN={0}",client.GetMaxMfn());


				//Console.WriteLine(client.GetVersion());
				//Thread.Sleep(100);
				//client.NoOp();
				//Thread.Sleep(100);
			    
                //string fileText = client.ReadTextFile(IrbisPath.MasterFile, "pst.mnu");
                //Console.WriteLine(fileText);
                //Thread.Sleep(100);

				//string[] rawRecord = client.ReadRawRecord(2, false);
				//foreach (string line in rawRecord)
				//{
				//    Console.WriteLine(line);
				//}

				//string[] found = client.RawSearch("K=A$", 1, 0, "@brief");
				//foreach (string line in found)
				//{
				//    Console.WriteLine(line);
				//}

				//int[] found = client.Search("K=A$");
				//foreach (int mfn in found)
				//{
				//    Console.WriteLine("{0})", mfn);
				//    Console.WriteLine(client.FormatRecord("@",mfn));
				//    Console.WriteLine();
				//}

				int[] found = client.Search("K=A$");
				string[] texts = client.FormatRecords("@", found);
				foreach (string text in texts)
				{
					Console.WriteLine(text);
					Console.WriteLine();
				}


				Console.WriteLine();
			}
		}
	}
}
