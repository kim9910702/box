﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GBManager.Interfaces
{
    public interface IBrightnessService
    {
        float Get();
        bool Set(float brightness);
    }
}