using System;
using System.Collections.Generic;
using System.IO;

namespace Dr.Pacients
{
    public class DirectorySettings
    {
        public String Source
        {
            set { mSourceDirectory = value; }
            get { return mSourceDirectory; }
        } private String mSourceDirectory;

        public String Destination
        {
            set { mDestinationDirectory = value; }
            get { return mDestinationDirectory; }
        } private String mDestinationDirectory;

        public bool Subfolders
        {
            set { mAllDirectories = (value == true) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly; }
            get { return mAllDirectories == (SearchOption.AllDirectories) ? true : false; }
        } private SearchOption mAllDirectories;

        public String Extension
        {
            set { mExtension = value; }
            get { return mExtension; }
        } private String mExtension;


        public SearchOption AllDirectories
        {
            get { return mAllDirectories; }
        }

        public DirectorySettings()
        {

        }
    }
}
