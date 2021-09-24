﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckWebService
{
    public class PassportCsvToXml : CsvToXml
    {
        protected override int SurnameCellIndex { get { return 0; } }
        protected override int FirstnameCellIndex { get { return 1; } }
        protected override int PatronymicCellIndex { get { return 2; } }
        protected override int CneNumberCellIndex { get { return -1; } }
        protected override int PassportNumberCellIndex { get { return 4; } }
        protected override int PassportSeriaCellIndex { get { return 3; } }
        protected override int TypographicNumberCellIndex { get { return -1; } }
    }
}
