using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionsMobile.Services
{
    public interface IFileService
    {
        string SaveFile(string filename, byte[] data);        
    }
    
}
