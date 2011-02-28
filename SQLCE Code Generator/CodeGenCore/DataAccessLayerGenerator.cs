﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public abstract class DataAccessLayerGenerator
    {
        protected StringBuilder code;
        protected Table table;

        protected DataAccessLayerGenerator(StringBuilder code, Table table)
        {
            this.code = code;
            this.table = table;
        }

        protected void GetReaderValues(Table table)
        {
            foreach (var column in table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    code.AppendLine("\t\t\t\t\t\titem." + column.Key + " = (" + column.Value + "?) (stream[\"" + column.Key + "\"] is System.DBNull ? null : stream[\"" + column.Key + "\"]);");
                else
                    code.AppendLine("\t\t\t\t\t\titem." + column.Key + " = stream[\"" + column.Key + "\"] as " + column.Value + ";");
            }
        }

        public string GetCode()
        {
            return code.ToString();
        }

        public abstract void GenerateSelectAll();
        public abstract void GenerateSelectTop();
        public abstract void GenerateCreateIgnoringPrimaryKey();
        public abstract void GenerateCreateUsingAllColumns();
        public abstract void GenerateDelete();
        public abstract void GenerateDeleteAll();
        public abstract void GenerateSaveChanges();
    }
}