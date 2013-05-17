using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Dr.Pacients;
using Dr.DataBase;

namespace Dr.Parser
{
    public class OperatingRoom
    {
        PacientsProvider pcProvider = null;
        DataBaseProvider dbProvider = null;

        Operations operations;

        public OperatingRoom(Operations operations)
        {
            this.operations = operations;

            pcProvider = (!string.IsNullOrEmpty(operations.Pacients)) ?
                new PacientsProvider(operations.Pacients, operations.PacientsSettings) : null;
            if ((pcProvider != null) && (pcProvider.Instance is IPacientCollection))
            {

            }

            dbProvider = (!string.IsNullOrEmpty(operations.DataBase)) ?
                new DataBaseProvider(operations.DataBase, operations.DataBaseSettings) : null;
            if ((dbProvider != null) && (dbProvider.Instance is IDataBaseProvider))
            {

            }
        }

        public static void _Callback(Object o)
        {

        }

        public static void _Background(Object o)
        {

        }

        public void DoOperations()
        {
            if (MultiThreadMode.Pool == operations.MultiThread)
            {
                int completion = 0;
                int available = 0;
                ThreadPool.GetAvailableThreads(out available, out completion);
                if ((operations.MinThreads > 1) && (operations.MinThreads < available))
                    if (ThreadPool.SetMinThreads(operations.MinThreads, completion))
                    {

                    }

                ThreadPool.QueueUserWorkItem(_Callback, null);
            }
            else if (MultiThreadMode.Background == operations.MultiThread)
            {
                Thread thread = new Thread(_Background, 0);
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                Pacient pacient = null;
                Doctor doc = new Doctor();
                doc.OnException += new DoctorExceptionHandler(OnException);
                doc.OnExecute += new DoctorExecuteQueryHandler(OnExecute);
                doc.OnFile += new DoctorFileHandler(OnFile);
                do
                {
                    pacient = pcProvider.Next;
                    if (pacient != null)
                    {
                        foreach (YamlNode op in operations.List)
                        {
                            doc.Do(op.Name, op.Parameters, ref pacient);
                        }
                        pcProvider.Save(pacient);
                    }
                }
                while (pacient != null);
            }
        }


        #region Events of doctor >

        private void OnException(DoctorExceptionArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void OnExecute(DoctorExecuteQueryArgs e)
        {
            if (dbProvider.Execute(e.Query))
            {

            }
        }

        private void OnFile(DoctorFileArgs e)
        {
            if (!e.File.Exists)
                e.File.Create();

            using (FileStream file = new FileStream(e.File.FullName, FileMode.Append))
            {
                byte[] buffer = Encoding.Default.GetBytes(e.Data);
                file.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion
    }
}
