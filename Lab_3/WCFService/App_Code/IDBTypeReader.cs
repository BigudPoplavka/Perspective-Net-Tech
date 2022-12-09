using PipeVolumeCalcService;
using System;
using System.Collections.Generic;

public interface IDBTypeReader
{
    void Read(string path);
    bool IsValidUser(User user);
}