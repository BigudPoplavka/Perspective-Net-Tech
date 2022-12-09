using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using PipeVolumeCalcService;

public class DBReader
{
    public IDBTypeReader DBTypeReader;
    private string _path;

    public DBReader(string path, IDBTypeReader dBTypeReader)
    {
        _path = path;
        DBTypeReader = dBTypeReader;
    }

    public void ReadDB()
    {
        DBTypeReader.Read(_path);
    }
}

