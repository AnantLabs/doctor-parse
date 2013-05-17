using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dr.Pacients
{
    internal delegate void FileProcess(FileInfo file);

    class OnFileEventArgs
    {
        public OnFileEventArgs(FileInfo f) { File = f; }
        public FileInfo File { get; private set; }
    }

    public class Directory : IPacientCollection
    {
        private DirectorySettings Settings;
        private DirectoryInfo mSrcDirectory;
        private DirectoryInfo mDesDirectory;
        private FileSystemInfo[] mFiles;
        private int mFilesIndex;
        private int mFilesCount;
        private int mDirectoriesCount;

        private Pacient mPrevPacinet;
        private Pacient mCurrentPacinet;

        public Directory(DirectorySettings settings)
        {
            Settings = settings;

            mSrcDirectory = new DirectoryInfo(Settings.Source);
            if (!mSrcDirectory.Exists)
            {

            }

            mDesDirectory = new DirectoryInfo(Settings.Destination.Replace("%CurrentDirectory%",AppDomain.CurrentDomain.BaseDirectory));
            if (!mDesDirectory.Exists)
                mDesDirectory.Create();
            else
            {
                mDesDirectory.Delete(true);
                mDesDirectory.Create();
            }

            mFiles = mSrcDirectory.GetFileSystemInfos();
            mFilesIndex = -1;
            mDirectoriesCount = 0;
            mFilesCount = mFiles.Length;
        }

        public int Count
        {
            get { return 0; }
        }

        public Pacient Next
        {
            get {
                
                while (mFilesCount > (++mFilesIndex))
                {
                    if (((FileInfo)mFiles[mFilesIndex]).FullName.Contains(Settings.Extension))
                    {
                        mPrevPacinet = mCurrentPacinet;
                        ////////////////////////////////////////////////////////////////
                        mCurrentPacinet = new Pacient();
                        mCurrentPacinet.Name = ((FileInfo)mFiles[mFilesIndex]).FullName.Replace(Settings.Source, string.Empty);
                        ReadPacient(ref mCurrentPacinet);
                        return mCurrentPacinet;
                    }
                }
                return null;
            }
        }

        public Pacient Prev
        {
            get { return mPrevPacinet; }
        }

        public bool Save(Pacient pacient)
        {
            WritePacient(ref pacient); 
            return false;
        }
        /*
        private void FilesCounter(FileInfo file)
        {
            mFilesCount++;
        }

        public delegate void OnFileEventHandler(OnFileEventArgs e);

        public event OnFileEventHandler OnFile;

        private void Recursion(FileSystemInfo[] FSInfo, FileProcess fp)
        {
            foreach (FileSystemInfo f in FSInfo)
            {
                if (f is DirectoryInfo)
                {
                    mDirectoriesCount++;

                    DirectoryInfo dInfo = (DirectoryInfo)f;
                    Recursion(dInfo.GetFileSystemInfos(), fp);
                }
                else if (f is FileInfo)
                {
                    if ((Settings.Extension != string.Empty) && (((FileInfo)f).Extension.ToUpper() == Settings.Extension))
                    {
                        if (this.OnFile != null)
                            this.OnFile(new OnFileEventArgs((FileInfo)f));

                        if (fp != null)
                            fp((FileInfo)f);
                    }
                }
            }
        }

        public void Process(FileProcess fp)
        {
            Recursion(mSrcDirectory.GetFileSystemInfos(), fp);
        }
                */
        private void ReadPacient(ref Pacient pacient)
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader( mSrcDirectory.FullName + pacient.Name);
                pacient.Body = reader.ReadToEnd();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        private void WritePacient(ref Pacient pacient)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(mDesDirectory.FullName + pacient.Name);
                writer.Write(pacient.Body);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }
    }
}
