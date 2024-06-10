using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using TTF_Project_version1;

namespace TTF_Project_Tests
{
    [TestClass]
    public class Form1Tests
    {
        [TestMethod]
        public void ReadCsvFile_ValidFilePath_ReturnsDataTable()
        {
            // Arrange
            string filePath = @"C:\Users\jorda\Downloads\TTFproducts.csv";
            Form1 form = new Form1();

            // Act
            DataTable result = form.ReadCsvFile(filePath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(12, result.Rows.Count);
            Assert.AreEqual("ID", result.Columns[0].ColumnName);
        }
    }
}
