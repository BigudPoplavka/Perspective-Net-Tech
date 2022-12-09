using PipeVolumeCalcService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


public class DBTypeCSVReader : IDBTypeReader
{
    private Dictionary<string, string> _data;

    public DBTypeCSVReader()
    {
        _data = new Dictionary<string, string>();
    }

    public bool IsValidUser(User user)
    {
        try
        {
            if (_data.ContainsKey(user.Login))
            {
                if (_data[user.Login] == user.Password)
                {
                    return true;
                }
            }
        }
        catch (ArgumentNullException e)
        {
            return false;
        }
        return false;
    }

    public void Read(string path)
    {
        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string DBrow;

                while ((DBrow = reader.ReadLine()) != null)
                {
                    var DBcol = DBrow.Split(new char[] { ',' });
                    _data.Add(DBcol[0], DBcol[1]);
                }
            }
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}