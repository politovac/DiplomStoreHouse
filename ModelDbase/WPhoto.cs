﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomStoreHouse.ModelDbase
{
    public partial class WarehouseDivision
    {
        public string PhotoFull2
        {
            get
            {
                if (Image == null)
                {
                    return null;
                }
                else
                {
                    string namePhoto = Directory.GetCurrentDirectory() +
                        "\\images\\" + Image;
                    return namePhoto;
                }
            }
        }
    }
}
