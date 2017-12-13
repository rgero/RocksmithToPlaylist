using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace RocksmithToPlaylist
{
    class Program
    {

        static List<SongInfo> masterSongList = new List<SongInfo>();

        static List<string> scanDirectory(string fileExt, string path)
        {
            string ext = "*_p." + fileExt;
            return Directory.GetFiles(path, ext, SearchOption.AllDirectories).ToList<string>();
        }

        static void Main(string[] args)
        {

            /* Here is the intended workflow
                    
                - Get list of files
                - Parse each file
                - Write to CSV file.

            */


            // These are hardcoded paths. I should find these in another way. Perhaps through the registry?
            var dlcDirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Rocksmith2014\\dlc\\";
            List<string> files = scanDirectory("psarc", dlcDirectory);

            files.Add("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Rocksmith2014\\songs.psarc");

            // Constructing the list of songs
            foreach (string i in files)
            {
                var browser = new PsarcBrowser(i);
                var songList = browser.GetSongList();
                foreach (var song in songList)
                {
                    masterSongList.Add(song);
                }

            }

            // Write the songs out to a txt file ('csv' format)
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\\Test\\Test.txt"))
            {
                foreach (SongInfo song in masterSongList)
                {
                    string line = String.Format("{0}\t{1}\t{2}\t{3}",
                            song.Artist, song.Title, song.Album, song.Year,
                            string.Join(", ", song.Arrangements));

                    file.WriteLine(line);
                }
            }
        }
    }
}
