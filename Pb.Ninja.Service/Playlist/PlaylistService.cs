using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Ninja.Service.Playlist
{
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    public class PlaylistService
    {
       
        /**
         * Function to get playlist from local resource (need to be converted into API service)
         */
        public async Task<List<string>> GetSongListAsync()
        {
            
            try
            {
                string[] files = File.ReadAllLines(@"Data\playlist.txt");
                return new List<string>(files);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                // TODO: Handle the System.IO.DirectoryNotFoundException
                return new List<string>();
            }

           
        }
    }
}
