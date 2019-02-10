using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Advanced_Lesson_6_Multithreading
{
    class Practice
    {      
        /// <summary>
        /// LA8.P1/X. Написать консольные часы, которые можно останавливать и запускать с 
        /// консоли без перезапуска приложения.
        /// </summary>
        public static void LA8_P1_5()
        {
            var timeThread = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine(DateTime.Now);
                    System.Threading.Thread.Sleep(1000);
                    Console.Clear();
                }
            });
            timeThread.Start();
            while (true)
            {
                var key = Console.ReadKey().KeyChar;
                if (key == '1')
                {
                    timeThread.Suspend();
                }

                if (key == '2')
                {
                    timeThread.Resume();
                }
                
            }
            //while(true)
            //{
            //    Console.WriteLine(DateTime.Now);
            //    System.Threading.Thread.Sleep(1000);
            //    Console.Clear();
            //}
        }

        /// <summary>
        /// LA8.P2/X. Написать консольное приложение, которое “делает массовую рассылку”. 
        /// </summary>
        public static void LA8_P2_5()
        {
			string pathFolder = @"D:\Emails\";
			string fileName = "EmailAdress";
			//Сохраниение файлов в одном потоке
			for (int i = 0; i < 50; i++)
			{
				DirectoryInfo dirInfo = new DirectoryInfo(pathFolder);
				if (dirInfo.Exists)
				{
					using (StreamWriter sw = new StreamWriter($"{dirInfo.FullName + fileName}_{i + 1}.txt", true))
					{
						sw.WriteLine($"Текст рассылки для почтового ящика {i + 1}");
						sw.Close();
						Thread.Sleep(500);
					}
				}
			}
			//Сохраниение файлов через ThreadPool
			fileName = "ThreadPool";
			for (int i = 0; i < 50; i++)
			{
				var index = i;//Переменная, объявленная внутри цикла, необходима для правильного захвата переменной i
				ThreadPool.QueueUserWorkItem((object state) =>
				{
					DirectoryInfo dirInfo = new DirectoryInfo(pathFolder);
					Random rnd = new Random();
					if (dirInfo.Exists)
					{
						using (StreamWriter sw = new StreamWriter($"{dirInfo.FullName + fileName}_{index + 1}.txt", true))
						{
							sw.WriteLine($"Текст рассылки для почтового ящика {index + 1}");
							sw.Close();
							Thread.Sleep(1000);
						}
					}
				});
			}
			Console.ReadKey();  //Ожидаем ввода символа, поскольку меобходимо, чтобы отработали все потоки ThreadPool-а
								//Иначе, при закрытии приложения, завершаются все выполняемые потоки.

			
		}

		/// <summary>
		/// Написать код, который в цикле (10 итераций) эмулирует посещение 
		/// сайта увеличивая на единицу количество посещений для каждой из страниц.  
		/// </summary>
		public static void LA8_P3_5()
        {            
        }

		/// <summary>
		/// LA8.P4/X. Отредактировать приложение по “рассылке” “писем”. 
		/// Сохранять все “тела” “писем” в один файл. Использовать блокировку потоков, чтобы избежать проблем синхронизации.  
		/// </summary>
		public static void LA8_P4_5()
		{
			var obj = new Object();
			int counter = 0;
			string pathFolder = @"D:\Emails\";
			string fileName = "EmailAdress";
			//==========================================================
			//Сохраниение писем в один файл через ThreadPool с блокировкой
			fileName = "ThreadPoolLock";
			DirectoryInfo dirInfo = new DirectoryInfo(pathFolder);
			
			for (int i = 0; i < 50; i++)
			{
				var index = i;//Переменная, объявленная внутри цикла, необходима для правильного захвата переменной i
				ThreadPool.QueueUserWorkItem((object state) =>
				{

					//DirectoryInfo dirInfo = new DirectoryInfo(pathFolder);
					//StreamWriter sw = new StreamWriter($"{dirInfo.FullName + fileName}_Lock.txt");
					if (dirInfo.Exists)
					{
						lock (obj)
						{
							using (StreamWriter sw = new StreamWriter($"{dirInfo.FullName + fileName}.txt",true))
							{
								sw.WriteLine($"Текст рассылки для почтового ящика {index + 1}");
								Thread.Sleep(500);
								sw.Close();
								counter++;
							}
						}
					}
				});
			}
			while(counter < 50)
			{
				Console.Clear();
				Console.WriteLine($"Идет отправка писем. Отправлено {counter} из 50");
				Thread.Sleep(100);
			}
			Console.Clear();
			Console.WriteLine($"Идет отправка писем. Отправлено {counter} из 50");
			Console.WriteLine($"Отправка писем завершена.");
			Console.ReadKey();
		}


        /// <summary>
        /// LA8.P5/5. Асинхронная “отсылка” “письма” с блокировкой вызывающего потока 
        /// и информировании об окончании рассылки (вывод на консоль информации 
        /// удачно ли завершилась отсылка). 
        /// </summary>
        public async static void LA8_P5_5()
        {           
        }
    }    
}
