using System;
using System.IO;
using System.IO.Compression;
using aviationLib;

namespace dofLoader
{
    class Program
    {
        static Char[] ors = new Char[9];
        static Char[] country = new Char[2];
        static Char[] state = new Char[2];
        static Char[] city = new Char[15];
        static Char[] latitude = new Char[12];
        static Char[] longitude = new Char[13];
        static Char[] type = new Char[18];
        static Char[] agl = new Char[5];
        static Char[] msl = new Char[5];

        static StreamWriter ofile = new StreamWriter("obstacle.txt");

        static void Main(String[] args)
        {
            String userprofileFolder = Environment.GetEnvironmentVariable("USERPROFILE");
            String[] fileEntries = Directory.GetFiles(userprofileFolder + "\\Downloads\\", "DOF_*.zip");

            ZipArchive archive = ZipFile.OpenRead(fileEntries[0]);
            ZipArchiveEntry entry = archive.GetEntry("DOF.DAT");
            entry.ExtractToFile("DOF.txt", true);

            StreamReader file = new StreamReader("DOF.txt");

            String rec = file.ReadLine();

            rec = file.ReadLine();
            rec = file.ReadLine();
            rec = file.ReadLine();
            rec = file.ReadLine();

            while (!file.EndOfStream)
            {
                ProcessRecord(rec);
                rec = file.ReadLine();
            }

            ProcessRecord(rec);

            file.Close();
            ofile.Close();
        }

        static void ProcessRecord(String record)
        {
            ors = record.ToCharArray(0, 9);
            String s = new String(ors).Trim();

            country = record.ToCharArray(12, 2);

            if (String.Compare(new String(country), "US") == 0)
            {
                ofile.Write(s);
                ofile.Write('~');

                s = new String(country).Trim();
                ofile.Write(s);
                ofile.Write('~');

                state = record.ToCharArray(15, 2);
                s = new String(state).Trim();
                ofile.Write(s);
                ofile.Write('~');

                city = record.ToCharArray(18, 15);
                s = new String(city).Trim();
                ofile.Write(s);
                ofile.Write('~');

                latitude = record.ToCharArray(35, 12);
                longitude = record.ToCharArray(48, 13);

                LatLon ll = new LatLon(new String(latitude).Replace(' ', '-').Trim(), new String(longitude).Replace(' ', '-').Trim());
                ofile.Write(ll.formattedLat);
                ofile.Write('~');

                ofile.Write(ll.formattedLon);
                ofile.Write('~');

                type = record.ToCharArray(62, 18);
                s = new String(type).Trim();
                ofile.Write(s);
                ofile.Write('~');

                agl = record.ToCharArray(83, 5);
                s = new String(agl).Trim();
                ofile.Write(s);
                ofile.Write('~');

                msl = record.ToCharArray(89, 5);
                s = new String(msl).Trim();
                ofile.Write(s);
                ofile.Write(ofile.NewLine);
            }
        }
    }

}
